using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 子流程类型
    /// </summary>
    public enum SubProcessTypeEnum
    {
        /// <summary>
        /// 未指定类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 嵌入式类型
        /// </summary>
        Nested = 1,

        /// <summary>
        /// 外部引用类型
        /// </summary>
        Referenced = 2
    }
}
