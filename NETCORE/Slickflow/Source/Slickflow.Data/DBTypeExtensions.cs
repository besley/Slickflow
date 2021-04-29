using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using Oracle.ManagedDataAccess.Client;
//using MySql.Data.MySqlClient;
//using Npgsql;

namespace Slickflow.Data
{
    #region 数据库类型
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
        /// Postgrel
        /// </summary>
        PGSQL = 4,

        /// <summary>
        /// KINGBASE
        /// </summary>
        KINGBASE = 5
    }
    #endregion

    /// <summary>
    /// 数据库类型的标识类
    /// </summary>
    public static class DBTypeExtenstions
    {
        #region 属性和构造方法
        private static DBTypeEnum _dbType = DBTypeEnum.NONE;
        private static DBTypeEnum DBType { get { return _dbType; } }

        private static string _connectionString = string.Empty;
        private static string ConnectionString {  get { return _connectionString; } }

        private static IWfDataProvider _wfDataProvider = null;
        public static IWfDataProvider WfDataProvider { get { return _wfDataProvider; } }

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static DBTypeExtenstions()
        {
            var connectionSetting = ConfigurationManager.ConnectionStrings["WfDBConnectionString"];
            if (connectionSetting != null)
            {
                _connectionString = connectionSetting.ConnectionString;
            }

            //设置数据库类型
            SetYourDataBaseType();
        }
        #endregion

        #region 自定义数据库类型：用户选择数据库或连接类型进行初始化
        /// <summary>
        /// 选择自己的数据库类型
        /// </summary>
        private static void SetYourDataBaseType()
        {
            //备注：用户需要在此进行数据库类型的选择

            //根据不同的数据库类型创建
            SetDBType(DBTypeEnum.SQLSERVER);        //默认SQL SERVER

            //如果是Oracle数据库，请使用Oracle类型参数
            //SetDBType(DBTypeEnum.ORACLE);

            //SetDBType(DBTypeEnum.MYSQL);

            //SetDBType(DBTypeEnum.PGSQL);
        }

        /// <summary>
        /// 根据DBType类型，创建数据库连接
        /// </summary>
        /// <returns></returns>
        internal static IDbConnection CreateConnectionByDBType()
        {
            //备注：用户需要在此进行数据库连接对象的初始化操作
            //前提：需要先引用选择数据库的访问组件dll文件

            IDbConnection conn = null;
            if (DBTypeExtenstions.DBType == DBTypeEnum.SQLSERVER)
            {
                conn = new SqlConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.ORACLE)
            {
                //conn = new OracleConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.MYSQL)
            {
                //conn = new MySqlConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.KINGBASE)
            {
                //conn = new KingbaseConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DBTypeEnum.PGSQL)
            {
                //conn = new NpgsqlConnection(ConnectionString);
            }
            return conn;
        }
        #endregion

        #region 数据库类型内部设置方法
        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <returns>数据类型</returns>
        public static DBTypeEnum GetDbType()
        {
            return _dbType;
        }
        /// <summary>
        /// 设置数据库类型
        /// </summary>
        /// <param name="type">数据库类型</param>
        public static void SetDBType(DBTypeEnum type)
        {
            // 设置数据库类型  
            _dbType = type;


            // 设置数据库Dialect
            if (type == DBTypeEnum.SQLSERVER)
            {
                //默认实现为SQLSERVER
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();
            }
            else if (type == DBTypeEnum.ORACLE)
            {
                //Oracle 数据库
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleDialect();
            }
            else if (type == DBTypeEnum.MYSQL)
            {
                //MySQL 数据库
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
            }
            else if (type == DBTypeEnum.PGSQL)
            {
                //PgSQL 数据库
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            }
            else if (type == DBTypeEnum.KINGBASE)
            {
                //KingBase 数据库
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.KingbaseSqlDialect();
            }
        }
        #endregion

        #region 提供给.NETCore程序初始化数据库类型
        /// <summary>
        /// 初始化连接串
        /// </summary>
        /// <param name="strConn">连接串</param>
        public static void InitConnectionString(string databaseType, string strConn)
        {
            DBTypeEnum dbType = (DBTypeEnum)Enum.Parse(typeof(DBTypeEnum), databaseType.ToUpper());
            InitConnectionStringInt(dbType, strConn);
        }

        /// <summary>
        /// 初始化连接串
        /// </summary>
        /// <param name="strConn">连接串</param>
        private static void InitConnectionStringInt(DBTypeEnum databaseType, string strConn)
        {
            _dbType = databaseType;
            _connectionString = strConn;
        }
        #endregion
    }
}
