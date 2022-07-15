using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Data.DataProvider
{
    /// <summary>
    /// 数据服务工厂类
    /// </summary>
    public class SqlDataProviderFactory
    {
        /// <summary>
        /// 根据数据库类型获取DataProvider
        /// </summary>
        /// <returns></returns>
        private static ISqlDataProvider GetSqlDataProvider()
        {
            ISqlDataProvider dataProvider = null;
            var dbType = DBTypeExtenstions.GetDbType();
            if (dbType == DBTypeEnum.ORACLE)
            {
                dataProvider = new OracleDataProvider();
            }
            else if(dbType == DBTypeEnum.MYSQL)
            {
                dataProvider = new MySqlDataProvider();
            }
            return dataProvider;
        }
        /// <summary>
        /// 任务分页SQL语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>SQL语句</returns>
        public static string GetSqlTaskPaged(string sql)
        {
            var dataProvider = GetSqlDataProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskPaged(sql);
            }
            else
            {
                return sql;
            }
        }

        /// <summary>
        /// 根据活动实例获取个人任务SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>SQL</returns>
        public static string GetSqlTaskOfMineByAtcitivityInstance(string sql)
        {
            var dataProvider = GetSqlDataProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskOfMineByAtcitivityInstance(sql);
            }
            else
            {
                return sql;
            }
        }

        /// <summary>
        /// 根据应用实例获取个人任务SQL
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>SQL</returns>
        public static string GetSqlTaskOfMineByAppInstance(string sql)
        {
            var dataProvider = GetSqlDataProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskOfMineByAppInstance(sql);
            }
            else
            {
                return sql;
            }
        }
    }
}
