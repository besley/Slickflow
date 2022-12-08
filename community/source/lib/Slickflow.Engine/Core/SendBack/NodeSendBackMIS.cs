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
    /// 串行会签内部退回处理
    /// </summary>
    internal class NodeSendBackMIS : NodeSendBack
    {
        internal NodeSendBackMIS(SendBackOperation sendbackOperation, IDbSession session)
            : base(sendbackOperation, session)
        {

        }

        /// <summary>
        /// 执行退回操作
        /// </summary>
        internal override void Execute()
        {
            var runningNode = base.SendBackOperation.BackwardFromActivityInstance;
            //创建撤销到上一步的节点记录
            var psc = new PreviousStepChecker();
            var previousAdjacentBrotherNode = psc.GetPreviousOfMultipleInstanceNode(
                runningNode.MIHostActivityInstanceID.Value,
                runningNode.ID,
                runningNode.CompleteOrder.Value);

            CreateBackwardActivityTaskOfInnerMultipleInstance(this.SendBackOperation.ProcessInstance,
                previousAdjacentBrotherNode,
                base.SendBackOperation.BackwardType,
                base.SendBackOperation.BackwardFromActivityInstance.ID,
                base.SendBackOperation.ActivityResource,
                base.Session);

            //创建新的一条待办状态的记录，用于下次执行
            var newSuspendNode = base.ActivityInstanceManager.CreateActivityInstanceObject(runningNode);
            newSuspendNode.ActivityState = (short)ActivityStateEnum.Suspended;
            newSuspendNode.MIHostActivityInstanceID = runningNode.MIHostActivityInstanceID;
            newSuspendNode.CompleteOrder = runningNode.CompleteOrder;
            newSuspendNode.ComplexType = runningNode.ComplexType;
            newSuspendNode.SignForwardType = runningNode.SignForwardType;
            newSuspendNode.AssignedToUserIDs = runningNode.AssignedToUserIDs;
            newSuspendNode.AssignedToUserNames = runningNode.AssignedToUserNames;

            base.ActivityInstanceManager.Insert(newSuspendNode, base.Session);

            //同时为此活动实例，创建新的任务
            base.TaskManager.Renew(runningNode, newSuspendNode, base.SendBackOperation.ActivityResource.AppRunner, base.Session);
        }

        /// <summary>
        /// 创建多实例节点之间回滚时的活动实例，任务数据
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="originalBackwardToActivityInstance">原始退回到的节点实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="backSrcActivityInstanceID">源退回节点实例ID</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
        internal void CreateBackwardActivityTaskOfInnerMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            ActivityResource activityResource,
            IDbSession session)
        {
            //创建回滚到的节点信息
            var rollbackPreviousActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                backSrcActivityInstanceID,
                originalBackwardToActivityInstance.ID,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MIHostActivityInstanceID = originalBackwardToActivityInstance.MIHostActivityInstanceID;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;
            //人员来自步骤列表的用户数据
            rollbackPreviousActivityInstance.AssignedToUserIDs = base.SendBackOperation.BackwardToTaskPerformer.UserID;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = base.SendBackOperation.BackwardToTaskPerformer.UserName;

            //插入新活动实例数据
            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            //创建新任务数据
            base.TaskManager.Insert(rollbackPreviousActivityInstance,
                base.SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                base.Session);
        }
    }
}
