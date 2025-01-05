using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next scheduling for intermediate event nodes
    /// 中间事件节点下一步调度
    /// </summary>
    internal class NextActivityScheduleIntermediate : NextActivityScheduleBase
    {
        internal NextActivityScheduleIntermediate(IProcessModel processModel) :
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

            var transitionList = this.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityGUID).ToList();
            foreach (Transition transition in transitionList)
            {
                bool isValidTransition = base.ProcessModel.IsValidTransition(transition, conditionKeyValuePair);
                if (isValidTransition)
                {
                    child = GetNextActivityListFromGatewayCore(transition,
                        conditionKeyValuePair,
                        session,
                        out resultType);

                    gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                }
            }

            if (gatewayComponent == null)
            {
                resultType = NextActivityMatchedType.NoneTransitionFilteredByCondition;
            }
            return gatewayComponent;
        }
    }
}
