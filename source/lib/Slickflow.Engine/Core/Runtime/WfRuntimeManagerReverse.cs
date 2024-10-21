using Slickflow.Data;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Core.Pattern;

namespace Slickflow.Engine.Core.Runtime
{
    /// <summary>
    /// 流程返签时的运行时
    /// </summary>
    internal class WfRuntimeManagerReverse : WfRuntimeManager
    {
        /// <summary>
        /// 返签执行方法
        /// </summary>
        /// <param name="session">会话</param>
        internal override void ExecuteInstanceImp(IDbSession session)
        {
            //修改流程实例为返签状态
            var pim = new ProcessInstanceManager();
            pim.Reverse(base.BackwardContext.ProcessInstance.ID, 
                base.AppRunner, 
                session);

            //创建新任务节点
            var nodeMediatorBackward = new NodeMediatorBackward(base.BackwardContext, session);
            nodeMediatorBackward.CreateBackwardActivityTaskTransitionInstance(base.BackwardContext.ProcessInstance,
                base.BackwardContext.BackwardFromActivityInstance,
                BackwardTypeEnum.Reversed,
                base.BackwardContext.BackwardToTargetTransitionGUID,
                TransitionTypeEnum.Backward,
                TransitionFlyingTypeEnum.NotFlying,
                base.ActivityResource,
                session);

            //构造回调函数需要的数据
            WfExecutedResult result = base.WfExecutedResult;
            result.BackwardTaskReceiver = base.BackwardContext.BackwardTaskReceiver;
            result.Status = WfExecutedStatus.Success;
        }
    }
}
