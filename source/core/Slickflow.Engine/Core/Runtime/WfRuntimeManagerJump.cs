using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;
using Slickflow.Engine.Core.Pattern.Event;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Handling of jump methods
    /// 跳转方式的处理
    /// </summary>
    internal class WfRuntimeManagerJump : WfRuntimeManager
    {
        /// <summary>
        /// Jump execution method
        /// 跳转执行方法
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            var aim = new ActivityInstanceManager();
            aim.Complete(base.RunningActivityInstance.Id, this.AppRunner, session);

            //构建跳转上下文
            //Build jump context
            var jumpActivityId = base.AppRunner.NextActivityPerformers.First().Key;
            var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityId);
            var processInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceId);

            var jumpforwardExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource,
                    true,
                    session);
            jumpforwardExecutionContext.TaskId = this.TaskView.Id;
            jumpforwardExecutionContext.FromActivityInstance = base.RunningActivityInstance;

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
            mediator.LinkContext.FromActivityInstance = base.RunningActivityInstance;
            mediator.LinkContext.CurrentActivity = jumpforwardActivity;

            if (mediator is NodeMediatorEnd)
            {
                //结束节点的连线转移
                //Insert transition instance of End activity
                mediator.CreateActivityTaskTransitionInstance(jumpforwardActivity,
                    processInstance,
                    base.RunningActivityInstance,
                    WfDefine.WF_XPDL_JUMP_BYPASS_GUID,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.ForwardFlying,
                    base.ActivityResource,
                    session);
            }
            mediator.ExecuteWorkItem(null);

            result.Status = WfExecutedStatus.Success;
            result.Message = mediator.GetNodeMediatedMessage();
        }
    }
}
