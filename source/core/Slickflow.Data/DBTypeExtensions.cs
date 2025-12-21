using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
//using Oracle.ManagedDataAccess.Client;
//using MySql.Data.MySqlClient;
using Npgsql;
//using MongoDB;

namespace Slickflow.Data
{
    /// <summary>
    /// Database type extension class
    /// 数据库类型扩展类
    /// </summary>
    public static class DBTypeExtenstions
    {
        #region Property and Constructor 属性和构造方法 
        private static DatabaseTypeEnum _dbType = DatabaseTypeEnum.NONE;
        private static DatabaseTypeEnum DBType { get { return _dbType; } }

        private static string _connectionString = string.Empty;
        private static string ConnectionString {  get { return _connectionString; } }

        /// <summary>
        /// Static construction method
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
            //Set database type
            SetYourDataBaseType();
        }
        #endregion

        #region Custom database type 定义数据库类型 
        /// <summary>
        /// Users select a database or connection type for initialization
        /// 选择自己的数据库类型
        /// </summary>
        private static void SetYourDataBaseType()
        {
            //备注：用户需要在此进行数据库类型的选择
            //根据不同的数据库类型创建
            //Note: Users need to select the database type here
            //Create according to different database types
            //SetDBType(DBTypeEnum.SQLSERVER);        //Default is SQL SERVER

            //如果是Oracle数据库，请使用Oracle类型参数
            //If it is an Oracle database, please use Oracle type parameters
            //SetDBType(DBTypeEnum.ORACLE);

            //SetDBType(DBTypeEnum.MYSQL);

            SetDBType(DatabaseTypeEnum.PGSQL);

            //SetDBType(DBTypeEnum.MONGODB);

            //设置蛇形命名规则，实现自动转换
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        /// <summary>
        /// Create a database connection based on the DBType type
        /// Note: Users need to perform initialization operations on database connection objects here
        /// Prerequisite: It is necessary to first reference the DLL file of the access component that selects the database
        /// 
        /// 根据DBType类型，创建数据库连接
        /// 备注：用户需要在此进行数据库连接对象的初始化操作
        /// 前提：需要先引用选择数据库的访问组件dll文件
        /// </summary>
        /// <returns></returns>
        internal static IDbConnection CreateConnectionByDBType()
        {
            IDbConnection conn = null;
            if (DBTypeExtenstions.DBType == DatabaseTypeEnum.MYSQL)
            {
                //conn = new MySqlConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DatabaseTypeEnum.SQLSERVER)
            {
                //conn = new SqlConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DatabaseTypeEnum.ORACLE)
            {
                //conn = new OracleConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DatabaseTypeEnum.KINGBASE)
            {
                //conn = new KingbaseConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DatabaseTypeEnum.PGSQL)
            {
                conn = new NpgsqlConnection(ConnectionString);
            }
            else if (DBTypeExtenstions.DBType == DatabaseTypeEnum.MONGODB)
            {
                //conn = new MongoDB.Driver.MongoClient(ConnectionString);
            }
            return conn;
        }
        #endregion

        #region Internal setting method for database type 数据库类型内部设置方法
        /// <summary>
        /// Internal setting method for database type
        /// 获取数据类型
        /// </summary>
        /// <returns>Database Type</returns>
        public static DatabaseTypeEnum GetDbType()
        {
            return _dbType;
        }

        /// <summary>
        /// Set Database Type
        /// 设置数据库类型
        /// </summary>
        /// <param name="type">Database Type</param>
        public static void SetDBType(DatabaseTypeEnum type)
        {
            // 设置数据库类型  
            // Set Database Type
            _dbType = type;

            // 设置数据库Dialect
            // Set Database Dialect
            if (type == DatabaseTypeEnum.SQLSERVER)
            {
                //默认实现为SQLSERVER
                //Default is SQLSERVER
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqlServerDialect();
            }
            else if (type == DatabaseTypeEnum.ORACLE)
            {
                //Oracle 数据库
                //Oracle Database
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.OracleDialect();
            }
            else if (type == DatabaseTypeEnum.MYSQL)
            {
                //MySQL 数据库
                //MySQL Database
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();
            }
            else if (type == DatabaseTypeEnum.PGSQL)
            {
                //PgSQL 数据库
                //PgSQL Database

                //Npgsql will have utc time format issue
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.PostgreSqlDialect();
            }
            else if (type == DatabaseTypeEnum.KINGBASE)
            {
                //KingBase 数据库
                //KingBase Database
                DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.KingbaseSqlDialect();
            }
        }
        #endregion

        #region Provide to Initialize database types for NET applications 提供给.NET应用程序初始化数据库类型 
        /// <summary>
        /// Initialize database connection string
        /// 初始化连接串
        /// </summary>
        /// <param name="databaseType">Database Type</param>
        /// <param name="strConn">Connection String</param>
        public static void InitConnectionString(string databaseType, string strConn)
        {
            DatabaseTypeEnum dbType = (DatabaseTypeEnum)Enum.Parse(typeof(DatabaseTypeEnum), databaseType.ToUpper());
            InitConnectionStringInt(dbType, strConn);
        }

        /// <summary>
        /// Initialize database connection string
        /// 初始化连接串
        /// </summary>
        /// <param name="databaseType">Database Type</param>
        /// <param name="strConn">Connection String</param>
        private static void InitConnectionStringInt(DatabaseTypeEnum databaseType, string strConn)
        {
            _dbType = databaseType;
            _connectionString = strConn;
        }
        #endregion
    }
}
