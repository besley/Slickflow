using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 操作方法类型
    /// </summary>
    public enum ActionMethodEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 本地服务程序
        /// </summary>
        LocalService = 1,

        /// <summary>
        /// C# 代码
        /// </summary>
        CSharpLibrary = 2,

        /// <summary>
        /// 外部插件方法
        /// </summary>
        WebApi = 3,

        /// <summary>
        /// SQL 语句
        /// </summary>
        SQL = 5,

        /// <summary>
        /// 存储过程
        /// </summary>
        StoreProcedure = 7,

        /// <summary>
        /// 执行脚本
        /// </summary>
        Script = 9,

        /// <summary>
        /// Python 脚本
        /// </summary>
        Python = 11,

        /// <summary>
        /// WebAPI 服务
        /// </summary>
        PlugIn = 13
    }

    /// <summary>
    /// 子方法類型
    /// </summary>
    public enum SubMethodEnum
    {
        /// <summary>
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Get方法
        /// </summary>
        HttpGet = 1,

        /// <summary>
        /// Post方法
        /// </summary>
        HttpPost = 2,

        /// <summary>
        /// Put方法
        /// </summary>
        HttpPut = 3,

        /// <summary>
        /// Delete方法
        /// </summary>
        HttpDelete = 4
    }
}
