using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process Startup Type
    /// 流程启动类型
    /// </summary>
    public enum ProcessStartTypeEnum
    {
        /// <summary>
        /// None
        /// 默认空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Timer
        /// 定时启动
        /// </summary>
        Timer = 1,

        /// <summary>
        /// Message
        /// 消息启动
        /// </summary>
        Message = 2
    }

    /// <summary>
    /// Process End Type
    /// 流程结束类型
    /// </summary>
    public enum ProcessEndTypeEnum
    {
        /// <summary>
        /// None
        /// 默认空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Timer
        /// 定时结束
        /// </summary>
        Timer = 1,

        /// <summary>
        /// Message
        /// 消息结束
        /// </summary>
        Message = 2
    }
}
