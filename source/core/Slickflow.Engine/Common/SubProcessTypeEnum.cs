using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Sub Process Type
    /// 子流程类型
    /// </summary>
    public enum SubProcessTypeEnum
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Nested
        /// 嵌入式类型
        /// </summary>
        Nested = 1,

        /// <summary>
        /// Referenced
        /// 外部引用类型
        /// </summary>
        Referenced = 2
    }
}
