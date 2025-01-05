using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// Signal Consumed Status
    /// 状态执行枚举类型
    /// </summary>
    public enum SignalConsumedStatus
    {
        /// <summary>
        /// Default
        /// </summary>
        Default = 0,

        /// <summary>
        /// Success
        /// </summary>
        Success = 1,

        /// <summary>
        /// Failed
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Exception
        /// </summary>
        Exception = 3
    }
}
