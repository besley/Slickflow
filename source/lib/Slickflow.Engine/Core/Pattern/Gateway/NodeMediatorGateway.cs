﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slickflow.Engine.Common;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Core.Pattern.Gateway
{
    /// <summary>
    /// 逻辑控制节点执行器
    /// </summary>
    internal class NodeMediatorGateway
    {
        #region 属性及构造方法
        private Activity _gatewayActivity;
        internal Activity GatewayActivity
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
        
        internal NodeMediatorGateway(Activity gActivity, IProcessModel processModel, IDbSession session)
        {
            _gatewayActivity = gActivity;
            _processModel = processModel;
            _session = session;
        }
        #endregion

        /// <summary>
        /// 创建节点对象
        /// </summary>
        /// <param name="activity">活动</param>
        /// <param name="processInstance">流程实例</param>
        /// <param name="runner">运行者</param>
        protected ActivityInstanceEntity CreateActivityInstanceObject(Activity activity,
            ProcessInstanceEntity processInstance,
            WfAppRunner runner)
        {
            ActivityInstanceManager aim = new ActivityInstanceManager();
            this.GatewayActivityInstance = aim.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ProcessGUID,
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
        /// <returns>新转移实例ID</returns>
        internal virtual int InsertTransitionInstance(ProcessInstanceEntity processInstance,
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
            var newID = tim.Insert(session.Connection, transitionInstanceObject, session.Transaction);

            return newID;
        }

        /// <summary>
        /// 节点对象的完成方法
        /// </summary>
        /// <param name="ActivityInstanceID">活动实例ID</param>
        /// <param name="runner">运行者</param>
        /// <param name="session">会话</param>
        internal virtual void CompleteActivityInstance(int ActivityInstanceID,
            WfAppRunner runner,
            IDbSession session)
        {
            //设置完成状态
            ActivityInstanceManager.Complete(ActivityInstanceID,
                runner,
                session);
        }
    }
}
