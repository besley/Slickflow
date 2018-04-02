using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Oracle.ManagedDataAccess.Client;

namespace Slickflow.Data
{
    /// <summary>
    /// Session 创建类
    /// </summary>
    public static class SessionFactory
    {
        /// <summary>
        /// 创建类的构造方法
        /// </summary>
        static SessionFactory()
        {
            InitializeDBType(DBTypeEnum.SQLSERVER);     //多数据库枚举类型，ORACLE, MYSQL等
        }

        /// <summary>
        /// 设置数据库类型相关的变量
        /// </summary>
        /// <param name="type">数据库类型</param>
        private static void InitializeDBType(DBTypeEnum type)
        {
            //指定某个具体的数据库类型，仅第一次赋值，其后不用赋值。
            if (type == DBTypeEnum.SQLSERVER)
            {
                DBTypeExtenstions.SetDBType(type, null);        //默认实现为SQLSERVER
            }
            else if (type == DBTypeEnum.ORACLE)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleSqlDialect();
                //DBTypeExtenstions.SetDBType(type, new OracleWfDataProvider());
            }
            else if (type == DBTypeEnum.MYSQL)
            {
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
                //DBTypeExtenstions.SetDBType(type, new MySqlWfDataProvider());
            }
        }

        /// <summary>
        /// 根据DBType类型，创建数据库连接
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateConnectionByDBType()
        {
            IDbConnection conn = null;
            var connStringSetting = ConfigurationManager.ConnectionStrings["WfDBConnectionString"];
            if (DBTypeExtenstions.DBType == DBTypeEnum.SQLSERVER)
            {
                conn = new SqlConnection(connStringSetting.ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.ORACLE)
            {
                //conn = new OracleConnection(connStringSetting.ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.MYSQL)
            {
                //conn = new MySqlConnection(connStringSetting.ConnectionString);
            }
            return conn;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = CreateConnectionByDBType();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        /// <summary>
        /// 创建数据库连接会话
        /// </summary>
        /// <returns></returns>
        public static IDbSession CreateSession()
        {
            IDbConnection conn = CreateConnection();
            IDbSession session = new DbSession(conn);

            return session;
        }

        /// <summary>
        /// 创建数据库事务会话
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static IDbSession CreateSession(IDbConnection conn, IDbTransaction trans)
        {
            IDbSession session = new DbSession(conn, trans);
            return session;
        }
    }
}
