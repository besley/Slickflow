using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Business.Result
{
    /// <summary>
    /// Status execution enumeration type
    /// 状态执行枚举类型
    /// </summary>
    public enum WfDataManagedStatusEnum
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
