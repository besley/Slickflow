using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 操作方法类型
    /// </summary>
    public enum ScriptMethodEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// SQL 语句
        /// </summary>
        SQL = 1,

        /// <summary>
        /// 执行脚本
        /// </summary>
        JavaScript = 2,

        /// <summary>
        /// Python 脚本
        /// </summary>
        Python = 3
    }
}
