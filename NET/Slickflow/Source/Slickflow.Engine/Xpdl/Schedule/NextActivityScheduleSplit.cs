using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 分支类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleSplit : NextActivityScheduleBase
    {
        internal NextActivityScheduleSplit(IProcessModel processModel) : base(processModel)
        {
            
        }

        /// <summary>
        /// 获取下一步节点列表
        /// </summary>
        /// <param name="fromTransition">起始转移</param>
        /// <param name="currentGatewayActivity">当前节点</param>
        /// <param name="conditionKeyValuePair">条件</param>
        /// <param name="resultType">结果类型</param>
        /// <returns>返回节点</returns>
        internal override NextActivityComponent GetNextActivityListFromGateway(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity, 
            IDictionary<string, string> conditionKeyValuePair,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            NextActivityComponent gatewayComponent = null;
            resultType = NextActivityMatchedType.Unknown;
            var transitionList = this.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityGUID).ToList();

            if (currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplit
                || currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
            {
                //判读连线上的条件是否都满足，如果都满足才可以取出后续节点列表
                bool isCheckedOk = base.ProcessModel.CheckAndSplitOccurrenceCondition(transitionList, conditionKeyValuePair);
                if (isCheckedOk)
                {
                    //获取AndSplit的每一条后续连线上的To节点
                    foreach (TransitionEntity transition in transitionList)
                    {
                        child = GetNextActivityListFromGatewayCore(transition,
                            conditionKeyValuePair,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                    }
                }

                if (gatewayComponent == null)
                {
                    resultType = NextActivityMatchedType.WaitingForSplitting;
                }
            }
            else if (currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.OrSplit)
            {
                //获取OrSplit的，满足条件的后续连线上的To节点
                foreach (TransitionEntity transition in transitionList)
                {
                    bool isValidTransition = base.ProcessModel.IsValidTransition(transition, conditionKeyValuePair);
                    if (isValidTransition)
                    {
                        child = GetNextActivityListFromGatewayCore(transition,
                            conditionKeyValuePair,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                    }

                    if (gatewayComponent == null)
                    {
                        resultType = NextActivityMatchedType.NoneTransitionMatchedToSplit;
                    }
                }
            }
            else if (currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.XOrSplit)
            {
                //按连线定义的优先级排序
                transitionList.Sort(new TransitionPriorityCompare());

                //获取XOrSplit的，第一条满足条件的后续连线上的To节点
                foreach (TransitionEntity transition in transitionList)
                {
                    bool isValidTransitionXOr = base.ProcessModel.IsValidTransition(transition, conditionKeyValuePair);
                    if (isValidTransitionXOr)
                    {
                        child = GetNextActivityListFromGatewayCore(transition,
                            conditionKeyValuePair,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                        //退出循环
                        break;
                    }
                }

                if (gatewayComponent == null)
                {
                    resultType = NextActivityMatchedType.NoneTransitionMatchedToSplit;
                }
            }
            else if (currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.ComplexSplit)
            {
                resultType = NextActivityMatchedType.Failed; 
                throw new Exception("ComplexSplit 没有具体实现！");
            }
            else
            {
                resultType = NextActivityMatchedType.Failed; 
                throw new Exception("Split 分支节点的类型不明确！");
            }

            return gatewayComponent;
        }
    }
}
