using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.SendBack;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Runtime manager sendback
    /// 退回流程运行时
    /// </summary>
    internal class WfRuntimeManagerSendBack : WfRuntimeManager
    {
        /// <summary>
        /// The processing logic of sendback operation
        /// 退回操作的处理逻辑
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            var runner = this.AppRunner;
            var runningNode = this.RunningActivityInstance;
            var sendbackOperation = base.SendBackOperation;

            if (runningNode.MIHostActivityInstanceID == null)
            {
                //普通节点
                //Normal task
                sendbackOperation.CurrentNodeOperationType = SendBackOperationTypeEnum.Normal;
                foreach (var step in runner.NextActivityPerformers)
                {
                    var prevActivityGUID = step.Key;
                    var prevActivity = this.ProcessModel.GetActivity(prevActivityGUID);
                    sendbackOperation.BackwardToTaskActivity = prevActivity;

                    if (this.ProcessModel.IsTaskNode(prevActivity))
                    {
                        //退回到也是普通节点
                        //Sendback to a regular node as well
                        sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.Normal;
                    }
                    else if (this.ProcessModel.IsMINode(prevActivity) == true)
                    {
                        var aim = new ActivityInstanceManager();
                        var prevActivityInstance = aim.GetActivityInstanceLatest(runningNode.ProcessInstanceID, prevActivityGUID, session);
                        if (prevActivityInstance.MIHostActivityInstanceID == null)
                        {
                            //虽然流程定义是加签节点，但是运行时没有加签实例，就当做普通任务节点处理
                            //Although the process definition is a signforward node, there is no signed instance at runtime,
                            //so it is treated as a regular task node
                            sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.Normal;
                        }
                        else
                        {
                            if (this.ProcessModel.IsMISequence(prevActivity) == true)
                            {
                                //退回到会签节点
                                //模式：串行会签的最后一步
                                //Return to the signing node
                                //Mode: The final step of serial countersignature
                                sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.MISPreviousIsLastOne;
                            }
                            else
                            {
                                //退回到会签节点
                                //模式：并行会签节点
                                //Return to the signing node
                                //Mode: Parallel Signing Node
                                sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.MIPAllIsInCompletedState;
                            }
                        }
                    }
                    else
                    {
                        throw new ApplicationException(LocalizeHelper.GetEngineMessage("wfruntimemanagersendback.ExecuteInstanceImp.error"));
                    }

                    //执行退回方法
                    //只要出现多个步骤人员选项，就需要生成同样的多笔退回活动记录和相应的待办任务
                    //Execute sendback method
                    //As long as multiple step personnel options appear, it is necessary to generate the same multiple return activity records and corresponding to-do tasks
                    var performerList = step.Value;
                    DistributeEachPerformerTask(sendbackOperation, step.Value, session);
                }
            }
            else
            {
                //会签节点
                //Multiple instance
                sendbackOperation.CurrentNodeOperationType = SendBackOperationTypeEnum.MultipleInstance;
                foreach (var step in runner.NextActivityPerformers)
                {
                    var prevActivityGUID = step.Key;
                    var prevActivity = this.ProcessModel.GetActivity(prevActivityGUID);
                    sendbackOperation.BackwardToTaskActivity = prevActivity;

                    //判断会签模式的子类型，然后进行退回处理
                    //Determine the subtype of the countersignature mode and proceed with the return process
                    if (runningNode.CompleteOrder == 1)
                    {
                        //只有串行模式下有CompleteOrder的值为 1
                        //串行模式多实例的第一个执行节点，此时可退回到上一步
                        //Only in sequence mode, the value of CompleteOrder is 1
                        //The first execution node of multiple instances in sequence mode, which can be reverted back to the previous step
                        sendbackOperation.CurrentMultipleInstanceDetailType = SendBackOperationTypeEnum.MISFirstOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder > 1)
                    {
                        //已经是中间节点，只能退回到上一步多实例子节点
                        //It is already an intermediate node and can only be reverted back to the previous step of the multi instance node
                        sendbackOperation.CurrentMultipleInstanceDetailType = SendBackOperationTypeEnum.MISOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder == -1)
                    {
                        sendbackOperation.CurrentMultipleInstanceDetailType = SendBackOperationTypeEnum.MIPOneIsRunning;
                    }
                    else
                    {
                        throw new ApplicationException(LocalizeHelper.GetEngineMessage("wfruntimemanagersendback.ExecuteInstanceImp.error"));
                    }

                    //执行退回方法
                    //只要出现多个步骤人员选项，就需要生成同样的多笔退回活动记录和相应的待办任务
                    //Execute return method
                    //As long as multiple step personnel options appear,
                    //it is necessary to generate the same multiple sendback activity records and corresponding to-do tasks
                    var performerList = step.Value;
                    DistributeEachPerformerTask(sendbackOperation, step.Value, session);
                }
            }

            //更新运行节点状态
            //Update the status of running nodes
            SetRunningNodeSendBack(session);
            result.Status = WfExecutedStatus.Success;
        }

        /// <summary>
        /// Generate tasks for the executor
        /// 给执行者生成任务
        /// </summary>
        /// <param name="sendbackOperation"></param>
        /// <param name="performerList"></param>
        /// <param name="session"></param>
        private void DistributeEachPerformerTask(SendBackOperation sendbackOperation,
            PerformerList performerList,
            IDbSession session)
        {
            foreach (var performer in performerList)
            {
                var nodeSendBack = NodeSendBackFactory.CreateNodeReverter(sendbackOperation, session);
                sendbackOperation.BackwardToTaskPerformer = performer;
                nodeSendBack.Execute();
            }
        }

        /// <summary>
        /// Update the current running node status to sendback status
        /// 更新当前运行节点状态为退回状态
        /// </summary>
        /// <param name="session"></param>
        private void SetRunningNodeSendBack(IDbSession session)
        {
            //更新当前任务记录为退回状态
            //Update the current task record to sendback status
            var tm = new TaskManager();
            tm.SendBack(base.TaskView.TaskID, base.SendBackOperation.ActivityResource.AppRunner, session);

            //更新当前办理节点的状态（从准备或运行状态更新为退回状态）
            //Update the status of the current processing node (from ready or running status to sendback status)
            var aim = new ActivityInstanceManager();
            aim.SendBack(base.SendBackOperation.BackwardFromActivityInstance.ID,
                base.SendBackOperation.ActivityResource.AppRunner,
                session);
        }
    }
}
