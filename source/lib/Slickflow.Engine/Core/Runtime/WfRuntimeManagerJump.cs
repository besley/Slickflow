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
    /// 跳转方式的处理
    /// </summary>
    internal class WfRuntimeManagerJump : WfRuntimeManager
    {
        /// <summary>
        /// 跳转执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            //设置当前活动实例为完成状态
            var aim = new ActivityInstanceManager();
            aim.Complete(base.RunningActivityInstance.ID, this.AppRunner, session);

            // 构建跳转上下文
            var jumpActivityGUID = base.AppRunner.NextActivityPerformers.First().Key;
            var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityGUID);
            var processInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceID);

            var jumpforwardExecutionContext = ActivityForwardContext.CreateRunningContextByTask(base.TaskView,
                    base.ProcessModel,
                    base.ActivityResource,
                    true,
                    session);
            jumpforwardExecutionContext.TaskID = this.TaskView.TaskID;
            jumpforwardExecutionContext.FromActivityInstance = base.RunningActivityInstance;

            NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
            mediator.LinkContext.FromActivityInstance = base.RunningActivityInstance;
            mediator.LinkContext.ToActivity = jumpforwardActivity;

            if (mediator is NodeMediatorEnd)
            {
                //结束节点的连线转移
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

            #region 考虑回退方式的跳转，代码暂时保留 2023-02-14 besley
            /*
            // 
            //回跳类型的处理
            if (base.IsBackward == true)
            {
                //创建新任务节点
                var nodeMediatorBackward = new NodeMediatorBackward(base.BackwardContext, session);
                nodeMediatorBackward.CreateBackwardActivityTaskTransitionInstance(base.BackwardContext.ProcessInstance,
                    base.BackwardContext.BackwardFromActivityInstance,
                    BackwardTypeEnum.Sendback,
                    base.BackwardContext.BackwardToTargetTransitionGUID,
                    TransitionTypeEnum.Sendback,
                    TransitionFlyingTypeEnum.NotFlying,
                    base.ActivityResource,
                    session);

                //更新当前办理节点的状态（从准备或运行状态更新为退回状态）
                var aim = new ActivityInstanceManager();
                aim.SendBack(base.BackwardContext.BackwardFromActivityInstance.ID,
                    base.ActivityResource.AppRunner,
                    session);

                //构造回调函数需要的数据
                result.BackwardTaskReceiver = base.BackwardContext.BackwardTaskReceiver;
                result.Status = WfExecutedStatus.Success;
            }
            else
            {
                var jumpActivityGUID = base.AppRunner.NextActivityPerformers.First().Key;
                var jumpforwardActivity = base.ProcessModel.GetActivity(jumpActivityGUID);
                var processInstance = (new ProcessInstanceManager()).GetById(base.RunningActivityInstance.ProcessInstanceID);
                var jumpforwardExecutionContext = ActivityForwardContext.CreateJumpforwardContext(jumpforwardActivity,
                    base.ProcessModel, processInstance, base.ActivityResource);
                jumpforwardExecutionContext.TaskID = this.TaskView.TaskID;
                jumpforwardExecutionContext.FromActivityInstance = base.RunningActivityInstance;

                NodeMediator mediator = NodeMediatorFactory.CreateNodeMediator(jumpforwardExecutionContext, session);
                mediator.LinkContext.FromActivityInstance = base.RunningActivityInstance;
                mediator.LinkContext.ToActivity = jumpforwardActivity;

                if (mediator is NodeMediatorEnd)
                {
                    //结束节点的连线转移
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
            */
            #endregion
        }
    }
}
