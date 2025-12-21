
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

namespace Slickflow.Engine.Core.Pattern.Event.Message
{
    /// <summary>
    ///  Intermediate Node Mediator message throw
    /// 中间事件节点处理类
    /// </summary>
    internal class NodeMediatorInterMsgThrow : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorInterMsgThrow(ActivityForwardContext forwardContext, IDbSession session)
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

            //执行节点上的消息发布
            //Publish messages on the execution node
            var msgDelegateService = new MessageDelegateService();
            msgDelegateService.PublishMessage(processInstance, LinkContext.CurrentActivity, toActivityInstance);

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
