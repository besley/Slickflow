using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Service Method Type
    /// </summary>
    public enum ServiceMethodEnum
    {
        None = 0,

        WebApi = 1,

        LocalService = 2,

        CSharpLibrary = 3,

        StoreProcedure = 4,

        /// <summary>
        /// Local method resolved from registry by key (BPMN2 ##DelegateExpression semantic).
        /// 从委托注册表按 key 解析并调用的本地方法。
        /// </summary>
        LocalMethod = 5
    }
}
