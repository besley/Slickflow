using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Slickflow.Data;
using Slickflow.Engine.Common;
using SfDemo.BizEntity;
using SfDemo.Interface;

namespace SfDemo.ServiceImp
{
    /// <summary>
    /// 流转记录器
    /// </summary>
    public class AppFlowService : ServiceBase, IAppFlowService
    {
        /// <summary>
        /// 流程业务记录分页方法
        /// </summary>
        /// <param name="query"></param>
        /// <param name="count"></param>
        /// <returns></returns>
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
                    predicate = Predicates.Field<AppFlowEntity>(f => f.AppInstanceID, Operator.Eq, query.AppInstanceID);

                count = DBHelper.Count<AppFlowEntity>(conn, predicate);
                list = DBHelper.GetPaged<AppFlowEntity>(conn, query.PageIndex, query.PageSize,
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
