using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Delegate.Event
{
    /// <summary>
    /// Process Delegate Service
    /// 流程委托服务类
    /// </summary>
    public class ProcessEventService : DelegateServiceBase, IDelegateService
    {
        #region Property and Constructor
        public ProcessEventService(IDbSession session, DelegateContext context) 
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
