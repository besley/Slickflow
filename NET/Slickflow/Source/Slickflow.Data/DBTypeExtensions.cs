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

        SQLSERVER = 1,

        ORACLE = 2,

        MYSQL = 3
    }

    /// <summary>
    /// 数据库类型的标识类
    /// </summary>
    public static class DBTypeExtenstions
    {
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

        /// <summary>
        /// 设置数据库类型变量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        public static void SetDBType(DBTypeEnum type, IWfDataProvider provider)
        {
            dbType = type;
            wfDataProvider = provider;
        }
    }
}
