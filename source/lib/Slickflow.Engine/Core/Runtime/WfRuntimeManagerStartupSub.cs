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
    /// 流程启动运行时
    /// </summary>
    internal class WfRuntimeManagerStartupSub : WfRuntimeManager
    {
        /// <summary>
        /// 启动执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            //构造流程实例
            var subProcessNode = (SubProcessNode)base.InvokedSubProcessNode;
            var pim = new ProcessInstanceManager();
            var processInstance = pim.CreateNewProcessInstanceObject(base.AppRunner,
                base.ProcessModel.ProcessEntity,
                subProcessNode);

            //构造活动实例
            //1. 获取开始节点活动

            var subStartactivity = base.ProcessModel.GetStartActivity();

            var startExecutionContext = ActivityForwardContext.CreateStartupContext(base.ProcessModel,
                processInstance,
                subStartactivity,
                base.ActivityResource);

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(startExecutionContext, session);
            mediator.LinkContext.FromActivityInstance = RunningActivityInstance;
            mediator.ExecuteWorkItem();

            //构造回调函数需要的数据
            WfExecutedResult result = base.WfExecutedResult;
            result.ProcessInstanceIDStarted = processInstance.ID;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
