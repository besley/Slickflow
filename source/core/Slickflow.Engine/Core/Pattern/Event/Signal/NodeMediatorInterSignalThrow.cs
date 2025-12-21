
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Essential;

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// Intermediate Signal throw node mediator
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterSignalThrow : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorInterSignalThrow(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Created automatically
        /// </summary>
        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var messageActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            base.InsertActivityInstance(messageActivityInstance,
                session);
            return messageActivityInstance;
        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                OnBeforeExecuteWorkItem();

                OnAfterExecuteWorkItem();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete automatically
        /// </summary>
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

            LinkContext.CurrentActivity = toActivity;
            LinkContext.CurrentActivityInstance = toActivityInstance;

            //执行节点上的信号发布
            //Signal release on the execution node
            var signalDelegateService = new SignalDelegateService();
            signalDelegateService.PublishSignal(processInstance, LinkContext.CurrentActivity, toActivityInstance);

            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                toActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
