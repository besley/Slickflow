using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.ModelDemo.Entity;
using Slickflow.ModelDemo.Interface;
using Slickflow.ModelDemo.Data;

namespace Slickflow.ModelDemo.Service
{
    /// <summary>
    /// 流转记录器
    /// </summary>
    public class AppFlowService : IAppFlowService
    {
        /// <summary>
        /// 流程业务记录分页方法
        /// </summary>
        /// <param name="query">查询实体</param>
        /// <param name="count">数目</param>
        /// <returns></returns>
        public List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count)
        {
            using (var session = DbFactory.CreateSession())
            {
                var appFlowDbSet = session.GetRepository<AppFlowEntity>().GetDbSet();
                var sqlQuery = (from vw in appFlowDbSet
                                select vw);
                if (!string.IsNullOrEmpty(query.AppInstanceID)) {
                    sqlQuery = sqlQuery.Where(e => e.AppInstanceID == query.AppInstanceID);
                }
                count = sqlQuery.Count();
                var list = sqlQuery.OrderByDescending(e=>e.ID)
                    .Skip(query.PageIndex * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();
                return list;
            }
        }
    }
}
