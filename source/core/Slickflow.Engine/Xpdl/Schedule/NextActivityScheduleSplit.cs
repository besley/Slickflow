using System;
using System.Collections.Generic;
using System.Linq;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Split type, obtain the next node list
    /// 分支类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleSplit : NextActivityScheduleBase
    {
        #region Property and Constructor
        private Nullable<int> _activityInstanceId;
        internal Nullable<int> ActivityInstanceId
        {
            get
            {
                return _activityInstanceId;
            }
        }

        internal NextActivityScheduleSplit(IProcessModel processModel, Nullable<int> activityInstanceId) : base(processModel)
        {
            _activityInstanceId = activityInstanceId;
        }
        #endregion

        /// <summary>
        /// Get next activity list from gateway
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
            var transitionList = this.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityId).ToList();

            if (currentGatewayActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplit
                || currentGatewayActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.AndSplitMI)
            {
                //获取AndSplit的每一条后续连线上的To节点
                //Obtain the To nodes on each subsequent connection of AndSplit
                foreach (Transition transition in transitionList)
                {
                    child = GetNextActivityListFromGatewayCore(transition,
                        conditionKeyValuePair,
                        session,
                        out resultType);

                    gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                }

                if (gatewayComponent == null)
                {
                    resultType = NextActivityMatchedType.WaitingForOtherSplitting;
                }
            }
            else if (currentGatewayActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.OrSplit)
            {
                //获取OrSplit的，满足条件的后续连线上的To节点
                //Obtain the To nodes on the subsequent connections of OrSplit that meet the conditions
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
                    //没有分支满足，则选择默认分支流转
                    //If no branch is met, select the default branch flow
                    var defaultTransition = transitionList.Find(t => t.GroupBehaviours != null && t.GroupBehaviours.DefaultBranch == true);
                    if (defaultTransition != null)
                    {
                        child = GetNextActivityListFromGatewayCore(defaultTransition,
                            conditionKeyValuePair,
                            session,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                    }
                    else
                    {
                        resultType = NextActivityMatchedType.NoneTransitionMatchedToSplit;
                    }
                }
            }
            else if (currentGatewayActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.XOrSplit)
            {
                //按连线定义的优先级排序
                //Sort by priority defined by the connection
                transitionList.Sort(new TransitionPriorityCompare());

                //获取XOrSplit的，第一条满足条件的后续连线上的To节点
                //Obtain the To node on the first subsequent connection that satisfies the condition for XOrSplit
                foreach (Transition transition in transitionList)
                {
                    bool isValidTransitionXOr = base.ProcessModel.IsValidTransition(transition, conditionKeyValuePair);
                    if (isValidTransitionXOr)
                    {
                        child = GetNextActivityListFromGatewayCore(transition,
                            conditionKeyValuePair,
                            session,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                        //退出循环
                        //Exit loop
                        break;
                    }
                }

                if (gatewayComponent == null)
                {
                    resultType = NextActivityMatchedType.NoneTransitionMatchedToSplit;
                }
            }
            else if(currentGatewayActivity.GatewayDetail.DirectionType == GatewayDirectionEnum.ApprovalOrSplit)
            {
                var fromActivity = fromTransition.FromActivity;
                var aim = new ActivityInstanceManager();
                var nodePassedResult = aim.CheckActivityInstancePassedResult(this.ActivityInstanceId, session);

                //根据节点通过类型连线类型
                //Connection type based on node connection type
                if (nodePassedResult.NodePassedType == NodePassedTypeEnum.Passed)
                {
                    transitionList = transitionList.Where<Transition>(t => t.GroupBehaviours.Approval == ApprovalStatusEnum.Agreed).ToList();
                }
                else if (nodePassedResult.NodePassedType == NodePassedTypeEnum.NotPassed)
                {
                    transitionList = transitionList.Where<Transition>(t => t.GroupBehaviours.Approval == ApprovalStatusEnum.Refused).ToList();
                }
                else if (nodePassedResult.NodePassedType == NodePassedTypeEnum.NeedToBeMoreApproved)
                {
                    transitionList = transitionList.Where<Transition>(t => t.GroupBehaviours.Approval == ApprovalStatusEnum.Agreed).ToList();
                }
                else
                {
                    transitionList = null;
                    resultType = NextActivityMatchedType.WaitingForAgreedOrRefused;
                }

                //获取有效的下一步节点
                //Obtain effective next step nodes
                if (transitionList != null)
                {
                    foreach (Transition transition in transitionList)
                    {
                        child = GetNextActivityListFromGatewayCore(transition,
                            conditionKeyValuePair,
                            session,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);

                        if (gatewayComponent == null)
                        {
                            resultType = NextActivityMatchedType.NoneTransitionMatchedToSplit;
                        }
                    }
                }
            }
            else
            {
                resultType = NextActivityMatchedType.Failed; 
                throw new WfXpdlException(LocalizeHelper.GetEngineMessage("nextactivityschedulesplit.error", 
                    currentGatewayActivity.GatewayDetail.DirectionType.ToString()));
            }
            return gatewayComponent;
        }
    }
}
