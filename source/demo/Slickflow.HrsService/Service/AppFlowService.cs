using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.HrsService.Entity;
using Slickflow.HrsService.Interface;

namespace Slickflow.HrsService.Service
{
    /// <summary>
    /// Application flow service
    /// </summary>
    public class AppFlowService : ServiceBase, IAppFlowService
    {
        /// <summary>
        /// appflow data paged
        /// </summary>
        public List<AppFlowEntity> GetPaged(AppFlowQuery query, out int count)
        {
            List<AppFlowEntity> list = null;
            var conn = SessionFactory.CreateConnection();

            try
            {
                var sortList = new List<DapperExtensions.ISort>();
                sortList.Add(new DapperExtensions.Sort { PropertyName = "ID", Ascending = false });

                IFieldPredicate predicate = null;
                if (!string.IsNullOrEmpty(query.AppInstanceID))
                    predicate = Predicates.Field<AppFlowEntity>(f => f.AppInstanceID, DapperExtensions.Operator.Eq, query.AppInstanceID);

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
