using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// Handling of reject methods 
    /// 驳回处理
    /// </summary>
    internal class WfRuntimeManagerReject : WfRuntimeManager
    {
        /// <summary>
        /// Reject Execute Method
        /// 驳回执行方法
        /// </summary>
        /// <param name="session"></param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            WfExecutedResult result = base.WfExecutedResult;

            var tm = new TaskManager();
            tm.Complete(this.TaskView.Id, base.AppRunner, session);

            //取消当前其它处于办理状态的节点
            //Cancel other nodes currently in processing status
            var aim = new ActivityInstanceManager();
            var runningList = aim.GetRunningActivityInstanceList(base.AppRunner.AppInstanceId, base.AppRunner.ProcessId, base.AppRunner.Version, session);
            foreach (var ai in runningList)
            {
                if (ai.Id != base.BackwardContext.BackwardFromActivityInstance.Id)
                {
                    aim.Cancel(ai.Id, base.AppRunner, session);
                }
            }

            var nodeMediatorBackward = new NodeMediatorBackward(base.BackwardContext, session);
            nodeMediatorBackward.CreateBackwardActivityTaskTransitionInstance(base.BackwardContext.ProcessInstance,
                base.BackwardContext.BackwardFromActivityInstance,
                BackwardTypeEnum.Sendback,
                base.BackwardContext.BackwardToTargetTransitionId,
                TransitionTypeEnum.Sendback,
                TransitionFlyingTypeEnum.NotFlying,
                base.ActivityResource,
                session);

            aim.SendBack(base.BackwardContext.BackwardFromActivityInstance.Id,
                base.ActivityResource.AppRunner,
                session);

            result.BackwardTaskReceiver = base.BackwardContext.BackwardTaskReceiver;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
