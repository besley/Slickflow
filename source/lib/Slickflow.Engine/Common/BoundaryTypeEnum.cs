using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Event Boundary Type
    /// 边界类型
    /// </summary>
    public enum BoundaryTypeEnum : int
    {
        /// <summary>
        /// None
        /// 缺省
        /// </summary>
        None = 0,

        /// <summary>
        /// Boundary
        /// 边界
        /// </summary>
        Boundary = 10,

        /// <summary>
        /// Throw
        /// 抛出
        /// </summary>
        Throw = 20,

        /// <summary>
        /// Catch
        /// 捕获
        /// </summary>
        Catch = 21
    }
}
