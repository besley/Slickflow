using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Parser;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// Sequence sign together internal sendback processing
    /// 串行会签内部退回处理
    /// </summary>
    internal class NodeSendBackMIS : NodeSendBack
    {
        internal NodeSendBackMIS(SendBackOperation sendbackOperation, IDbSession session)
            : base(sendbackOperation, session)
        {

        }

        /// <summary>
        /// Execute method
        /// </summary>
        internal override void Execute()
        {
            var runningNode = base.SendBackOperation.BackwardFromActivityInstance;
            //创建撤销到上一步的节点记录
            //Create a node record to undo to the previous step
            var psc = new PreviousStepChecker();
            var previousAdjacentBrotherNode = psc.GetPreviousOfMultipleInstanceNode(
                runningNode.MainActivityInstanceId.Value,
                runningNode.Id,
                runningNode.CompleteOrder.Value);

            CreateBackwardActivityTaskOfInnerMultipleInstance(this.SendBackOperation.ProcessInstance,
                previousAdjacentBrotherNode,
                base.SendBackOperation.BackwardType,
                base.SendBackOperation.BackwardFromActivityInstance.Id,
                base.SendBackOperation.ActivityResource,
                base.Session);

            //创建新的一条待办状态的记录，用于下次执行
            //Create a new record of pending status for the next execution
            var newSuspendNode = base.ActivityInstanceManager.CreateActivityInstanceObject(runningNode);
            newSuspendNode.ActivityState = (short)ActivityStateEnum.Suspended;
            newSuspendNode.MainActivityInstanceId = runningNode.MainActivityInstanceId;
            newSuspendNode.CompleteOrder = runningNode.CompleteOrder;
            newSuspendNode.ComplexType = runningNode.ComplexType;
            newSuspendNode.SignForwardType = runningNode.SignForwardType;
            newSuspendNode.AssignedUserIds = runningNode.AssignedUserIds;
            newSuspendNode.AssignedUserNames = runningNode.AssignedUserNames;

            base.ActivityInstanceManager.Insert(newSuspendNode, base.Session);

            //同时为此活动实例，创建新的任务
            //At the same time, create a new task for this activity instance
            base.TaskManager.Renew(runningNode, newSuspendNode, base.SendBackOperation.ActivityResource.AppRunner, base.Session);
        }

        /// <summary>
        /// Create active instances and task data for rollback between multiple instance nodes
        /// 创建多实例节点之间回滚时的活动实例，任务数据
        /// </summary>
        internal void CreateBackwardActivityTaskOfInnerMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceId,
            ActivityResource activityResource,
            IDbSession session)
        {
            var rollbackPreviousActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                backSrcActivityInstanceId,
                originalBackwardToActivityInstance.Id,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MainActivityInstanceId = originalBackwardToActivityInstance.MainActivityInstanceId;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;
            //人员来自步骤列表的用户数据
            //User data of performer from the step list
            rollbackPreviousActivityInstance.AssignedUserIds = base.SendBackOperation.BackwardToTaskPerformer.UserId;      
            rollbackPreviousActivityInstance.AssignedUserNames = base.SendBackOperation.BackwardToTaskPerformer.UserName;

            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            base.TaskManager.Insert(rollbackPreviousActivityInstance,
                base.SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                base.Session);
        }
    }
}
