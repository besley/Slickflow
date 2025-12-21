using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.BizAppService.Entity;
using Slickflow.BizAppService.Interface;

namespace Slickflow.BizAppService.Service
{
    /// <summary>
    /// Appflow Service
    /// </summary>
    public class AppFlowService : ServiceBase, IAppFlowService
    {
        /// <summary>
        /// App Flow Data Paged
        /// </summary>
        public List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count)
        {
            List<AppFlowEntity> list = null;
            var conn = SessionFactory.CreateConnection();

            try
            {
                var sortList = new List<DapperExtensions.ISort>();
                sortList.Add(new DapperExtensions.Sort { PropertyName = "Id", Ascending = false });

                IFieldPredicate predicate = null;
                if (!string.IsNullOrEmpty(query.AppInstanceId))
                    predicate = Predicates.Field<AppFlowEntity>(f => f.AppInstanceId, DapperExtensions.Operator.Eq, query.AppInstanceId);

                count = QuickRepository.Count<AppFlowEntity>(conn, predicate);
                list = QuickRepository.GetPaged<AppFlowEntity>(conn, query.PageIndex, query.PageSize,
                    predicate, sortList, false).ToList();

                return list;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
