using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Schedule
    /// 节点调度基类
    /// </summary>
    internal abstract class NextActivityScheduleBase
    {
        #region Property and Constructor
        internal IProcessModel _processModel;
        internal IProcessModel ProcessModel
        {
            get
            {
                return _processModel;
            }
        }

        internal NextActivityScheduleBase(IProcessModel processModel)
        {
            _processModel = processModel;
        }
        #endregion

        /// <summary>
        /// Abstract method for obtaining the next node list based on gateway type
        /// 根据网关类型获取下一步节点列表的抽象方法
        /// </summary>
        internal abstract NextActivityComponent GetNextActivityListFromGateway(Transition fromTransition,
            Activity currentGatewayActivity,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType);


        /// <summary>
        /// Obtain the next node list based on Transition
        /// 根据Transition，获取下一步节点列表
        /// </summary>
        protected NextActivityComponent GetNextActivityListFromGatewayCore(Transition forwardTransition,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            if (XPDLHelper.IsSimpleComponentNode(forwardTransition.ToActivity.ActivityType) == true)       //可流转简单类型节点
            {
                child = NextActivityComponentFactory.CreateNextActivityComponent(forwardTransition, forwardTransition.ToActivity);
                resultType = NextActivityMatchedType.Successed;
            }
            else if (forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                child = GetNextActivityListFromGateway(forwardTransition, 
                    forwardTransition.ToActivity, 
                    conditionKeyValuePair,
                    session,
                    out resultType);
            }
            else if (forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.IntermediateNode)
            {
                if (forwardTransition.ToActivity.TriggerDetail.TriggerType == TriggerTypeEnum.Timer)
                {
                    child = NextActivityComponentFactory.CreateNextActivityComponent(forwardTransition, forwardTransition.ToActivity);
                    resultType = NextActivityMatchedType.Successed;
                }
                else
                {
                    NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(this.ProcessModel);
                    child = activitySchedule.GetNextActivityListFromGateway(forwardTransition,
                        forwardTransition.ToActivity,
                        conditionKeyValuePair,
                        session,
                        out resultType);
                }
            }
            else if (forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.ServiceNode)
            {
                NextActivityScheduleBase activitySchedule = NextActivityScheduleFactory.CreateActivityScheduleIntermediate(this.ProcessModel);
                child = activitySchedule.GetNextActivityListFromGateway(forwardTransition,
                    forwardTransition.ToActivity,
                    conditionKeyValuePair,
                    session,
                    out resultType);
            }
            else
            {
                resultType = NextActivityMatchedType.Failed;

                throw new XmlDefinitionException(LocalizeHelper.GetEngineMessage("nextactivityschedulebase.unknownnodetype", forwardTransition.ToActivity.ActivityType.ToString()));
            }
            return child;
        }

        /// <summary>
        /// Add child nodes to the gateway routing node, 
        /// and process them based on whether the gateway node and child nodes are empty or not
        /// 把子节点添加到网关路由节点，根据网关节点和子节点是否为空处理
        /// </summary>
        protected NextActivityComponent AddChildToGatewayComponent(Transition fromTransition,
            Activity currentGatewayActivity,
            NextActivityComponent gatewayComponent,
            NextActivityComponent child)
        {
            if ((gatewayComponent == null) && (child != null))
                gatewayComponent = NextActivityComponentFactory.CreateNextActivityComponent(fromTransition, currentGatewayActivity);

            if ((gatewayComponent != null) && (child != null))
                gatewayComponent.Add(child);

            return gatewayComponent;
        }
    }
}
