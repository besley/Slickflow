using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 节点调度基类
    /// </summary>
    internal abstract class NextActivityScheduleBase
    {
        #region 属性和构造函数
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
        /// 根据网关类型获取下一步节点列表的抽象方法
        /// </summary>
        /// <param name="fromTransition">转移</param>
        /// <param name="currentGatewayActivity">活动</param>
        /// <param name="conditionKeyValuePair">条件kv对</param>
        /// <param name="session">会话</param>
        /// <param name="resultType">匹配类型</param>
        /// <returns></returns>
        internal abstract NextActivityComponent GetNextActivityListFromGateway(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity,
            IDictionary<string, string> conditionKeyValuePair,
            IDbSession session,
            out NextActivityMatchedType resultType);


        /// <summary>
        /// 根据Transition，获取下一步节点列表
        /// </summary>
        /// <param name="forwardTransition">转移实体</param>
        /// <param name="conditionKeyValuePair">条件kv对</param>
        /// <param name="session">会话</param>
        /// <param name="resultType">结果类型</param>
        protected NextActivityComponent GetNextActivityListFromGatewayCore(TransitionEntity forwardTransition,
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
                if (forwardTransition.ToActivity.ActivityTypeDetail.TriggerType == TriggerTypeEnum.Timer)
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
        /// 把子节点添加到网关路由节点，根据网关节点和子节点是否为空处理
        /// </summary>
        /// <param name="fromTransition">起始转移</param>
        /// <param name="currentGatewayActivity">当前网关节点</param>
        /// <param name="gatewayComponent">网关Component</param>
        /// <param name="child">子节点</param>
        /// <returns>下一步Component</returns>
        protected NextActivityComponent AddChildToGatewayComponent(TransitionEntity fromTransition,
            ActivityEntity currentGatewayActivity,
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
