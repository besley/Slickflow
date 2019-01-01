using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Data
{
    /// <summary>
    /// 数据库类型标识
    /// </summary>
    public enum DBTypeEnum
    {
        NONE = 0,

        /// <summary>
        /// Mcirosoft SQLSERVER
        /// </summary>
        SQLSERVER = 1,

        /// <summary>
        /// ORACLE
        /// </summary>
        ORACLE = 2,

        /// <summary>
        /// MYSQL
        /// </summary>
        MYSQL = 3,

        /// <summary>
        /// KINGBASE
        /// </summary>
        KINGBASE = 4
    }

    /// <summary>
    /// 数据库类型的标识类
    /// </summary>
    public static class DBTypeExtenstions
    {
        #region 属性
        private static DBTypeEnum dbType = DBTypeEnum.NONE;
        public static DBTypeEnum DBType 
        { 
            get 
            { 
                return dbType; 
            } 
        }

        private static IWfDataProvider wfDataProvider = null;
        public static IWfDataProvider WfDataProvider { get { return wfDataProvider; } }
        #endregion

        /// <summary>
        /// 设置数据库类型变量
        /// </summary>
        /// <param name="type">数据库类型</param>
        /// <param name="provider">数据提供者</param>
        public static void SetDBType(DBTypeEnum type, IWfDataProvider provider = null)
        {
            dbType = type;
            wfDataProvider = provider;

            if (type == DBTypeEnum.SQLSERVER)
            {
                //默认实现为SQLSERVER
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();     
            }
            else if (type == DBTypeEnum.ORACLE)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleSqlDialect();
            }
            else if (type == DBTypeEnum.MYSQL)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
            }
            else if (type == DBTypeEnum.KINGBASE)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.KingbaseSqlDialect();
            }
        }
    }
}
