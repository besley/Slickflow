using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;

namespace Slickflow.Engine.Xpdl
{
    internal abstract class NextActivityScheduleBase
    {
        #region 属性和构造函数
        internal ProcessModel _processModel;
        internal ProcessModel ProcessModel
        {
            get
            {
                return _processModel;
            }
        }

        internal NextActivityScheduleBase(ProcessModel processModel)
        {
            _processModel = processModel;
        }
        #endregion

        /// <summary>
        /// 根据网关类型获取下一步节点列表的抽象方法
        /// </summary>
        /// <param name="processInstanceID"></param>
        /// <param name="transition"></param>
        /// <param name="activity"></param>
        /// <param name="conditionKeyValuePair"></param>
        /// <returns></returns>
        internal abstract NextActivityComponent GetNextActivityListFromGateway(TransitionEntity transition,
            ActivityEntity activity,
            IDictionary<string, string> conditionKeyValuePair,
            out NextActivityMatchedType scheduleStatus);


        /// <summary>
        /// 根据Transition，获取下一步节点列表
        /// </summary>
        /// <param name="nextActivityList"></param>
        /// <param name="processInstanceID"></param>
        /// <param name="currentGatewayActivity"></param>
        /// <param name="forwardTransition"></param>
        /// <param name="conditionKeyValuePair"></param>
        protected NextActivityComponent GetNextActivityListFromGatewayCore(TransitionEntity forwardTransition,
            IDictionary<string, string> conditionKeyValuePair,
            out NextActivityMatchedType resultType)
        {
            NextActivityComponent child = null;
            if (forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.TaskNode
                || forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.EndNode)
            {
                child = NextActivityComponentFactory.CreateNextActivityComponent(forwardTransition, forwardTransition.ToActivity);
                resultType = NextActivityMatchedType.Successed;
            }
            else if (forwardTransition.ToActivity.ActivityType == ActivityTypeEnum.GatewayNode)
            {
                child = GetNextActivityListFromGateway(forwardTransition, 
                    forwardTransition.ToActivity, 
                    conditionKeyValuePair,
                    out resultType);
            }
            else
            {
                resultType = NextActivityMatchedType.Failed;

                throw new XmlDefinitionException(string.Format("未知的节点类型：{0}", forwardTransition.ToActivity.ActivityType.ToString()));
            }
            return child;
        }

        /// <summary>
        /// 把子节点添加到网关路由节点，根据网关节点和子节点是否为空处理
        /// </summary>
        /// <param name="gatewayComponent"></param>
        /// <param name="child"></param>
        /// <param name="currentGatewayActivity"></param>
        /// <returns></returns>
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
