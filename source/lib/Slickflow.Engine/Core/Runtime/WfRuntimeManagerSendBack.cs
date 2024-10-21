﻿using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.SendBack;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 退回流程运行时
    /// </summary>
    internal class WfRuntimeManagerSendBack : WfRuntimeManager
    {
        /// <summary>
        /// 退回操作的处理逻辑
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            var runner = this.AppRunner;
            var runningNode = this.RunningActivityInstance;
            var sendbackOperation = base.SendBackOperation;

            if (runningNode.MIHostActivityInstanceID == null)
            {
                //普通节点
                sendbackOperation.CurrentNodeOperationType = SendBackOperationTypeEnum.Normal;
                foreach (var step in runner.NextActivityPerformers)
                {
                    var prevActivityGUID = step.Key;
                    var prevActivity = this.ProcessModel.GetActivity(prevActivityGUID);
                    sendbackOperation.BackwardToTaskActivity = prevActivity;

                    if (this.ProcessModel.IsTaskNode(prevActivity))
                    {
                        //退回到也是普通节点
                        sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.Normal;
                    }
                    else if (this.ProcessModel.IsMINode(prevActivity) == true)
                    {
                        var aim = new ActivityInstanceManager();
                        var prevActivityInstance = aim.GetActivityInstanceLatest(runningNode.ProcessInstanceID, prevActivityGUID, session);
                        if (prevActivityInstance.MIHostActivityInstanceID == null)
                        {
                            //虽然流程定义是加签节点，但是运行时没有加签实例，就当做普通任务节点处理
                            sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.Normal;
                        }
                        else
                        {
                            if (this.ProcessModel.IsMISequence(prevActivity) == true)
                            {
                                //退回到会签节点
                                //模式：串行会签的最后一步
                                sendbackOperation.PreviousNodeOperationType = SendBackOperationTypeEnum.MISPreviousIsLastOne;
                            }
                            else
                            {
                                //退回到会签节点
                                //模式：并行会签节点
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
                    var performerList = step.Value;
                    DistributeEachPerformerTask(sendbackOperation, step.Value, session);
                }
            }
            else
            {
                //会签节点
                sendbackOperation.CurrentNodeOperationType = SendBackOperationTypeEnum.MultipleInstance;
                foreach (var step in runner.NextActivityPerformers)
                {
                    var prevActivityGUID = step.Key;
                    var prevActivity = this.ProcessModel.GetActivity(prevActivityGUID);
                    sendbackOperation.BackwardToTaskActivity = prevActivity;

                    //判断会签模式的子类型，然后进行退回处理
                    if (runningNode.CompleteOrder == 1)
                    {
                        //只有串行模式下有CompleteOrder的值为 1
                        //串行模式多实例的第一个执行节点，此时可退回到上一步
                        sendbackOperation.CurrentMultipleInstanceDetailType = SendBackOperationTypeEnum.MISFirstOneIsRunning;
                    }
                    else if (runningNode.CompleteOrder > 1)
                    {
                        //已经是中间节点，只能退回到上一步多实例子节点
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
                    var performerList = step.Value;
                    DistributeEachPerformerTask(sendbackOperation, step.Value, session);
                }
            }

            //更新运行节点状态
            SetRunningNodeSendBack(session);

            //构造回调函数需要的数据
            result.Status = WfExecutedStatus.Success;
        }

        /// <summary>
        /// 给执行者生成任务
        /// </summary>
        /// <param name="sendbackOperation">退回类型</param>
        /// <param name="performerList"></param>
        /// <param name="session">会话</param>
        private void DistributeEachPerformerTask(SendBackOperation sendbackOperation,
            PerformerList performerList,
            IDbSession session)
        {
            foreach (var performer in performerList)
            {
                //执行退回方法
                var nodeSendBack = NodeSendBackFactory.CreateNodeReverter(sendbackOperation, session);
                sendbackOperation.BackwardToTaskPerformer = performer;
                nodeSendBack.Execute();
            }
        }

        /// <summary>
        /// 更新当前运行节点状态为退回状态
        /// </summary>
        /// <param name="session">会话</param>
        private void SetRunningNodeSendBack(IDbSession session)
        {
            //更新当前任务记录为退回状态
            var tm = new TaskManager();
            tm.SendBack(base.TaskView.TaskID, base.SendBackOperation.ActivityResource.AppRunner, session);

            //更新当前办理节点的状态（从准备或运行状态更新为退回状态）
            var aim = new ActivityInstanceManager();
            aim.SendBack(base.SendBackOperation.BackwardFromActivityInstance.ID,
                base.SendBackOperation.ActivityResource.AppRunner,
                session);
        }
    }
}
