/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

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
    /// OrJoin 节点处理类
    /// </summary>
    internal class NodeMediatorOrJoin : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorOrJoin(ActivityEntity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        /// <summary>
        /// 计算需要的汇合Token数目
        /// </summary>
        /// <param name="processInstanceID">流程实例ID</param>
        /// <param name="gatewayActivity">网关活动</param>
        /// <param name="session">会话</param>
        /// <returns>Token数目</returns>
		internal int GetTokensRequired(int processInstanceID,
            ActivityEntity gatewayActivity,
            IDbSession session)
        {
            int splitCount = 0;
            int joinCount = 0;
            ActivityEntity splitActivity = this.ProcessModel.GetBackwardGatewayActivity(gatewayActivity, ref joinCount, ref splitCount);
            ActivityInstanceEntity splitActivityInstance = base.ActivityInstanceManager.GetActivityInstanceLatest(processInstanceID, splitActivity.ActivityGUID, session);

            return base.ActivityInstanceManager.GetGatewayInstanceCountByTransition(splitActivity.ActivityGUID, 
                splitActivityInstance.ID,
                processInstanceID, 
                session);
        }

        #region ICompleteAutomaticlly 成员

        /// <summary>
        /// OrJoin合并时的节点完成方法
        /// 1. 如果有满足条件的OrJoin前驱转移，则会重新生成新的OrJoin节点实例
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityEntity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);

			//检查是否有运行中的合并节点实例
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(processInstance.ID,
                base.GatewayActivity.ActivityGUID,
                session);

            if (joinNode == null)
            {
                joinNode = base.CreateActivityInstanceObject(base.GatewayActivity,
                    processInstance, runner);

                //计算总需要的Token数目
                joinNode.TokensRequired = GetTokensRequired(processInstance.ID, base.GatewayActivity, session);
                joinNode.TokensHad = 1;

                //进入运行状态
                joinNode.ActivityState = (short)ActivityStateEnum.Running;
                joinNode.GatewayDirectionTypeID = (short)GatewayDirectionEnum.OrJoin;

                //写入默认第一次的预选步骤用户列表
                joinNode.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);

                base.InsertActivityInstance(joinNode,
                    session);
				base.InsertTransitionInstance(processInstance,
                    transitionGUID,
                    fromActivityInstance,
                    joinNode,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.NotFlying,
                    runner,
                    session);
            }
            else
            {
                //更新节点的活动实例属性
                joinNode.TokensHad += 1;
                base.GatewayActivityInstance = joinNode;

                //更新Token数目
                base.ActivityInstanceManager.IncreaseTokensHad(base.GatewayActivityInstance.ID,
                    runner,
                    session);
                base.InsertTransitionInstance(processInstance,
                    transitionGUID,
                    fromActivityInstance,
                    joinNode,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.NotFlying,
                    runner,
                    session);
            }

            //判断是否到达合并节点的通过Token数目要求
            if (joinNode.TokensHad == joinNode.TokensRequired)
            {
                //如果达到完成节点的Token数，则设置该节点状态为完成
                base.CompleteActivityInstance(base.GatewayActivityInstance.ID,
                    runner,
                    session);
                base.GatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;

                result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Successed);
            }
            else if (joinNode.TokensHad < joinNode.TokensRequired)
            {
                result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.WaitingForOthersJoin);
            }
            
            return result;
        }

        #endregion
    }
}
