using System;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Delegate;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Business.Entity;


namespace Slickflow.Engine.Core.Pattern.Auto
{
    /// <summary>
    /// Script node mediator
    /// 任务节点执行器
    /// </summary>
    internal class NodeMediatorScript : NodeMediator, ICreatedAutomaticlly, ICompletedAutomaticlly
    {
        internal NodeMediatorScript(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        internal NodeMediatorScript(IDbSession session)
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
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            try
            {
                //完成节点上绑定的外部程序逻辑或服务
                //Complete the external program logic or service bound on the node
                OnExecutingServiceItem();
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
            LinkContext.CurrentActivityInstance = toActivityInstance;
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
