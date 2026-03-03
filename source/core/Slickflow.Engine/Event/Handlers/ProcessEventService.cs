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
    /// Process Event Service
    /// 流程事件服务类
    /// </summary>
    public class ProcessEventService : EventServiceBase, IEventService
    {
        #region Property and Constructor
        public ProcessEventService(IDbSession session, EventContext context) 
            : base(session, context)
        {
            
        }
        #endregion

        /// <summary>
        /// Get Instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public override T GetInstance<T>(int id)
        {
            var pim = new ProcessInstanceManager();
            var entity = pim.GetById(Session.Connection, id, Session.Transaction);
            return entity as T;
        }
    }
}
