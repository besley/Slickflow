/*
* Slickflow 工作流引擎遵循LGPL协议，也可联系作者商业授权并获取技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的商业版权纠纷。
* 
The Slickflow project.
Copyright (C) 2014  .NET Workflow Engine Library

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, you can access the official
web page about lgpl: https://www.gnu.org/licenses/lgpl.html
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Xpdl;
using Slickflow.Engine.Xpdl.Entity;
using Slickflow.Engine.Delegate;

namespace Slickflow.Engine.Core.Pattern.Event
{
    /// <summary>
    /// 逻辑控制节点执行器
    /// </summary>
    internal class NodeMediatorEvent
    {
        #region 属性及构造方法
        /// <summary>
        /// 活动上下文
        /// </summary>
        private ActivityForwardContext _activityFowardContext;
        /// <summary>
        /// 活动上下文
        /// </summary>
        internal ActivityForwardContext ActivityForwardContext
        {
            get { return _activityFowardContext; }
           
        }

        /// <summary>
        /// 事件活动
        /// </summary>
        private ActivityEntity _eventActivity;
        /// <summary>
        /// 事件活动
        /// </summary>
        internal ActivityEntity EventActivity
        {
            get { return _eventActivity; }
        }

        /// <summary>
        /// 流程模型
        /// </summary>
        private IProcessModel _processModel;
        /// <summary>
        /// 流程模型
        /// </summary>
        internal IProcessModel ProcessModel
        {
            get { return _processModel; }
        }

        /// <summary>
        /// 数据会话
        /// </summary>
        private IDbSession _session;
        /// <summary>
        /// 数据会话
        /// </summary>
        internal IDbSession Session
        {
            get { return _session; }
        }

        /// <summary>
        /// 事件活动实例
        /// </summary>
        internal ActivityInstanceEntity EventActivityInstance
        {
            get;
            set;
        }

        /// <summary>
        /// 活动实例管理
        /// </summary>
        private ActivityInstanceManager activityInstanceManager;
        /// <summary>
        /// 活动实例管理
        /// </summary>
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="forwardContext">上下文</param>
        /// <param name="eActivity">活动</param>
        /// <param name="processModel">流程模型</param>
        /// <param name="session">会话</param>
        internal NodeMediatorEvent(ActivityForwardContext forwardContext,
            ActivityEntity eActivity, 
            IProcessModel processModel, 
            IDbSession session)
        {
            _activityFowardContext = forwardContext;
            _eventActivity = eActivity;
            _processModel = processModel;
            _session = session;
        }
        #endregion

        #region 节点逻辑及事件响应执行
        internal virtual void ExecuteWorkItem() { }

        /// <summary>
        /// 执行外部操作的方法
        /// </summary>
        /// <param name="actionList">操作列表</param>
        /// <param name="delegateService">委托方法</param>
        protected void ExecteActionList(IList<ActionEntity> actionList,
            IDelegateService delegateService)
        {
            if (actionList != null && actionList.Count > 0)
            {
                ActionExecutor.ExecteActionList(actionList, delegateService);
            }
        }

        /// <summary>
        /// 获取委托服务
        /// </summary>
        /// <returns>委托服务类</returns>
        private DelegateServiceBase GetDelegateService()
        {
            //执行Action列表
            var delegateContext = new DelegateContext
            {
                AppInstanceID = ActivityForwardContext.ProcessInstance.AppInstanceID,
                ProcessGUID = ActivityForwardContext.ProcessInstance.ProcessGUID,
                ProcessInstanceID = ActivityForwardContext.ProcessInstance.ID,
                ActivityGUID = ActivityForwardContext.FromActivityInstance.ActivityGUID,
                ActivityName = ActivityForwardContext.FromActivityInstance.ActivityName
            };

            var delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Activity,
                this.Session,
                delegateContext);
            return delegateService;
        }

        /// <summary>
        /// 触发前执行
        /// </summary>
        protected void OnBeforeExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = this.EventActivity.ActionList;
            ActionExecutor.ExecteActionListBefore(actionList, delegateService as IDelegateService);

            //----> 节点流转前，调用活动执行的委托事件
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuting,
                this.EventActivity,
                ActivityForwardContext);
        }

        /// <summary>
        /// 执行代码自动服务内容
        /// </summary>
        protected void OnExecutingServiceItem()
        {
            var delegateService = GetDelegateService();
            var serviceList = this.EventActivity.ServiceList;
            ServiceExecutor.ExecteServiceList(serviceList, delegateService as IDelegateService);
        }

        /// <summary>
        /// 触发后执行
        /// </summary>
        protected void OnAfterExecuteWorkItem()
        {
            var delegateService = GetDelegateService();
            var actionList = this.EventActivity.ActionList;
            ActionExecutor.ExecteActionListAfter(actionList, delegateService as IDelegateService);

            //----> 节点流转完成后，调用活动完成执行的委托事件
            DelegateExecutor.InvokeExternalDelegate(this.Session,
                EventFireTypeEnum.OnActivityExecuted,
                this.EventActivity,
                ActivityForwardContext);
        }
        #endregion

        #region 节点创建
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
            this.EventActivityInstance = aim.CreateActivityInstanceObject(processInstance.AppName,
                processInstance.AppInstanceID,
                processInstance.AppInstanceCode,
                processInstance.ID,
                activity,
                runner);

            return this.EventActivityInstance;
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
        #endregion
    }
}
