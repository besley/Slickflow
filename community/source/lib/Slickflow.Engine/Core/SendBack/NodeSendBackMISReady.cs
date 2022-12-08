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
        /// 执行退回操作
        /// </summary>
        internal override void Execute()
        {
            //取出运行节点信息
            var runner = base.SendBackOperation.ActivityResource.AppRunner;
            var runningNode = base.SendBackOperation.BackwardFromActivityInstance;
            var runningMainNode = base.ActivityInstanceManager.GetById(runningNode.MIHostActivityInstanceID.Value);
            var previousActivityInstance = base.ActivityInstanceManager.GetPreviousActivityInstanceSimple(runningNode,
                base.SendBackOperation.BackwardToTaskActivity.ActivityGUID,
                base.Session);

            //创建撤销到上一步的节点记录
            CreateBackwardActivityTaskTransitionOfLastMultipleInstance(base.SendBackOperation.ProcessInstance,
                runningMainNode,
                previousActivityInstance,
                base.SendBackOperation.BackwardType,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.SendBackOperation.ActivityResource,
                base.Session);
        }

        /// <summary>
        /// 最后一个会签多实例子节点的撤销操作
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="fromActivityInstance">运行节点</param>
        /// <param name="originalBackwardToActivityInstance">初始退回到的节点实例</param>
        /// <param name="backwardType">退回类型</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">跳跃类型</param>
        /// <param name="activityResource">资源</param>
        /// <param name="session">会话</param>
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
            rollbackPreviousActivityInstance.AssignedToUserIDs = base.SendBackOperation.BackwardToTaskPerformer.UserID;      //多实例节点为单一用户任务
            rollbackPreviousActivityInstance.AssignedToUserNames = base.SendBackOperation.BackwardToTaskPerformer.UserName;

            //插入新活动实例数据
            base.ActivityInstanceManager.Insert(rollbackPreviousActivityInstance,
                session);

            //创建新任务数据
            //插入任务数据
            base.TaskManager.Insert(rollbackPreviousActivityInstance,
                base.SendBackOperation.BackwardToTaskPerformer,
                activityResource.AppRunner,
                session);

            //插入转移数据
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
