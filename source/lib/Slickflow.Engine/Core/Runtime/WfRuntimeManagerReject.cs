using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 驳回处理
    /// </summary>
    internal class WfRuntimeManagerReject : WfRuntimeManager
    {
        /// <summary>
        /// 跳转执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            //设置当前办理任务为完成状态
            var tm = new TaskManager();
            tm.Complete(this.TaskView.TaskID, base.AppRunner, session);

            //取消当前其它处于办理状态的节点
            var aim = new ActivityInstanceManager();
            var runningList = aim.GetRunningActivityInstanceList(base.AppRunner.AppInstanceID, base.AppRunner.ProcessGUID, base.AppRunner.Version, session);
            foreach (var ai in runningList)
            {
                if (ai.ID != base.BackwardContext.BackwardFromActivityInstance.ID)
                {
                    aim.Cancel(ai.ID, base.AppRunner, session);
                }
            }

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
            aim.SendBack(base.BackwardContext.BackwardFromActivityInstance.ID,
                base.ActivityResource.AppRunner,
                session);

            //构造回调函数需要的数据
            result.BackwardTaskReceiver = base.BackwardContext.BackwardTaskReceiver;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
