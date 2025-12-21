
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// End signal catch node mediator
    /// 信号结束节点的Catch类
    /// </summary>
    internal class NodeMediatorEndSignalCatch : NodeMediator
    {
        internal NodeMediatorEndSignalCatch(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// Execute work item
        /// </summary>
        internal override void ExecuteWorkItem(ActivityInstanceEntity activityInstance)
        {
            OnBeforeExecuteWorkItem();

            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processInstance = pim.Complete(ActivityForwardContext.ProcessInstance.Id,
                ActivityForwardContext.ActivityResource.AppRunner,
                Session);

            OnAfterExecuteWorkItem();
        }
    }
}
