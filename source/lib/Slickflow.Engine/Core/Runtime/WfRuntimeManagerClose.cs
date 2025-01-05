using System.Linq;
using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;
using Slickflow.Engine.Core.Pattern.Event;
using static IronPython.Modules._ast;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Workflow Runtimer Manager Close
    /// 流程运行时关闭
    /// </summary>
    internal class WfRuntimeManagerClose : WfRuntimeManager
    {
        /// <summary>
        /// Execution method for closing operation
        /// 关闭操作的执行方法
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            //设置当前活动实例为完成状态
            //Set the current activity instance to completion status
            var aim = new ActivityInstanceManager();
            aim.Complete(base.RunningActivityInstance.ID, this.AppRunner, session);

            var jumpActivityGUID = base.AppRunner.NextActivityPerformers.First().Key;
            var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityGUID);
            var processInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceID);
            var jumpforwardExecutionContext = ActivityForwardContext.CreateJumpforwardContext(jumpforwardActivity,
                base.ProcessModel, processInstance, base.ActivityResource);
            jumpforwardExecutionContext.FromActivityInstance = base.RunningActivityInstance;

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
            mediator.LinkContext.FromActivityInstance = base.RunningActivityInstance;
            mediator.LinkContext.ToActivity = jumpforwardActivity;

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
            mediator.ExecuteWorkItem();

            result.Status = WfExecutedStatus.Success;
            result.Message = mediator.GetNodeMediatedMessage();
        }
    }
}
