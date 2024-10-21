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
        /// 邮件
        /// </summary>
        EMail = 1,

        /// <summary>
        /// 定时器
        /// </summary>
        Timer = 2,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 3,

        /// <summary>
        /// 信号
        /// </summary>
        Signal = 4,

        /// <summary>
        /// 条件
        /// </summary>
        Conditional = 5
    }
}
