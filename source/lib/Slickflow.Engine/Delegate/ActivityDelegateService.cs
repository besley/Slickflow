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
    /// 活动委托服务类
    /// </summary>
    public class ActivityDelegateService : DelegateServiceBase, IDelegateService
    {
        #region 属性及构造方法       
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="session">会话</param>
        /// <param name="context">上下文</param>
        public ActivityDelegateService(IDbSession session, DelegateContext context)
            : base(session, context)
        {

        }
        #endregion

        #region 获取及设置方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="id">实体主键ID</param>
        /// <returns>实体</returns>
        public override T GetInstance<T>(int id)
        {
            var aim = new ActivityInstanceManager();
            var entity = aim.GetById(Session.Connection, id, Session.Transaction);
            return entity as T;
        }
        #endregion
    }
}
