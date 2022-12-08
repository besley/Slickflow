using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 操作方法类型
    /// </summary>
    public enum ServiceMethodEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 外部插件方法
        /// </summary>
        WebApi = 1,

        /// <summary>
        /// 本地服务程序
        /// </summary>
        LocalService = 2,

        /// <summary>
        /// C# 代码
        /// </summary>
        CSharpLibrary = 3,

        /// <summary>
        /// 存储过程
        /// </summary>
        StoreProcedure = 4
    }
}
