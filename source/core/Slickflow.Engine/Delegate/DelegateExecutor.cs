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
    /// Delegate Agent Executor
    /// 委托代理执行器
    /// </summary>
    internal class DelegateExecutor
    {
        /// <summary>
        /// Call program methods for external business applications
        /// 调用外部业务应用的程序方法
        /// </summary>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            Activity currentActivity,
            ActivityForwardContext context)
        {
            var delegateContext = new DelegateContext
            {
                AppInstanceId = context.ProcessInstance.AppInstanceId,
                ProcessId = context.ProcessInstance.ProcessId,
                ProcessInstanceId = context.ProcessInstance.Id,
                ActivityId = currentActivity.ActivityId,
                ActivityName = currentActivity.ActivityName,
                ActivityCode = currentActivity.ActivityCode,
                ActivityResource = context.ActivityResource
            };
            InvokeExternalDelegate(session, eventType,
                context.ActivityResource.AppRunner.DelegateEventList,
                    delegateContext);
        }

        /// <summary>
        /// Call program methods for external business applications
        /// 调用外部业务应用的程序方法
        /// </summary>
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            int processInstanceId)
        {
            //过滤注册事件类型
            //Filter registration event types
            var eventListFiltered = eventList.Where(k => k.Key == eventType);
            if (eventListFiltered != null)
            {
                var pim = new ProcessInstanceManager();
                var entity = pim.GetById(session.Connection, processInstanceId, session.Transaction);

                var context = new DelegateContext
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
        internal static void InvokeExternalDelegate(IDbSession session,
            EventFireTypeEnum eventType,
            DelegateEventList eventList,
            DelegateContext context)
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
        /// Execution delegation list method
        /// 执行委托列表方法
        /// </summary>
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
        /// Execution delegation method
        /// 执行委托方法
        /// </summary>
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
