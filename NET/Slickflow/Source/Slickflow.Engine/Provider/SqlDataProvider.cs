using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.Data;

namespace Slickflow.Engine.Provider
{
    /// <summary>
    /// 不同类型数据库的SQL语句
    /// </summary>
    public class SqlDataProvider
    {
        /// <summary>
        /// 任务分页SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>SQL语句</returns>
        public static string GetSqlTaskPaged(string sql)
        {
            if (DBTypeExtenstions.WfDataProvider != null)
                return DBTypeExtenstions.WfDataProvider.GetSqlTaskPaged(sql);
            else
                return sql;
        }

        /// <summary>
        /// 根据活动实例获取个人任务SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>SQL</returns>
        public static string GetSqlTaskOfMineByAtcitivityInstance(string sql)
        {
            if (DBTypeExtenstions.WfDataProvider != null)
                return DBTypeExtenstions.WfDataProvider.GetSqlTaskOfMineByAtcitivityInstance(sql);
            else
                return sql;
        }

        /// <summary>
        /// 根据应用实例获取个人任务SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>SQL</returns>
        public static string GetSqlTaskOfMineByAppInstance(string sql)
        {
            if (DBTypeExtenstions.WfDataProvider != null)
                return DBTypeExtenstions.WfDataProvider.GetSqlTaskOfMineByAppInstance(sql);
            else
                return sql;
        }
    }
}
