using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// XOrJoin Node Mediator
    /// XOrJoin 节点处理类
    /// </summary>
    internal class NodeMediatorXOrJoin : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorXOrJoin(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        /// <summary>
        /// Complete automatically
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionId,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);

            //XOrJoin合并，直接置为完成状态，因为始终只有一个分支可以得到执行，其它分支被排斥
            //XOrJoin merge, directly set to completion state, because only one branch can always be executed,
            //and the other branches are excluded
            var gatewayActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, 
                processInstance, runner);

            gatewayActivityInstance.GatewayDirectionTypeId = (short)GatewayDirectionEnum.XOrJoin;

            base.InsertActivityInstance(gatewayActivityInstance,
                session);

            base.CompleteActivityInstance(gatewayActivityInstance.Id,
                runner,
                session);

            gatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.GatewayActivityInstance = gatewayActivityInstance;
                
            base.InsertTransitionInstance(processInstance,
                transitionId,
                fromActivityInstance,
                gatewayActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);

            return result;
        }
    }
}
