/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
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
    /// 会签子节点执行
    /// </summary>
    internal class NodeMediatorMultiSignTogether : NodeMediator
    {
        internal NodeMediatorMultiSignTogether(ActivityForwardContext forwardContext, IDbSession session)
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

            //多实例会签和加签处理
            //先判断是否是会签和加签类型
            //主节点不为空时不发起加签可以正常运行
            var complexType = base.LinkContext.FromActivity.MultiSignDetail.ComplexType;
            if (complexType == ComplexTypeEnum.SignTogether
                || complexType == ComplexTypeEnum.SignForward && base.LinkContext.FromActivityInstance.MIHostActivityInstanceID != null)
            {
                //取出主节点信息
                var mainNodeIndex = base.LinkContext.FromActivityInstance.MIHostActivityInstanceID.Value;
                var mainActivityInstance = base.ActivityInstanceManager.GetById(mainNodeIndex);

                //串行会签和并行会签的处理
                if (complexType == ComplexTypeEnum.SignTogether)
                {
                    if (base.LinkContext.FromActivity.MultiSignDetail.MergeType == MergeTypeEnum.Sequence)    //串行会签处理
                    {
                        //取出处于多实例节点列表
                        var childList = base.ActivityInstanceManager.GetValidActivityInstanceListOfMI(mainActivityInstance.ID, 
                            mainActivityInstance.ProcessInstanceID, session);
                        var sqList = childList.Where(a => a.ActivityState == (short)ActivityStateEnum.Suspended).ToList();
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
                            //串行会签通过率（按人数判断）
                            if (base.LinkContext.FromActivityInstance.CompleteOrder < maxOrder)
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
                            else if (base.LinkContext.FromActivityInstance.CompleteOrder == maxOrder)
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
                    else if (base.LinkContext.FromActivity.MultiSignDetail.MergeType == MergeTypeEnum.Parallel)   //并行会签处理
                    {
                        //取出处于多实例节点列表
                        var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithStateBatch(mainActivityInstance.ProcessInstanceID, 
                            mainActivityInstance.ID, null, session);
                        var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed
                             && x.ActivityState != (short)ActivityStateEnum.Sendbacked).ToList().Count();
                        var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed)
                            .ToList<ActivityInstanceEntity>()
                            .Count();

                        if (mainActivityInstance.CompareType == null || mainActivityInstance.CompareType == (short)CompareTypeEnum.Percentage)
                        {
                            if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)//并行会签未设置通过率的判断
                                mainActivityInstance.CompleteOrder = 1;

                            if ((completedCount * 0.01) / (allCount * 0.01) >= mainActivityInstance.CompleteOrder)
                            {
                                var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                    mainActivityInstance, session);
                                if (passed)
                                {
                                    //如果超过约定的比例数，则执行下一步节点
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
                            //并行会签通过率（按人数判断）
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
                                    //如果超过约定的比例数，则执行下一步节点
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
                    }
                }
            }
            return canContinueForwardCurrentNode;
        }
    }
}