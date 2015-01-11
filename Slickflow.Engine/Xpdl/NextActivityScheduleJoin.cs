using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    /// <summary>
    /// 合并类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleJoin : NextActivityScheduleBase
    {

        internal NextActivityScheduleJoin(ProcessModel processModel) :
            base(processModel)
        {
            
        }

                /// <summary>
        /// 获取下一步节点列表
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="fromTransition"></param>
        /// <param name="currentGatewayActivity"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
        internal override NextActivityComponent GetNextActivityListFromGateway(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity,
            IDictionary<string, string> conditionKeyValuePair,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            NextActivityComponent gatewayComponent = null;
            resultType = NextActivityMatchedType.Unknown;

            //直接取出下步列表，运行时再根据条件执行
            List<TransitionEntity> transitionList = base.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityGUID).ToList();
            foreach (TransitionEntity transition in transitionList)
            {
                child = GetNextActivityListFromGatewayCore(transition,
                    conditionKeyValuePair,
                    out resultType);

                gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
            }

            return gatewayComponent;
        }
    }
}
