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

namespace Slickflow.Engine.Event
{
    /// <summary>
    /// Event Executor
    /// 事件执行器
    /// </summary>
    internal class EventExecutor
    {
        /// <summary>
        /// Call program methods for external business applications
        /// 调用外部业务应用的程序方法
        /// </summary>
        internal static void InvokeExternalEvent(IDbSession session,
            EventFireTypeEnum eventType,
            Activity currentActivity,
            ActivityForwardContext context)
        {
            var eventContext = new EventContext
            {
                AppInstanceId = context.ProcessInstance.AppInstanceId,
                ProcessId = context.ProcessInstance.ProcessId,
                ProcessInstanceId = context.ProcessInstance.Id,
                ActivityId = currentActivity.ActivityId,
                ActivityName = currentActivity.ActivityName,
                ActivityCode = currentActivity.ActivityCode,
                ActivityResource = context.ActivityResource
            };
            InvokeExternalEvent(session, eventType,
                context.ActivityResource.AppRunner.EventSubscriptionList,
                    eventContext);
        }

        /// <summary>
        /// Call program methods for external business applications
        /// 调用外部业务应用的程序方法
        /// </summary>
        internal static void InvokeExternalEvent(IDbSession session,
            EventFireTypeEnum eventType,
            EventSubscriptionList eventList,
            int processInstanceId)
        {
            //过滤注册事件类型
            //Filter registration event types
            var eventListFiltered = eventList.Where(k => k.Key == eventType);
            if (eventListFiltered != null)
            {
                var pim = new ProcessInstanceManager();
                var entity = pim.GetById(session.Connection, processInstanceId, session.Transaction);

                var context = new EventContext
                {
                    AppInstanceId = entity.AppInstanceId,
                    ProcessId = entity.ProcessId,
                    ProcessInstanceId = processInstanceId
                };
                Execute(session, context, eventListFiltered);
            }
        }

        /// <summary>
        /// Call program methods for external business applications
        /// 调用外部业务应用的程序方法
        /// </summary>
        internal static void InvokeExternalEvent(IDbSession session,
            EventFireTypeEnum eventType,
            EventSubscriptionList eventList,
            EventContext context)
        {
            //过滤注册事件类型
            //Filter registration event types
            var eventListFiltered = eventList.Where(k => k.Key == eventType);

            if (eventListFiltered != null)
            {
                Execute(session, context, eventListFiltered);
            }
        }

        /// <summary>
        /// Execution event subscription list method
        /// 执行事件订阅列表方法
        /// </summary>
        private static void Execute(IDbSession session,
            EventContext context,
            IEnumerable<KeyValuePair<EventFireTypeEnum, Func<EventContext, IEventService, Boolean>>> eventList)
        {
            foreach (var e in eventList)
            {
                Execute(session, context, e);
            }
        }

        /// <summary>
        /// Execution event subscription method
        /// 执行事件订阅方法
        /// </summary>
        private static Boolean Execute(IDbSession session,
            EventContext context,
            KeyValuePair<EventFireTypeEnum, Func<EventContext, IEventService, Boolean>> item)
        {
            var result = false;
            if (item.Key == EventFireTypeEnum.OnProcessStarted
                || item.Key == EventFireTypeEnum.OnProcessRunning
                || item.Key == EventFireTypeEnum.OnProcessCompleted)
            {
                var eventService = EventServiceFactory.CreateEventService(EventScopeTypeEnum.Process,
                    session, context);
                eventService.SetActivityResource(context.ActivityResource);
                result = item.Value(context, eventService as IEventService);
            }
            else if (item.Key == EventFireTypeEnum.OnActivityCreated
                || item.Key == EventFireTypeEnum.OnActivityExecuting
                || item.Key == EventFireTypeEnum.OnActivityExecuted
                || item.Key == EventFireTypeEnum.OnActivityCompleted)
            {
                var eventService = EventServiceFactory.CreateEventService(EventScopeTypeEnum.Activity,
                    session, context);
                eventService.SetActivityResource(context.ActivityResource);
                result = item.Value(context, eventService as IEventService);
            }
            return result;
        }
    }
}
