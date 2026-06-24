using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;

namespace Slickflow.Engine.Core.Pattern.Auto
{
    /// <summary>
    /// Business rule task mediator
    /// </summary>
    internal class NodeMediatorRule : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorRule(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {
        }

        internal NodeMediatorRule(IDbSession session)
            : base(session)
        {
        }

        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var activityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            base.InsertActivityInstance(activityInstance, session);
            return activityInstance;
        }

        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            var currentActivity = LinkContext.CurrentActivity;
            if (currentActivity == null
                || currentActivity.RuleConfigDetail == null
                || string.IsNullOrWhiteSpace(currentActivity.RuleConfigDetail.RuleSetCode))
            {
                throw new WorkflowException("RuleTask configuration is missing ruleSetCode.");
            }

            var eventService = GetInnerEventService();
            RuleExecutor.ExecuteRule(currentActivity.RuleConfigDetail, currentActivity, activityInstance, eventService);
        }

        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            ActivityInstanceEntity toActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            base.ActivityInstanceManager.Complete(toActivityInstance, runner, session);
            LinkContext.CurrentActivityInstance = toActivityInstance;
            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                toActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            return NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
        }
    }
}
