using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// XOrSplit 节点处理类
    /// </summary>
    internal class NodeMediatorXOrSplit : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorXOrSplit(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        #region ICompleteAutomaticlly 成员
        /// <summary>
        /// 自动完成
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        /// <returns>网关执行结果</returns>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            var gatewayActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, 
                processInstance, runner);
            gatewayActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.XOrSplit;

            base.InsertActivityInstance(gatewayActivityInstance,
                session);

            base.CompleteActivityInstance(gatewayActivityInstance.ID,
                runner,
                session);

            gatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            base.GatewayActivityInstance = gatewayActivityInstance;
            
            //写节点转移实例数据
            base.InsertTransitionInstance(processInstance,
                transitionGUID,
                fromActivityInstance,
                gatewayActivityInstance,
                TransitionTypeEnum.Forward,
                TransitionFlyingTypeEnum.NotFlying,
                runner,
                session);

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            return result;
        }

        #endregion
    }
}
