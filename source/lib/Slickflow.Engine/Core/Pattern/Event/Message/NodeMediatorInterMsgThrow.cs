
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
    internal class NodeMediatorInterMsgThrow : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterMsgThrow(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem()
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
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity toActivity,
            WfAppRunner runner,
            IDbSession session)
        {
            var messageActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            LinkContext.ToActivity = toActivity;
            LinkContext.ToActivityInstance = messageActivityInstance;

            base.InsertActivityInstance(messageActivityInstance,
                session);

            base.CompleteActivityInstance(messageActivityInstance.ID,
                runner,
                session);
            messageActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

            //执行节点上的消息发布
            //Publish messages on the execution node
            var msgDelegateService = new MessageDelegateService();
            msgDelegateService.PublishMessage(processInstance, LinkContext.ToActivity, messageActivityInstance);

            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                messageActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
