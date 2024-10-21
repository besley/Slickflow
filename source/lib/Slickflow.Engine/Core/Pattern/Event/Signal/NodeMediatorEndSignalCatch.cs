
using Slickflow.Data;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Core.Pattern.Event.Signal
{
    /// <summary>
    /// 信号结束节点的Catch类
    /// </summary>
    internal class NodeMediatorEndSignalCatch : NodeMediator
    {
        internal NodeMediatorEndSignalCatch(ActivityForwardContext forwardContext, IDbSession session)
            : base(forwardContext, session)
        {

        }

        /// <summary>
        /// 节点内部业务逻辑执行
        /// </summary>
        internal override void ExecuteWorkItem()
        {
            //执行前Action列表
            OnBeforeExecuteWorkItem();

            //设置流程完成
            ProcessInstanceManager pim = new ProcessInstanceManager();
            var processInstance = pim.Complete(ActivityForwardContext.ProcessInstance.ID,
                ActivityForwardContext.ActivityResource.AppRunner,
                Session);

            //执行后Action列表
            OnAfterExecuteWorkItem();
        }
    }
}
