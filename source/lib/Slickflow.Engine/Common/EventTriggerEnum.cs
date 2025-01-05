using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Event Trigger Type
    /// 事件触发类型
    /// </summary>
    public enum EventTriggerEnum : int
    {
        /// <summary>
        /// None
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Timer
        /// 定时
        /// </summary>
        Timer = 1,

        /// <summary>
        /// Message
        /// 消息
        /// </summary>
        Message = 2,

        /// <summary>
        /// Error
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// Escalation
        /// 升级
        /// </summary>
        Escalation = 4,

        /// <summary>
        /// Cancel
        /// 取消
        /// </summary>
        Cancel = 5,

        /// <summary>
        /// Compensation
        /// 补偿
        /// </summary>
        Compensation = 6,

        /// <summary>
        /// Conditional
        /// 条件
        /// </summary>
        Conditional = 7,

        /// <summary>
        /// Link
        /// 链接
        /// </summary>
        Link = 8,

        /// <summary>
        /// Signal
        /// 信号
        /// </summary>
        Signal = 9,

        /// <summary>
        /// Terminate
        /// 终结
        /// </summary>
        Terminate = 10
    }
}
