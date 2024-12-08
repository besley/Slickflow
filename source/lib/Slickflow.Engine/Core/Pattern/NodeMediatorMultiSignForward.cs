﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Core.Result;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 加签子节点执行
    /// </summary>
    internal class NodeMediatorMultiSignForward : NodeMediator
    {
        internal NodeMediatorMultiSignForward(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// 执行普通任务节点
        /// 1. 当设置任务完成时，同时设置活动完成
        /// 2. 当实例化活动数据时，产生新的任务数据
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            try
            {
                //执行前Action列表
                OnBeforeExecuteWorkItem();

                //完成当前的任务节点
                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskID,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                //执行后Action列表
                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                if (canContinueForwardCurrentNode)
                {
                    ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, this.Session);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 完成任务实例
        /// </summary>
        /// <param name="taskID">任务视图</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>        
        internal bool CompleteWorkItem(int? taskID,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            WfAppRunner runner = new WfAppRunner
            {
                UserID = activityResource.AppRunner.UserID,         //避免taskview为空
                UserName = activityResource.AppRunner.UserName
            };

            //流程强制拉取向前跳转时，没有运行人的任务实例
            if (taskID != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                base.TaskManager.Complete(taskID.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            base.ActivityInstanceManager.Complete(base.LinkContext.FromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            base.LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

            //如果是非正常流转模式，不用判断通过率，直接跳转
            if (base.ActivityForwardContext.IsNotParsedByTransition == true)
            {
                canContinueForwardCurrentNode = true;
                return canContinueForwardCurrentNode;
            }

            //多实例会签和加签处理
            //先判断是否是会签和加签类型
            //主节点不为空时不发起加签可以正常运行
            var signforwardType = (SignForwardTypeEnum)Enum.Parse(typeof(SignForwardTypeEnum),
                base.ActivityForwardContext.FromActivityInstance.SignForwardType.Value.ToString());
            if (base.LinkContext.FromActivityInstance.MIHostActivityInstanceID != null
                && signforwardType != SignForwardTypeEnum.None)
            {
                //取出主节点信息
                var mainNodeIndex = base.LinkContext.FromActivityInstance.MIHostActivityInstanceID.Value;
                var mainActivityInstance = base.ActivityInstanceManager.GetById(mainNodeIndex);

                //判断加签是否全部完成，如果是，则流转到下一步，否则不能流转
                if (signforwardType == SignForwardTypeEnum.SignForwardBehind
                    || signforwardType == SignForwardTypeEnum.SignForwardBefore)
                {
                    //取出处于多实例节点列表
                    var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithState(
                        mainNodeIndex,
                        base.LinkContext.FromActivityInstance.ProcessInstanceID,
                        (short)ActivityStateEnum.Suspended,
                        session).ToList<ActivityInstanceEntity>();

                    short maxOrder = 0;
                    if (sqList != null && sqList.Count > 0)
                    {
                        //取出最大执行节点
                        maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                    }
                    else
                    {
                        //最后一个执行节点
                        maxOrder = (short)base.LinkContext.FromActivityInstance.CompleteOrder.Value;
                    }

                    if (mainActivityInstance.CompareType == (short)CompareTypeEnum.Count || mainActivityInstance.CompareType == null)
                    {
                        //加签通过率
                        if (mainActivityInstance.CompleteOrder != null && mainActivityInstance.CompleteOrder <= maxOrder)
                        {
                            maxOrder = (short)mainActivityInstance.CompleteOrder;
                        }

                        if (base.LinkContext.FromActivityInstance.CompleteOrder < maxOrder)
                        {
                            //设置下一个节点进入等待办理状态
                            var currentNodeIndex = (short)base.LinkContext.FromActivityInstance.CompleteOrder.Value;
                            var nextActivityInstance = sqList[0];
                            nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                            base.ActivityInstanceManager.Update(nextActivityInstance, session);

                            //更新主节点的执行人员列表
                            mainActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(
                                base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers);
                            base.ActivityInstanceManager.Update(mainActivityInstance, session);
                            canContinueForwardCurrentNode = false;
                            base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.ForwardToNextSequenceTask;
                        }
                        else if (base.LinkContext.FromActivityInstance.CompleteOrder == maxOrder)
                        {
                            var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                    mainActivityInstance, session);
                            if (passed)
                            {
                                //最后一个节点执行完，主节点进入完成状态，整个流程向下执行
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, session);
                                //更新未办理完成节点状态为取消状态
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.ID, session, runner);
                            }
                            else
                            {
                                canContinueForwardCurrentNode = false;
                                base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.NotEnoughApprovalBranchesCount;
                            }
                        }
                    }
                    else
                    {
                        if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)//串行会签未设置通过率的判断
                            mainActivityInstance.CompleteOrder = 1;

                        if ((base.LinkContext.FromActivityInstance.CompleteOrder * 0.01) / (maxOrder * 0.01) >= mainActivityInstance.CompleteOrder)
                        {
                            var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                mainActivityInstance, session);
                            if (passed)
                            {
                                //完成最后一个会签任务，会签主节点状态由挂起设置为完成状态
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, session);
                                //更新未办理完成节点状态为取消状态
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.ID, session, runner);
                            }
                            else
                            {
                                canContinueForwardCurrentNode = false;
                                base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.NotEnoughApprovalBranchesCount;
                            }
                        }
                        else
                        {
                            //设置下一个任务进入准备状态
                            var nextActivityInstance = sqList[0];     //始终取第一条挂起实例
                            nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                            base.ActivityInstanceManager.Update(nextActivityInstance, session);

                            //更新主节点的执行人员列表
                            mainActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(
                                base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers);
                            base.ActivityInstanceManager.Update(mainActivityInstance, session);
                            canContinueForwardCurrentNode = false;
                            base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.ForwardToNextSequenceTask;
                        }
                    }
                }
                else if (signforwardType == SignForwardTypeEnum.SignForwardParallel)
                {
                    //取出处于多实例节点列表
                    var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithState(
                        mainNodeIndex,
                        base.LinkContext.FromActivityInstance.ProcessInstanceID,
                        null,
                        session).ToList<ActivityInstanceEntity>();

                    //并行加签，按照通过率来决定是否标识当前节点完成
                    var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                    var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed)
                        .ToList<ActivityInstanceEntity>()
                        .Count();
                    if (mainActivityInstance.CompareType == null || mainActivityInstance.CompareType == (short)CompareTypeEnum.Percentage)
                    {
                        if (mainActivityInstance.CompleteOrder > 1)//并行加签通过率的判断
                            mainActivityInstance.CompleteOrder = 1;

                        if ((completedCount * 0.01) / (allCount * 0.01) >= mainActivityInstance.CompleteOrder)
                        {
                            var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                    mainActivityInstance, session);
                            if (passed)
                            {
                                base.ActivityForwardContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, session);
                                //更新未办理完成节点状态为取消状态
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.ID, session, runner);
                            }
                            else
                            {
                                canContinueForwardCurrentNode = false;
                                base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.NotEnoughApprovalBranchesCount;
                            }
                        }
                        else
                        {
                            //更新主节点的执行人员列表
                            mainActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(
                                base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers);
                            base.ActivityInstanceManager.Update(mainActivityInstance, session);
                            canContinueForwardCurrentNode = false;
                            base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.WaitingForCompletedMore;
                        }
                    }
                    else
                    {
                        //串行加签通过率（按人数判断）
                        if (mainActivityInstance.CompleteOrder != null && mainActivityInstance.CompleteOrder > allCount)
                        {
                            mainActivityInstance.CompleteOrder = allCount;
                        }

                        if (mainActivityInstance.CompleteOrder > completedCount)
                        {
                            //更新主节点的执行人员列表
                            mainActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(
                                base.ActivityForwardContext.ActivityResource.AppRunner.NextActivityPerformers);
                            base.ActivityInstanceManager.Update(mainActivityInstance, session);
                            canContinueForwardCurrentNode = false;
                            base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.WaitingForCompletedMore;
                        }
                        else if (mainActivityInstance.CompleteOrder == completedCount)
                        {
                            var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                    mainActivityInstance, session);
                            if (passed)
                            {
                                base.ActivityForwardContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, base.Session);
                                //更新未办理完成节点状态为取消状态
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.ID, session, runner);
                            }
                            else
                            {
                                canContinueForwardCurrentNode = false;
                                base.WfNodeMediatedResult.Feedback = WfNodeMediatedFeedback.NotEnoughApprovalBranchesCount;
                            }
                        }
                    }
                }
            }
            return canContinueForwardCurrentNode;
        }
    }
}
