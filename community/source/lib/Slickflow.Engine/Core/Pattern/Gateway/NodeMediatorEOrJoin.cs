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
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// OrJoin 节点处理类
    /// </summary>
    internal class NodeMediatorEOrJoin : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorEOrJoin(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        #region ICompleteAutomaticlly 成员
        /// <summary>
        /// EOrJoin合并时的节点完成方法
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);
            //根据强制或合并类型来处理
            if (GatewayActivity.GatewayDetail.JoinPassType == GatewayJoinPassEnum.Count)
            {
                result = CompleteAutomaticallyByTokensCount(processInstance, transitionGUID, fromActivity, fromActivityInstance, runner, session);
            }
            else if (GatewayActivity.GatewayDetail.JoinPassType == GatewayJoinPassEnum.Forced)
            {
                result = CompleteAutomaticallyByForcedBranchesCount(processInstance, transitionGUID, fromActivity, fromActivityInstance, runner, session);
            }
            return result;
        }

        /// <summary>
        /// EOrJoin合并时的节点完成方法
        /// 前端动态传递要合并的Tokens的数目
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        private NodeAutoExecutedResult CompleteAutomaticallyByTokensCount(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);
            if (runner.ControlParameterSheet == null || runner.ControlParameterSheet.EOrJoinTokenPassCount <= 0)
            {
                result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Failed);
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nodemediatoreorjoin.CompleteAutomaticallyByTokensCount.error"));
            }
            var tokensCountRequired = runner.ControlParameterSheet.EOrJoinTokenPassCount;
            result = CompleteAutomaticallyInternal(processInstance, transitionGUID, fromActivity, fromActivityInstance, tokensCountRequired, runner, session);
            return result;
        }

        /// <summary>
        /// EOrJoin合并时的节点完成方法
        /// 根据前置转移上定义的是否强制分支来判断完成合并节点
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        private NodeAutoExecutedResult CompleteAutomaticallyByForcedBranchesCount(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);

            IList<Transition> forcedTransitionList = null;
            var tokensCountRequired = base.ProcessModel.GetForcedBranchesCountBeforeEOrJoin(GatewayActivity, out forcedTransitionList);
            if (tokensCountRequired == 0)
            {
                result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Failed);
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nodemediatoreorjoin.CompleteAutomaticallyByForcedBranchesCount.error"));
            }

            //根据强制分支的数目和具体分支来完成增强合并节点
            var forcedCount = forcedTransitionList.Where(t => t.TransitionGUID == transitionGUID).Count();
            if (forcedCount == 0)
            {
                //当前执行的分支不是强制分支，直接返回就可以
                result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.NotForcedBrancheWhenEOrJoin);
            }
            else if (forcedCount == 1)
            {
                result = CompleteAutomaticallyInternal(processInstance, transitionGUID, fromActivity, fromActivityInstance, tokensCountRequired, runner, session);
            }
            return result;
        }

        /// <summary>
        /// EOrJoin合并时的节点完成方法
        /// 内部执行逻辑
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivity">起始活动</param>
        /// <param name="fromActivityInstance">起始活动实例</param>
        /// <param name="tokensCountRequired">合并要求的Token数目</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        private NodeAutoExecutedResult CompleteAutomaticallyInternal(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            int tokensCountRequired,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);
            //当前执行的分支就是强制分支
            //检查是否有运行中的合并节点实例
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(
                    processInstance.ID,
                    base.GatewayActivity.ActivityGUID,
                    session); ;

            if (joinNode == null)
            {
                //第一个分支首次运行
                joinNode = base.CreateActivityInstanceObject(base.GatewayActivity,
                    processInstance, runner);

                //计算总需要的Token数目
                joinNode.TokensRequired = tokensCountRequired;
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

                //更新其它分支上的待办节点为阻止状态(CanRenewInstance = 0)
                base.ActivityInstanceManager.UpdateActivityInstanceBlockedBetweenSplitJoin(base.GatewayActivity, base.GatewayActivityInstance, 
                    base.ProcessModel, session);

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
