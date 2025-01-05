using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// Sendback processor for task types
    /// Sequence signature, the first node is in pending status
    /// 任务类型的退回处理器
    /// 串行会签，第一个节点处于待办
    /// </summary>
    internal class NodeSendBackMISReady : NodeSendBack
    {
        internal NodeSendBackMISReady(SendBackOperation sendbackOperation, IDbSession session)
            : base(sendbackOperation, session)
        {

        }

        /// <summary>
        /// Execute method
        /// </summary>
        internal override void Execute()
        {
            var runner = base.SendBackOperation.ActivityResource.AppRunner;
            var runningNode = base.SendBackOperation.BackwardFromActivityInstance;
            var runningMainNode = base.ActivityInstanceManager.GetById(runningNode.MIHostActivityInstanceID.Value);
            var previousActivityInstance = base.ActivityInstanceManager.GetPreviousActivityInstanceSimple(runningNode,
                base.SendBackOperation.BackwardToTaskActivity.ActivityGUID,
                base.Session);

            //创建撤销到上一步的节点记录
            //Create a node record to sendback to the previous step
            CreateBackwardActivityTaskTransitionOfLastMultipleInstance(base.SendBackOperation.ProcessInstance,
                runningMainNode,
                previousActivityInstance,
                base.SendBackOperation.BackwardType,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.SendBackOperation.ActivityResource,
                base.Session);

            //如果上一步节点也是会签节点，更新主节点状态为运行状态
            //If the previous node is also a co signing node, update the main node status to running status
            if (previousActivityInstance.ActivityType == (short)ActivityTypeEnum.MultiSignNode)
            {
                base.ActivityInstanceManager.Rerun(previousActivityInstance.MIHostActivityInstanceID.Value,
                    base.Session, runner);
            }
        }

        /// <summary>
        /// The revocation operation of the last multi instance node with multiple signatures
        /// 最后一个会签多实例子节点的撤销操作
        /// </summary>
        internal void CreateBackwardActivityTaskTransitionOfLastMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var rollbackPreviousActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                fromActivityInstance.ID,
                originalBackwardToActivityInstance.ID,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MIHostActivityInstanceID = originalBackwardToActivityInstance.MIHostActivityInstanceID;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;
            //人员来自步骤列表的用户数据
            //User data of performer from the step list
            rollbackPreviousActivityInstance.AssignedToUserIDs = base.SendBackOperation.BackwardToTaskPerformer.UserID;     
            rollbackPreviousActivityInstance.AssignedToUserNames = base.SendBackOperation.BackwardToTaskPerformer.UserName;

            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            base.TaskManager.Insert(rollbackPreviousActivityInstance,
                base.SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                session);

            base.InsertTransitionInstance(processInstance,
                fromActivityInstance,
                rollbackPreviousActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
