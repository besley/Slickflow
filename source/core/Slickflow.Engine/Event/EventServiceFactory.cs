using System;
using Slickflow.Data;
using Slickflow.Module.Localize;
using Slickflow.Engine.Event.Handlers;

namespace Slickflow.Engine.Event
{
    /// <summary>
    /// Event Service Factory
    /// 事件服务创建类
    /// </summary>
    public class EventServiceFactory
    {
        /// <summary>
        /// Create Event Service
        /// 创建事件服务
        /// </summary>
        public static EventServiceBase CreateEventService(EventScopeTypeEnum scopeType, 
            IDbSession session, 
            EventContext context)
        {
            if (scopeType == EventScopeTypeEnum.Process)
            {
                var processEventService = new ProcessEventService(session, context);
                return processEventService;
            }
            else if (scopeType == EventScopeTypeEnum.Activity)
            {
                var activityEventService = new ActivityEventService(session, context);
                return activityEventService;
            }
            else
            {
                throw new ApplicationException(LocalizeHelper.GetEngineMessage("eventservicefactory.CreateEventService.error"));
            }
        }
    }
}
