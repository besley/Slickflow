
using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Essential;
using System.CodeDom.Compiler;

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// Start signal throw node mediator
    /// 开始节点执行器
    /// </summary>
    internal class NodeMediatorStartSignalThrow : NodeMediator
    {
        internal NodeMediatorStartSignalThrow(ActivityForwardContext forwardContext, IDbSession session)
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
                ProcessInstanceManager pim = new ProcessInstanceManager();
                var newID = pim.Insert(Session.Connection, ActivityForwardContext.ProcessInstance,
                    Session.Transaction);
                ActivityForwardContext.ProcessInstance.ID = newID;

                OnBeforeExecuteWorkItem();

                CompleteAutomaticlly(ActivityForwardContext.ProcessInstance,
                    ActivityForwardContext.ActivityResource,
                    Session);

                OnAfterExecuteWorkItem();

                //执行开始节点之后的节点集合
                //Collection of nodes after executing the start node
                ContinueForwardCurrentNode(ActivityForwardContext.IsNotParsedByTransition, Session);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Complete automatically
        /// </summary>
        /// <param name="processInstance"></param>
        /// <param name="activityResource"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            var fromActivityInstance = CreateActivityInstanceObject(LinkContext.FromActivity, processInstance, activityResource.AppRunner);

            ActivityInstanceManager.Insert(fromActivityInstance, session);

            ActivityInstanceManager.Complete(fromActivityInstance.ID,
                activityResource.AppRunner,
                session);

            fromActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            LinkContext.FromActivityInstance = fromActivityInstance;

            //执行节点上的信号发布
            //Signal release on the execution node
            var signalDelegateService = new SignalDelegateService();
            signalDelegateService.PublishSignal(processInstance, LinkContext.FromActivity, fromActivityInstance);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
