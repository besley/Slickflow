/*
* Slickflow 软件遵循自有项目开源协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
* 
The Slickflow Open License (SfPL 1.0)
Copyright (C) 2014  .NET Workflow Engine Library

1. Slickflow software must be legally used, and should not be used in violation of law, 
   morality and other acts that endanger social interests;
2. Non-transferable, non-transferable and indivisible authorization of this software;
3. The source code can be modified to apply Slickflow components in their own projects 
   or products, but Slickflow source code can not be separately encapsulated for sale or 
   distributed to third-party users;
4. The intellectual property rights of Slickflow software shall be protected by law, and
   no documents such as technical data shall be made public or sold.
5. The enterprise, ultimate and universe version can be provided with commercial license, 
   technical support and upgrade service.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Node;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 逻辑控制节点执行器
    /// </summary>
    internal class NodeMediatorGateway
    {
        private ActivityEntity _gatewayActivity;
        internal ActivityEntity GatewayActivity
        {
            get { return _gatewayActivity; }
        }

        private IProcessModel _processModel;
        internal IProcessModel ProcessModel
        {
            get { return _processModel; }
        }

        private IDbSession _session;
        internal IDbSession Session
        {
            get { return _session; }
        }

        internal ActivityInstanceEntity GatewayActivityInstance
        {
            get;
            set;
        }

        private ActivityInstanceManager activityInstanceManager;
        internal ActivityInstanceManager ActivityInstanceManager
        {
            get
            {
                if (activityInstanceManager == null)
                {
                    activityInstanceManager = new ActivityInstanceManager();
                }
                return activityInstanceManager;
            }
        }
        
        internal NodeMediatorGateway(ActivityEntity gActivity, IProcessModel processModel, IDbSession session)
        {
            _gatewayActivity = gActivity;
            _processModel = processModel;
            _session = session;
        }

        /// <summary>
        /// 创建节点对象
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="runner">运行者</param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(ActivityEntity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceManager aim = new ActivityInstanceManager();
            this.GatewayActivityInstance = aim.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.ID,
                activity,
                runner);

            return this.GatewayActivityInstance;
        }

        /// <summary>
        /// 插入实例数据
        /// </summary>
        /// <param name="activityInstance">活动资源</param>
        /// <param name="session">会话</param>
        internal virtual void InsertActivityInstance(ActivityInstanceEntity activityInstance,
            IDbSession session)
        {
            ActivityInstanceManager.Insert(activityInstance, session);
        }

        /// <summary>
        /// 插入连线实例的方法
        /// </summary>
        /// <param name="processInstance">流程实例</param>
        /// <param name="transitionGUID">转移GUID</param>
        /// <param name="fromActivityInstance">来源活动实例</param>
        /// <param name="toActivityInstance">目的活动实例</param>
        /// <param name="transitionType">转移类型</param>
        /// <param name="flyingType">飞跃类型</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        /// <returns></returns>
        internal virtual void InsertTransitionInstance(ProcessInstanceEntity processInstance,
            string transitionGUID,
            ActivityInstanceEntity fromActivityInstance,
            ActivityInstanceEntity toActivityInstance,
            TransitionTypeEnum transitionType,
            TransitionFlyingTypeEnum flyingType,
            WfAppRunner runner,
            IDbSession session)
        {
            var tim = new TransitionInstanceManager();
            var transitionInstanceObject = tim.CreateTransitionInstanceObject(processInstance,
                transitionGUID,
                fromActivityInstance,
                toActivityInstance,
                transitionType,
                flyingType,
                runner,
                (byte)ConditionParseResultEnum.Passed);

            tim.Insert(session.Connection, transitionInstanceObject, session.Transaction);
        }

        /// <summary>
        /// 节点对象的完成方法
        /// </summary>
        /// <param name="ActivityInstanceID">活动实例ID</param>
        /// <param name="activityResource">活动资源</param>
        /// <param name="session">会话</param>
        internal virtual void CompleteActivityInstance(int ActivityInstanceID,
            ActivityResource activityResource,
            IDbSession session)
        {
            //设置完成状态
            ActivityInstanceManager.Complete(ActivityInstanceID,
                activityResource.AppRunner,
                session);
        }
		/// <summary>
        /// 获取分支实例的个数
        /// </summary>
        /// <param name="splitActivityInstanceGUID"></param>
        /// <param name="processInstanceID"></param>
        /// <returns></returns>
        protected int GetInstanceGatewayCount(string splitActivityInstanceGUID,int processInstanceID)
        {
            ActivityInstanceManager aim = new ActivityInstanceManager();
            return aim.GetInstanceGatewayCount(splitActivityInstanceGUID, processInstanceID);
        }
    }
}
