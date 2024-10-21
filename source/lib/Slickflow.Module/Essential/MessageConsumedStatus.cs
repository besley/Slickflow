using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Module.Essential
{
    /// <summary>
    /// 状态执行枚举类型
    /// </summary>
    public enum MessageConsumedStatus
    {
        /// <summary>
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// 成功状态
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行失败状态
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 异常状态
        /// </summary>
        Exception = 3
    }
}
