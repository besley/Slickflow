
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
    internal class NodeMediatorInterSignalThrow : NodeMediator, ICompleteAutomaticlly
    {
        internal NodeMediatorInterSignalThrow(ActivityForwardContext forwardContext, IDbSession session)
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

            //执行节点上的信号发布
            //Signal release on the execution node
            var signalDelegateService = new SignalDelegateService();
            signalDelegateService.PublishSignal(processInstance, LinkContext.ToActivity, messageActivityInstance);

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
