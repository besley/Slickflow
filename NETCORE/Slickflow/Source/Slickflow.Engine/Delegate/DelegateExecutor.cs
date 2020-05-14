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
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Common;
using Slickflow.Engine.Core;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// 委托代理执行器
    /// </summary>
    internal class DelegateExecutor
    {
        /// <summary>
        /// 调用外部业务应用的程序方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="currentActivity">当前活动节点</param>
        /// <param name="context">活动上下文</param>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            ActivityEntity currentActivity,
            ActivityForwardContext context)
        {
            //默认为linker.FromActivity表示执行当前运行节点
            //linker.ToActivity != null 为运行事件类型的节点
            var delegateContext = new DelegateContext
            {
                AppInstanceID = context.ProcessInstance.AppInstanceID,
                ProcessGUID = context.ProcessInstance.ProcessGUID,
                ProcessInstanceID = context.ProcessInstance.ID,
                ActivityGUID = currentActivity.ActivityGUID,
                ActivityName = currentActivity.ActivityName,
                ActivityCode = currentActivity.ActivityCode,
                ActivityResource = context.ActivityResource
            };
            InvokeExternalDelegate(session, eventType,
                context.ActivityResource.AppRunner.DelegateEventList,
                    delegateContext);
        }

        /// <summary>
        /// 调用外部业务应用的程序方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventList">事件列表</param>
        /// <param name="processInstanceID">流程实例ID</param>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            int processInstanceID)
        {
            //过滤注册事件类型
            var eventListFiltered = eventList.Where(k => k.Key == eventType);
            if (eventListFiltered != null)
            {
                var pim = new ProcessInstanceManager();
                var entity = pim.GetById(session.Connection, processInstanceID, session.Transaction);
                //执行方法
                var context = new DelegateContext
                {
                    AppInstanceID = entity.AppInstanceID,
                    ProcessGUID = entity.ProcessGUID,
                    ProcessInstanceID = processInstanceID
                };
                Execute(session, context, eventListFiltered);
            }
        }

        /// <summary>
        /// 调用外部业务应用的程序方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventList">事件列表</param>
        /// <param name="context">上下文</param>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            DelegateContext context)
        {
            //过滤注册事件类型
            var eventListFiltered = eventList.Where(k => k.Key == eventType);

            if (eventListFiltered != null)
            {
                //执行方法
                Execute(session, context, eventListFiltered);
            }
        }

        /// <summary>
        /// 执行委托列表方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="context">上下文</param>
        /// <param name="eventList">事件列表</param>
        private static void Execute(IDbSession session,
            DelegateContext context,
            IEnumerable<KeyValuePair<EventFireTypeEnum, Func<DelegateContext, IDelegateService, Boolean>>> eventList)
        {
            foreach (var e in eventList)
            {
                Execute(session, context, e);
            }
        }

        /// <summary>
        /// 执行委托方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="context">上下文</param>
        /// <param name="item">事件</param>
        /// <returns>执行结果</returns>
        private static Boolean Execute(IDbSession session,
            DelegateContext context,
            KeyValuePair<EventFireTypeEnum, Func<DelegateContext, IDelegateService, Boolean>> item)
        {
            var result = false;
            if (item.Key == EventFireTypeEnum.OnProcessStarted
                || item.Key == EventFireTypeEnum.OnProcessRunning
                || item.Key == EventFireTypeEnum.OnProcessCompleted)
            {
                var delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Process,
                    session, context);
                delegateService.SetActivityResource(context.ActivityResource);
                result = item.Value(context, delegateService as IDelegateService);
            }
            else if (item.Key == EventFireTypeEnum.OnActivityCreated
                || item.Key == EventFireTypeEnum.OnActivityExecuting
                || item.Key == EventFireTypeEnum.OnActivityExecuted
                || item.Key == EventFireTypeEnum.OnActivityCompleted)
            {
                var delegateService = DelegateServiceFactory.CreateDelegateService(DelegateScopeTypeEnum.Activity,
                    session, context);
                delegateService.SetActivityResource(context.ActivityResource);
                result = item.Value(context, delegateService as IDelegateService);
            }
            return result;
        }
    }
}
