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
    /// OrJoin Node Mediator
    /// OrJoin 节点处理类
    /// </summary>
    internal class NodeMediatorOrJoin : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorOrJoin(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        /// <summary>
        /// Calculate the required number of merging tokens
        /// 计算需要的汇合Token数目
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="gatewayActivity"></param>
        /// <param name="session"></param>
        /// <returns></returns>
		internal int GetTokensRequired(int processInstanceID,
            Activity gatewayActivity,
            IDbSession session)
        {
            int splitCount = 0;
            int joinCount = 0;
            Activity splitActivity = this.ProcessModel.GetBackwardGatewayActivity(gatewayActivity, ref joinCount, ref splitCount);
            ActivityInstanceEntity splitActivityInstance = base.ActivityInstanceManager.GetActivityInstanceLatest(processInstanceID, splitActivity.ActivityGUID, session);

            return base.ActivityInstanceManager.GetGatewayInstanceCountByTransition(splitActivity.ActivityGUID, 
                splitActivityInstance.ID,
                processInstanceID, 
                session);
        }

        /// <summary>
        /// Node completion method for OrJoin merging
        /// 1.  If there are OrJoin predecessor transfers that meet the conditions, a new OrJoin node instance will be generated
        /// OrJoin合并时的节点完成方法
        /// 1. 如果有满足条件的OrJoin前驱转移，则会重新生成新的OrJoin节点实例
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(NodeAutoExecutedStatus.Unknown);

            //检查是否有运行中的合并节点实例
            //Check if there are any running merge node instances
            ActivityInstanceEntity joinNode = base.ActivityInstanceManager.GetActivityRunning(processInstance.ID,
                base.GatewayActivity.ActivityGUID,
                session);

            if (joinNode == null)
            {
                joinNode = base.CreateActivityInstanceObject(base.GatewayActivity,
                    processInstance, runner);

                //计算总需要的Token数目
                //Calculate the total number of tokens required
                joinNode.TokensRequired = GetTokensRequired(processInstance.ID, base.GatewayActivity, session);
                joinNode.TokensHad = 1;

                joinNode.ActivityState = (short)ActivityStateEnum.Running;
                joinNode.GatewayDirectionTypeID = (short)GatewayDirectionEnum.OrJoin;

                //写入默认第一次的预选步骤用户列表
                //Write the default user list for the first pre selection step
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
                joinNode.TokensHad += 1;
                base.GatewayActivityInstance = joinNode;

                //更新Token数目
                //Increase tokens count
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
            //Determine whether the required number of tokens for merging nodes has been reached
            if (joinNode.TokensHad == joinNode.TokensRequired)
            {
                //如果达到完成节点的Token数，则设置该节点状态为完成
                //If the number of tokens for the completion node is reached, set the node status to complete
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
    }
}
