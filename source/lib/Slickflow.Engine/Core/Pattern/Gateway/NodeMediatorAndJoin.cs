using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// AndJoin 节点处理类
    /// </summary>
    internal class NodeMediatorAndJoin : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorAndJoin(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        /// <summary>
        /// 计算汇合Token数目
        /// </summary>
        /// <returns></returns>
        internal int GetTokensRequired()
        {
            int tokensRequired = this.ProcessModel.GetBackwardTransitionListCount(GatewayActivity.ActivityGUID);
            return tokensRequired;
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
            //检查是否有运行中的合并节点实例
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(processInstance.ID,
                base.GatewayActivity.ActivityGUID,
                session);

            int tokensRequired = 0;
            int tokensHad = 0;

            if (joinNode == null)
            {
                var joinActivityInstance = base.CreateActivityInstanceObject(base.GatewayActivity, 
                    processInstance, runner);

                //计算总需要的Token数目
                joinActivityInstance.TokensRequired = GetTokensRequired();
                joinActivityInstance.TokensHad = 1;
                tokensRequired = joinActivityInstance.TokensRequired;

                //进入运行状态
                joinActivityInstance.ActivityState = (short)ActivityStateEnum.Running;
                joinActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.AndJoin;

                //写入默认第一次的预选步骤用户列表
                joinActivityInstance.NextStepPerformers = NextStepUtility.SerializeNextStepPerformers(runner.NextActivityPerformers);

                base.InsertActivityInstance(joinActivityInstance,
                    session);
				base.InsertTransitionInstance(processInstance,
                    transitionGUID,
                    fromActivityInstance,
                    joinActivityInstance,
                    TransitionTypeEnum.Forward,
                    TransitionFlyingTypeEnum.NotFlying,
                    runner,
                    session);
            }
            else
            {
                //更新节点的活动实例属性
                base.GatewayActivityInstance = joinNode;
                tokensRequired = base.GatewayActivityInstance.TokensRequired;
                tokensHad = base.GatewayActivityInstance.TokensHad;
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
            if ((tokensHad + 1) == tokensRequired)
            {
                //如果达到完成节点的Token数，则设置该节点状态为完成
                base.CompleteActivityInstance(base.GatewayActivityInstance.ID,
                    runner,
                    session);
                base.GatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            }

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(
                NodeAutoExecutedStatus.Successed);
            return result;
        }

        #endregion
    }
}
