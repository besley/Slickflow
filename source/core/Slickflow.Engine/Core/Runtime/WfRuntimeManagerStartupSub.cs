using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Core.Runtime
{ 
    /// <summary>
    /// Runtime Manager Startup Sub Process
    /// 流程启动运行时
    /// </summary>
    internal class WfRuntimeManagerStartupSub : WfRuntimeManager
    {
        /// <summary>
        /// The processing logic of startup subprocess
        /// 启动执行方法
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            var subProcessNode = (SubProcessNode)base.InvokedSubProcessNode;
            var pim = new ProcessInstanceManager();
            var processInstance = pim.CreateNewProcessInstanceObject(base.AppRunner,
                base.ProcessModel.ProcessEntity,
                subProcessNode);

            var subStartactivity = base.ProcessModel.GetStartActivity();
            var startExecutionContext = ActivityForwardContext.CreateStartupContext(base.ProcessModel,
                processInstance,
                subStartactivity,
                base.ActivityResource);

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(startExecutionContext, session);
            mediator.LinkContext.FromActivityInstance = RunningActivityInstance;
            mediator.ExecuteWorkItem(null);

            WfExecutedResult result = base.WfExecutedResult;
            result.ProcessInstanceIdStarted = processInstance.Id;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
