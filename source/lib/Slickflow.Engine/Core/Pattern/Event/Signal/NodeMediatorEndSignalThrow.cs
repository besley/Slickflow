using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Essential;


namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// End signal throw node mediator
    /// 结束节点处理类
    /// </summary>
    internal class NodeMediatorEndSignalThrow : NodeMediator
    {
        internal NodeMediatorEndSignalThrow(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            OnBeforeExecuteWorkItem();

            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processInstance = pim.Complete(ActivityForwardContext.ProcessInstance.ID,
                ActivityForwardContext.ActivityResource.AppRunner,
                Session);

            //执行节点上的信号发布
            //Publish signal on the execution node
            var msgDelegateService = new MessageDelegateService();
            msgDelegateService.PublishMessage(processInstance, LinkContext.ToActivity, ActivityForwardContext.FromActivityInstance);

            OnAfterExecuteWorkItem();
        }


        /// <summary>
        /// End node activity and transfer instantiation, no task data available
        /// 结束节点活动及转移实例化，没有任务数据
        /// </summary>
        internal override void CreateActivityTaskTransitionInstance(Activity toActivity,
            ProcessInstanceEntity processInstance,
            ActivityInstanceEntity fromActivityInstance,
            string transitionGUID,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            ActivityResource activityResource,
            IDbSession session)
        {
            var toActivityInstance = CreateActivityInstanceObject(toActivity,
                processInstance, activityResource.AppRunner);

            ActivityInstanceManager.Insert(toActivityInstance, session);

            ActivityInstanceManager.Complete(toActivityInstance.ID,
                activityResource.AppRunner,
                session);

            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                activityResource.AppRunner,
                session);
        }
    }
}
