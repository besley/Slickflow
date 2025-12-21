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
using IronPython.Compiler.Ast;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static IronPython.Modules._ast;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Execution of signed child nodes
    /// 加签子节点执行
    /// </summary>
    internal class NodeMediatorMultiSignForward : NodeMediator
    {
        internal NodeMediatorMultiSignForward(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                OnBeforeExecuteWorkItem();

                bool canContinueForwardCurrentNode = CompleteWorkItem(ActivityForwardContext.TaskId,
                    ActivityForwardContext.ActivityResource,
                    this.Session);

                OnAfterExecuteWorkItem();

                //获取下一步节点列表：并继续执行
                //Get the next node list: and continue execution
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
        /// Complete work item
        /// </summary>     
        internal bool CompleteWorkItem(int? taskId,
            ActivityResource activityResource,
            IDbSession session)
        {
            bool canContinueForwardCurrentNode = true;

            WfAppRunner runner = new WfAppRunner
            {
                UserId = activityResource.AppRunner.UserId,         
                UserName = activityResource.AppRunner.UserName
            };

            if (taskId != null)
            {
                //完成本任务，返回任务已经转移到下一个会签任务，不继续执行其它节点
                //Complete this task, return that the task has been transferred to the next co signing task,
                //and do not continue to execute other nodes
                base.TaskManager.Complete(taskId.Value, activityResource.AppRunner, session);
            }

            //设置活动节点的状态为完成状态
            //Set the status of the activity node to complete status
            base.ActivityInstanceManager.Complete(base.LinkContext.FromActivityInstance.Id,
                activityResource.AppRunner,
                session);

            base.LinkContext.FromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

            //如果是非正常流转模式，不用判断通过率，直接跳转
            ////If it is an abnormal circulation mode, there is no need to judge the pass rate, just jump directly
            if (base.ActivityForwardContext.IsNotParsedByTransition == true)
            {
                canContinueForwardCurrentNode = true;
                return canContinueForwardCurrentNode;
            }

            //多实例会签和加签处理
            //先判断是否是会签和加签类型
            //主节点不为空时不发起加签可以正常运行
            //Multiple practical meetings for signing and adding signatures
            //First, determine whether it is a countersignature or countersignature type
            //When the master node is not empty, it can run normally without initiating signing
            var signforwardType = (SignForwardTypeEnum)Enum.Parse(typeof(SignForwardTypeEnum),
                base.ActivityForwardContext.FromActivityInstance.SignForwardType.Value.ToString());
            if (base.LinkContext.FromActivityInstance.MainActivityInstanceId != null
                && signforwardType != SignForwardTypeEnum.None)
            {
                //取出主节点信息
                //Retrieve master node information
                var mainNodeIndex = base.LinkContext.FromActivityInstance.MainActivityInstanceId.Value;
                var mainActivityInstance = base.ActivityInstanceManager.GetById(mainNodeIndex);

                //判断加签是否全部完成，如果是，则流转到下一步，否则不能流转
                //Check if all signatures have been added. If so, proceed to the next step. Otherwise, do not proceed
                if (signforwardType == SignForwardTypeEnum.SignForwardBehind
                    || signforwardType == SignForwardTypeEnum.SignForwardBefore)
                {
                    //取出处于挂起状态的多实例节点列表
                    //Retrieve the list of multi instance nodes in a suspended state
                    var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithState(
                        mainNodeIndex,
                        base.LinkContext.FromActivityInstance.ProcessInstanceId,
                        (short)ActivityStateEnum.Suspended,
                        session).ToList<ActivityInstanceEntity>();

                    short maxOrder = 0;
                    if (sqList != null && sqList.Count > 0)
                    {
                        //取出最大执行节点
                        //Retrieve the maximum execution node
                        maxOrder = (short)sqList.Max<ActivityInstanceEntity>(t => t.CompleteOrder.Value);
                    }
                    else
                    {
                        //最后一个执行节点
                        //The last execution node
                        maxOrder = (short)base.LinkContext.FromActivityInstance.CompleteOrder.Value;
                    }

                    if (mainActivityInstance.CompareType == (short)CompareTypeEnum.Count || mainActivityInstance.CompareType == null)
                    {
                        //加签通过率
                        //Approval rate of additional signatures
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
                            //Set the next node to enter the waiting processing state
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
                                //After the last node completes its execution, the main node enters the completion state,
                                //and the entire process proceeds downwards
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, session);

                                //更新未办理完成节点状态为取消状态
                                //Update the status of unfinished nodes to cancelled status
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.Id, session, runner);
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
                        //串行会签未设置通过率的判断
                        //Judgment of no set pass rate for serial countersignature
                        if (mainActivityInstance.CompleteOrder == null || mainActivityInstance.CompleteOrder > 1)
                            mainActivityInstance.CompleteOrder = 1;

                        if ((base.LinkContext.FromActivityInstance.CompleteOrder * 0.01) / (maxOrder * 0.01) >= mainActivityInstance.CompleteOrder)
                        {
                            var passed = base.ActivityInstanceManager.GetMiApprovalThresholdStatus(base.LinkContext.FromActivityInstance,
                                mainActivityInstance, session);
                            if (passed)
                            {
                                //完成最后一个会签任务，会签主节点状态由挂起设置为完成状态
                                //Complete the last countersignature task, and set the status of the countersignature master node from suspended to completed
                                mainActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
                                base.ActivityInstanceManager.Update(mainActivityInstance, session);

                                //更新未办理完成节点状态为取消状态
                                //date the status of unfinished nodes to cancelled status
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.Id, session, runner);
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
                            //Set the next task to enter preparation mode
                            var nextActivityInstance = sqList[0];     //Always take the first suspended instance
                            nextActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
                            base.ActivityInstanceManager.Update(nextActivityInstance, session);

                            //更新主节点的执行人员列表
                            //Update the list of executing personnel for the main node
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
                    //取出多实例节点列表
                    //Retrieve a list of multiple instance nodes
                    var sqList = base.ActivityInstanceManager.GetActivityMulitipleInstanceWithState(
                        mainNodeIndex,
                        base.LinkContext.FromActivityInstance.ProcessInstanceId,
                        null,
                        session).ToList<ActivityInstanceEntity>();

                    //并行加签，按照通过率来决定是否标识当前节点完成
                    //Parallel signing, determining whether to mark the current node completion based on the pass rate
                    var allCount = sqList.Where(x => x.ActivityState != (short)ActivityStateEnum.Withdrawed).ToList().Count();
                    var completedCount = sqList.Where<ActivityInstanceEntity>(w => w.ActivityState == (short)ActivityStateEnum.Completed)
                        .ToList<ActivityInstanceEntity>()
                        .Count();
                    if (mainActivityInstance.CompareType == null || mainActivityInstance.CompareType == (short)CompareTypeEnum.Percentage)
                    {
                        //并行加签通过率的判断
                        //Determination of parallel endorsement pass rate
                        if (mainActivityInstance.CompleteOrder > 1)  
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
                                //Update the status of unfinished nodes to cancelled status
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.Id, session, runner);
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
                            //Update the list of executing personnel for the main node
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
                        //Serial signature pass rate(judged by number of people)
                        if (mainActivityInstance.CompleteOrder != null && mainActivityInstance.CompleteOrder > allCount)
                        {
                            mainActivityInstance.CompleteOrder = allCount;
                        }

                        if (mainActivityInstance.CompleteOrder > completedCount)
                        {
                            //更新主节点的执行人员列表
                            //Update the list of executing personnel for the main node
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
                                //Update the status of unfinished nodes to cancelled status
                                base.ActivityInstanceManager.CancelUnCompletedMultipleInstance(mainActivityInstance.Id, session, runner);
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
