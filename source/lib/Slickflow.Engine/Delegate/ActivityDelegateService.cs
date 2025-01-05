using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Business.Manager;

namespace Slickflow.Engine.Delegate
{
    /// <summary>
    /// Activity Delegate Service
    /// 活动委托服务类
    /// </summary>
    public class ActivityDelegateService : DelegateServiceBase, IDelegateService
    {    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="session"></param>
        /// <param name="context"></param>
        public ActivityDelegateService(IDbSession session, DelegateContext context)
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
