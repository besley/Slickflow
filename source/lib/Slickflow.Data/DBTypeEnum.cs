using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Data
{
    /// <summary>
    /// Database type identification
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
        KINGBASE = 5,

        /// <summary>
        /// MongoDB
        /// </summary>
        MONGODB = 21,
    }
}
