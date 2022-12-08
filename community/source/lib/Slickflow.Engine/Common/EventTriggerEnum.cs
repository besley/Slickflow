using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Event Trigger Type
    /// </summary>
    public enum EventTriggerEnum : int
    {
        /// <summary>
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// 定时
        /// </summary>
        Timer = 1,

        /// <summary>
        /// 消息
        /// </summary>
        Message = 2,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 3,

        /// <summary>
        /// 升级
        /// </summary>
        Escalation = 4,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 5,

        /// <summary>
        /// 补偿
        /// </summary>
        Compensation = 6,

        /// <summary>
        /// 条件
        /// </summary>
        Conditional = 7,

        /// <summary>
        /// 链接
        /// </summary>
        Link = 8,

        /// <summary>
        /// 信号
        /// </summary>
        Signal = 9,

        /// <summary>
        /// 终结
        /// </summary>
        Terminate = 10
    }
}
