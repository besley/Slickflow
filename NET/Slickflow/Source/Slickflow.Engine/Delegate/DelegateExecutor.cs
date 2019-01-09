/*
* Slickflow 开源项目遵循LGPL协议，也可联系作者获取企业版商业授权和技术支持；
* 除此之外的使用则视为不正当使用，请您务必避免由此带来的一切商业版权纠纷和损失。
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
using Slickflow.Engine.Common;
using Slickflow.Engine.Core;

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
        /// <param name="context">活动上下文</param>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            ActivityForwardContext context)
        {
            InvokeExternalDelegate(session, eventType,
                context.ActivityResource.AppRunner.DelegateEventList,
                    context.FromActivityInstance.ID,
                    context.Activity.ActivityCode,
                    context.ActivityResource);
        }

        /// <summary>
        /// 调用外部业务应用的程序方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventList">事件列表</param>
        /// <param name="instanceID">实例ID</param>
        /// <param name="code">实例代码</param>
        /// <param name="activityResource">活动资源</param>
            internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            int instanceID,
            string code,
            ActivityResource activityResource)
        {
            //过滤注册事件类型
            var eventListFiltered = eventList.Where(k => k.Key == eventType);
            if (eventListFiltered != null)
            {
                //执行方法
                Execute(session, instanceID, code, eventListFiltered, activityResource);
            }
        }

        /// <summary>
        /// 调用外部业务应用的程序方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventList">事件列表</param>
        /// <param name="instanceID">实例ID</param>
        /// <param name="code">实例代码</param>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            int instanceID,
            string code = null)
        {
            //过滤注册事件类型
            var eventListFiltered = eventList.Where(k => k.Key == eventType);

            if (eventListFiltered != null)
            {
                //执行方法
                Execute(session, instanceID, code, eventListFiltered);
            }
        }

        /// <summary>
        /// 执行委托列表方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="id">实例主键ID</param>
        /// <param name="code">实例代码</param>
        /// <param name="eventList">事件列表</param>
        /// <param name="activityResource">活动资源</param>
        private static void Execute(IDbSession session,
            int id,
            string code,
            IEnumerable<KeyValuePair<EventFireTypeEnum, Func<int, string, IDelegateService, Boolean>>> eventList,
            ActivityResource activityResource = null)
        {
            foreach (var e in eventList)
            {
                Execute(session, id, code, e, activityResource);
            }
        }

        /// <summary>
        /// 执行委托方法
        /// </summary>
        /// <param name="session">数据会话</param>
        /// <param name="id">实例主键ID</param>
        /// <param name="code">实例代码</param>
        /// <param name="item">事件</param>
        /// <param name="activityResource">活动资源</param>
        /// <returns>执行结果</returns>
        private static Boolean Execute(IDbSession session,
            int id,
            string code,
            KeyValuePair<EventFireTypeEnum, Func<int, string, IDelegateService, Boolean>> item,
            ActivityResource activityResource = null)
        {
            var result = false;
            if (item.Key == EventFireTypeEnum.OnProcessStarted
                || item.Key == EventFireTypeEnum.OnProcessRunning
                || item.Key == EventFireTypeEnum.OnProcessCompleted)
            {
                var delegateService = new ProcessDelegateService(session, id);
                delegateService.SetActivityResource(activityResource);
                result = item.Value(id, code, delegateService);
            }
            return result;
        }
    }
}
