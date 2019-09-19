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
            IWfDataProvider dataProvider = DataProviderFactory.GetProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskPaged(sql);
            }
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
            IWfDataProvider dataProvider = DataProviderFactory.GetProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskOfMineByAtcitivityInstance(sql);
            }
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
            IWfDataProvider dataProvider = DataProviderFactory.GetProvider();
            if (dataProvider != null)
            {
                return dataProvider.GetSqlTaskOfMineByAppInstance(sql);
            }
            else
                return sql;
        }
    }
}
