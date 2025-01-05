using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// AndJoinMI Node Mediator
    /// AndJoinMI 节点处理类
    /// </summary>
    internal class NodeMediatorAndJoinMI : NodeMediatorGateway, ICompleteGatewayAutomaticlly
    {
        internal NodeMediatorAndJoinMI(Activity activity, IProcessModel processModel, IDbSession session)
            : base(activity, processModel, session)
        {

        }

        /// <summary>
        /// Obtain the required number of tokens
        /// 获取需要的Token数目
        /// </summary>
        /// <param name="appInstanceID"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="joinNode"></param>
        /// <returns></returns>
        internal int GetTokensRequired(string appInstanceID, int processInstanceID, Activity joinNode)
        {
            int joinCount = 0, splitCount = 0;
            var gatewayNode = base.ProcessModel.GetBackwardGatewayActivity(joinNode, ref joinCount, ref splitCount);

            var gatewayActivityInstance = base.ActivityInstanceManager.GetActivityInstanceLatest(processInstanceID, gatewayNode.ActivityGUID, base.Session);
            var splitedActivityInstanceList = base.ActivityInstanceManager.GetValidSplitedActivityInstanceList(processInstanceID, gatewayActivityInstance.ID, base.Session);
            int tokensCount = splitedActivityInstanceList.Count;

            return tokensCount;
        }

        /// <summary>
        /// Complete automatically
        /// </summary>
        public NodeAutoExecutedResult CompleteAutomaticlly(ProcessInstanceEntity processInstance,
            string transitionGUID,
            Activity fromActivity,
            ActivityInstanceEntity fromActivityInstance,
            WfAppRunner runner,
            IDbSession session)
        {
            //检查是否有运行中的合并节点实例
            //Check if there are any running merge node instances
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
                //Calculate the total number of tokens required
                joinActivityInstance.TokensRequired = GetTokensRequired(processInstance.AppInstanceID, processInstance.ID, base.GatewayActivity);
                joinActivityInstance.TokensHad = 1;
                tokensRequired = joinActivityInstance.TokensRequired;

                joinActivityInstance.ActivityState = (short)ActivityStateEnum.Running;
                joinActivityInstance.GatewayDirectionTypeID = (short)GatewayDirectionEnum.AndJoinMI;

                //写入默认第一次的预选步骤用户列表
                //Write the default user list for the first pre selection step
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
                base.GatewayActivityInstance = joinNode;
                tokensRequired = base.GatewayActivityInstance.TokensRequired;
                tokensHad = base.GatewayActivityInstance.TokensHad;

                //更新Token数目
                //Update tokens count
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
                //If the number of tokens for the completion node is reached,
                //set the node status to complete
                base.CompleteActivityInstance(base.GatewayActivityInstance.ID,
                    runner,
                    session);
                base.GatewayActivityInstance.ActivityState = (short)ActivityStateEnum.Completed;
            }

            NodeAutoExecutedResult result = NodeAutoExecutedResult.CreateGatewayExecutedResult(
                NodeAutoExecutedStatus.Successed);
            return result;
        }
    }
}
