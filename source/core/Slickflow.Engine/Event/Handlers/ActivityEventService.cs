using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;
using Slickflow.Engine.Event;

namespace Slickflow.Engine.Event.Handlers
{
    /// <summary>
    /// Activity Event Service
    /// 活动事件服务类
    /// </summary>
    public class ActivityEventService : EventServiceBase, IEventService
    {    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        /// <param name="context"></param>
        public ActivityEventService(IDbSession session, EventContext context)
            : base(session, context)
        {

        }

        /// <summary>
        /// Get Instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T GetInstance<T>(int id)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetById(Session.Connection, id, Session.Transaction);
            return entity as T;
        }
    }
}
