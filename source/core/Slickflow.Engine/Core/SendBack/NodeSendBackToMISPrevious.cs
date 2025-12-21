using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;

namespace Slickflow.Engine.Core.SendBack
{
    /// <summary>
    /// Sign together type sendback processor
    /// The revocation operation of the last multi instance node with multiple signatures
    /// 会签类型的退回处理器
    /// 最后一个会签多实例子节点的撤销操作
    /// </summary>
    internal class NodeSendBackToMISPrevious : NodeSendBack
    {
        internal NodeSendBackToMISPrevious(SendBackOperation sendbackOperation, IDbSession session)
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
            var previousActivityInstance = base.ActivityInstanceManager.GetPreviousActivityInstanceSimple(runningNode,
                base.SendBackOperation.BackwardToTaskActivity.ActivityId,
                base.Session);

            //只退回到最后一个实例子节点的办理位置
            //Only return to the processing location of the last example node
            CreateBackwardActivityTaskTransitionOfLastMultipleInstance(base.SendBackOperation.ProcessInstance,
                base.SendBackOperation.BackwardFromActivityInstance,
                previousActivityInstance,
                base.SendBackOperation.BackwardType,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.SendBackOperation.ActivityResource,
                base.Session);

            //更新主节点状态从完成状态->挂起状态
            //Update the status of the master node from completed state to suspended state
            base.ActivityInstanceManager.Resuspend(previousActivityInstance.MainActivityInstanceId.Value,
                base.Session,
                base.SendBackOperation.ActivityResource.AppRunner);
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
            //创建回滚到的节点信息
            //Create rollback to node information
            var rollbackPreviousActivityInstance = base.ActivityInstanceManager.CreateBackwardActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceId,
                processInstance.AppInstanceCode,
                processInstance.Id,
                processInstance.ProcessId,
                this.SendBackOperation.BackwardToTaskActivity,
                backwardType,
                fromActivityInstance.Id,
                originalBackwardToActivityInstance.Id,
                activityResource.AppRunner);

            rollbackPreviousActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            rollbackPreviousActivityInstance.MainActivityInstanceId = originalBackwardToActivityInstance.MainActivityInstanceId;
            rollbackPreviousActivityInstance.CompleteOrder = originalBackwardToActivityInstance.CompleteOrder;
            rollbackPreviousActivityInstance.ComplexType = originalBackwardToActivityInstance.ComplexType;
            rollbackPreviousActivityInstance.SignForwardType = originalBackwardToActivityInstance.SignForwardType;

            //人员来自步骤列表的用户数据
            //User data of personnel from the step list
            rollbackPreviousActivityInstance.AssignedUserIds = base.SendBackOperation.BackwardToTaskPerformer.UserId;     
            rollbackPreviousActivityInstance.AssignedUserNames = base.SendBackOperation.BackwardToTaskPerformer.UserName;

            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            base.TaskManager.Insert(rollbackPreviousActivityInstance,
                base.SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                session);

            base.InsertTransitionInstance(processInstance,
                fromActivityInstance,
                rollbackPreviousActivityInstance,
                TransitionTypeEnum.Backward,
                TransitionFlyingTypeEnum.NotFlying,
                this.SendBackOperation.ActivityResource.AppRunner,
                session);
        }
    }
}
