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
    /// 分支类型，获取下一步节点列表
    /// </summary>
    internal class NextActivityScheduleSplit : NextActivityScheduleBase
    {
        #region 属性及构造函数
        private Nullable<int> _taskID;
        internal Nullable<int> TaskID
        {
            get
            {
                return _taskID;
            }
        }

        internal NextActivityScheduleSplit(IProcessModel processModel, Nullable<int> taskID) : base(processModel)
        {
            _taskID = taskID;
        }
        #endregion

        /// <summary>
        /// 获取下一步节点列表
        /// </summary>
        /// <param name="fromTransition">起始转移</param>
        /// <param name="currentGatewayActivity">当前节点</param>
        /// <param name="conditionKeyValuePair">条件</param>
        /// <param name="session">会话</param>
        /// <param name="resultType">结果类型</param>
        /// <returns>返回节点</returns>
        internal override NextActivityComponent GetNextActivityListFromGateway(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity, 
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            NextActivityComponent gatewayComponent = null;
            resultType = NextActivityMatchedType.Unknown;
            var transitionList = this.ProcessModel.GetForwardTransitionList(currentGatewayActivity.ActivityGUID).ToList();

            if (currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplit
                || currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.AndSplitMI)
            {
                //获取AndSplit的每一条后续连线上的To节点
                foreach (TransitionEntity transition in transitionList)
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
                            session,
                            out resultType);

                        gatewayComponent = AddChildToGatewayComponent(fromTransition, currentGatewayActivity, gatewayComponent, child);
                    }
                }

                if (gatewayComponent == null)
                {
                    //没有分支满足，则选择默认分支流转
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
                            session,
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
            else if(currentGatewayActivity.GatewayDirectionType == GatewayDirectionEnum.ApprovalOrSplit)
            {
                var fromActivity = fromTransition.FromActivity;
                var aim = new ActivityInstanceManager();
                var nodePassedResult = aim.CheckActivityInstancePassedResult(this.TaskID, session);

                //根据节点通过类型连线类型
                if (nodePassedResult.NodePassedType == NodePassedTypeEnum.Passed)
                {
                    transitionList = transitionList.Where<TransitionEntity>(t => t.GroupBehaviours.Approval == (short)ApprovalStatusEnum.Agreed).ToList();
                }
                else if (nodePassedResult.NodePassedType == NodePassedTypeEnum.NotPassed)
                {
                    transitionList = transitionList.Where<TransitionEntity>(t => t.GroupBehaviours.Approval == (short)ApprovalStatusEnum.Refused).ToList();
                }
                else if (nodePassedResult.NodePassedType == NodePassedTypeEnum.NeedToBeMoreApproved)
                {
                    transitionList = transitionList.Where<TransitionEntity>(t => t.GroupBehaviours.Approval == (short)ApprovalStatusEnum.Agreed).ToList();
                }
                else
                {
                    transitionList = null;
                    resultType = NextActivityMatchedType.WaitingForAgreedOrRefused;
                }

                //获取有效的下一步节点
                if (transitionList != null)
                {
                    foreach (TransitionEntity transition in transitionList)
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
                    currentGatewayActivity.GatewayDirectionType.ToString()));
            }
            return gatewayComponent;
        }
    }
}
