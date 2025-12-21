using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using System;
using System.Text;


namespace Slickflow.Engine.Core.Pattern.Auto
{
    /// <summary>
    /// Service node mediator
    /// 任务节点执行器
    /// </summary>
    internal class NodeMediatorAIService : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorAIService(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorAIService(IDbSession session)
            : base(session)
        {

        }

        /// <summary>
        /// Created automatically
        /// </summary>
        public ActivityInstanceEntity CreatedAutomaticlly(Activity toActivity, ProcessInstanceEntity processInstance, WfAppRunner runner, IDbSession session)
        {
            var aiServiceActivityInstance = CreateActivityInstanceObject(toActivity, processInstance, runner);
            base.InsertActivityInstance(aiServiceActivityInstance,
                session);
            return aiServiceActivityInstance;
        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity currentActivityInstance)
        {
            try
            {
                //完成节点上绑定的外部程序逻辑或服务
                //Complete the external program logic or service bound on the node
                OnExecutingAIServiceItem(base.ActivityForwardContext.FromActivityInstance, currentActivityInstance);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Completed automatically
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            Activity currentActivity,
            ActivityInstanceEntity currentActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            base.ActivityInstanceManager.Complete(currentActivityInstance, runner, session);
            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                currentActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
