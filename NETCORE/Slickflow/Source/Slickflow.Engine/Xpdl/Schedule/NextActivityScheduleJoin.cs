using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 合并类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleJoin : NextActivityScheduleBase
    {
        internal NextActivityScheduleJoin(IProcessModel processModel) :
            base(processModel)
        {
            
        }

        /// <summary>
        /// 获取下一步节点列表
        /// </summary>
        /// <param name="fromTransition">起始转移</param>
        /// <param name="currentGatewayActivity">当前节点</param>
        /// <param name="conditionKeyValuePair">条件对</param>
        /// <param name="session">会话</param>
        /// <param name="resultType">结果类型</param>
        /// <returns>下一步组件类型</returns>
        internal override NextActivityComponent GetNextActivityListFromGateway(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            NextActivityComponent gatewayComponent = null;
            resultType = NextActivityMatchedType.Unknown;

            //直接取出下步列表，运行时再根据条件执行
            List<TransitionEntity> transitionList = base.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityGUID,
                conditionKeyValuePair).ToList();
            foreach (TransitionEntity transition in transitionList)
            {
                child = GetNextActivityListFromGatewayCore(transition,
                    conditionKeyValuePair,
                    session,
                    out resultType);

                gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
            }

            return gatewayComponent;
        }
    }
}
