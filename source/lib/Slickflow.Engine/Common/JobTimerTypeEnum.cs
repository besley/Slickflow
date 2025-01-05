using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Job Timer Type
    /// 作业定时器类型
    /// </summary>
    public enum JobTimerTypeEnum
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
        /// Conditional
        /// 条件
        /// </summary>
        Conditional = 2,

        /// <summary>
        /// EMail
        /// 邮件
        /// </summary>
        EMail = 3,

        /// <summary>
        /// Message
        /// 消息
        /// </summary>
        Message = 4
    }
}
