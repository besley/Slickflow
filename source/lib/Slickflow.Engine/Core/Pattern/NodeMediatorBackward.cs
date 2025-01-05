
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Backward Node Mediator
    /// 退回处理时的节点调节器
    /// </summary>
    internal class NodeMediatorBackward : NodeMediator
    {
        internal NodeMediatorBackward(BackwardContext backwardContext, IDbSession session)
            : base(backwardContext, session)
        {
            
        }

        internal override void ExecuteWorkItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create backward activity task transition instance
        /// 创建退回时的流转节点对象、任务和转移数据
        /// </summary>
        internal void CreateBackwardActivityTaskTransitionInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = base.CreateBackwardToActivityInstanceObject(processInstance,
                backwardType,
                fromActivityInstance.ID,
                base.BackwardContext.BackwardToTaskActivityInstance.ID,
                activityResource.AppRunner);

            toActivityInstance.ActivityState = (short)ActivityStateEnum.Ready;
            toActivityInstance = GenerateActivityAssignedUserInfo(toActivityInstance, activityResource);

            base.ActivityInstanceManager.Insert(toActivityInstance,
                session);

            base.ReturnDataContext.ActivityInstanceID = toActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = toActivityInstance.ProcessInstanceID;

            base.CreateNewTask(toActivityInstance, activityResource, session);

            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }

        /// <summary>
        /// Return is handled in the case of countersignature:
        /// The node to be returned is the countersign node
        /// 1) Instantiate all multi instance nodes under the co signing node
        /// 2) Only obtain nodes that have been processed and ensure the uniqueness of the Completed Order
        /// 退回是会签情况下的处理：
        /// 要退回的节点是会签节点
        /// 1) 全部实例化会签节点下的多实例节点
        /// 2) 只取得办理完成的节点，而且保证CompleteOrder的唯一性
        /// </summary>
        internal void CreateBackwardActivityTaskRepeatedSignTogetherMultipleInstance(ProcessInstanceEntity processInstance,
            Activity backwardToTaskActvity,
            ActivityInstanceEntity fromActivityInstance,
            BackwardTypeEnum backwardType,
            ActivityInstanceEntity previousMainInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //上一步节点是会签节点的退回处理
            //需要重新实例化会签节点上的所有办理人的任务
            //重新封装任务办理人为AssignedToUsers, AssignedToUsernames
            //The previous node is the return processing of the countersignature node
            //Need to re instantiated the tasks of all handlers on the co signing node
            //Encapsulate the task handler as Assigned To Users, AssignedToUsernames
            var performerList = AntiGenerateActivityPerformerList(previousMainInstance);

            activityResource.NextActivityPerformers.Clear();
            activityResource.NextActivityPerformers = new Dictionary<string, PerformerList>();
            activityResource.NextActivityPerformers.Add(backwardToTaskActvity.ActivityGUID, performerList);

            //重新生成会签节点的多实例数据
            //Re generate multi instance data for the countersignature node
            CreateMultipleInstance(backwardToTaskActvity, processInstance, fromActivityInstance,
                transitionGUID, transitionType, flyingType, activityResource, session);
        }

        /// <summary>
        /// Create active instances and task data for rollback between multiple instance nodes
        /// 创建多实例节点之间回滚时的活动实例，任务数据
        /// </summary>
        internal void CreateBackwardActivityTaskOfInnerMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            int backSrcActivityInstanceID,
            ActivityResource activityResource,
            IDbSession session)
        {
            //创建回滚到的节点信息
            //Create rollback to node information
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
            rollbackPreviousActivityInstance.AssignedToUserIDs = originalBackwardToActivityInstance.AssignedToUserIDs;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = originalBackwardToActivityInstance.AssignedToUserNames;

            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            base.ReturnDataContext.ActivityInstanceID = rollbackPreviousActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = rollbackPreviousActivityInstance.ProcessInstanceID;

            base.CreateNewTask(rollbackPreviousActivityInstance, activityResource, session);
        }

        /// <summary>
        /// The withdraw operation of the last multi instance node with multiple signatures
        /// 最后一个会签多实例子节点的撤销操作
        /// </summary>
        internal void CreateBackwardActivityTaskTransitionOfLastMultipleInstance(ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity originalBackwardToActivityInstance,
            BackwardTypeEnum backwardType,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            //创建回滚到的节点信息
            //Create rollback to node information
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
            rollbackPreviousActivityInstance.AssignedToUserIDs = originalBackwardToActivityInstance.AssignedToUserIDs;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = originalBackwardToActivityInstance.AssignedToUserNames;

            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);
            base.ReturnDataContext.ActivityInstanceID = rollbackPreviousActivityInstance.ID;
            base.ReturnDataContext.ProcessInstanceID = rollbackPreviousActivityInstance.ProcessInstanceID;
            base.CreateNewTask(rollbackPreviousActivityInstance, activityResource, session);

            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                rollbackPreviousActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
