/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// XOrJoin 节点处理类
    /// </summary>
    internal class NodeMediatorXOrJoin : NodeMediatorGateway, ICompleteAutomaticlly
    {
        internal NodeMediatorXOrJoin(ActivityEntity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        #region ICompleteAutomaticlly 成员

        public GatewayExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityResource activityResource,
            IDbSession session)
        {
            GatewayExecutedResult result = GatewayExecutedResult.CreateGatewayExecutedResult(GatewayExecutedStatus.Unknown);

            bool canRenewInstance = false;

            //检查是否有运行中的合并节点实例
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(
                processInstance.ID,
                base.GatewayActivity.ActivityGUID,
                session);

            if (joinNode == null)
            {
                canRenewInstance = true;
            }
            else
            {
                //判断是否可以激活下一步节点
                canRenewInstance = (joinNode.CanNotRenewInstance == 1);
                if (!canRenewInstance)
                {
                    result = GatewayExecutedResult.CreateGatewayExecutedResult(GatewayExecutedStatus.FallBehindOfXOrJoin);
                    return result;
                }
            }

            if (canRenewInstance)
            {
                var gatewayActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, 
                    processInstance, activityResource.AppRunner);

                gatewayActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.XOrJoin;

                base.InsertActivityInstance(gatewayActivityInstance,
                    session);

                base.CompleteActivityInstance(gatewayActivityInstance.ID,
                    activityResource,
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
                    activityResource.AppRunner,
                    session);

                result = GatewayExecutedResult.CreateGatewayExecutedResult(GatewayExecutedStatus.Successed);
            }
            return result;
        }
        #endregion
    }
}
