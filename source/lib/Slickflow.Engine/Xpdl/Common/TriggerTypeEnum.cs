using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 触发器类型
    /// </summary>
    public enum TriggerTypeEnum
    {
        /// <summary>
        /// 默认是没有定时器
        /// </summary>
        None = 0,

        /// <summary>
        /// 定时器
        /// </summary>
        Timer = 1,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 2,

        /// <summary>
        /// 条件
        /// </summary>
        Conditional = 3
    }
}
