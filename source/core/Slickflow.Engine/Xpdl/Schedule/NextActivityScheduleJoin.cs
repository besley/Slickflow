using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Merge types to obtain the next node list
    /// 合并类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleJoin : NextActivityScheduleBase
    {
        internal NextActivityScheduleJoin(IProcessModel processModel) :
            base(processModel)
        {
            
        }

        /// <summary>
        /// Get Next Activity List from Gateway Node
        /// 获取下一步节点列表
        /// </summary>
        internal override NextActivityComponent GetNextActivityListFromGateway(Transition fromTransition,
            Activity currentGatewayActivity,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            NextActivityComponent gatewayComponent = null;
            resultType = NextActivityMatchedType.Unknown;

            //直接取出下步列表，运行时再根据条件执行
            //Directly retrieve the next step list and execute it based on the conditions during runtime
            List<Transition> transitionList = base.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityId,
                conditionKeyValuePair).ToList();
            foreach (Transition transition in transitionList)
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
