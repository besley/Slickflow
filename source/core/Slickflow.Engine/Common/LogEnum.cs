using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Log Event Type
    /// 日志事件类型
    /// </summary>
    public enum LogEventType
    {
        /// <summary>
        /// Warning
        /// </summary>
        Warnning = 0,

        /// <summary>
        /// Exception
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Error
        /// </summary>
        Error = 2,
    }

    /// <summary>
    /// Log Priority
    /// 日志优先级
    /// </summary>
    public enum LogPriority
    {
        /// <summary>
        /// Emergency
        /// </summary>
        Emergency = 0,

        /// <summary>
        /// High
        /// </summary>
        High = 1,

        /// <summary>
        /// Normal
        /// </summary>
        Normal = 2,

        /// <summary>
        /// Low
        /// </summary>
        Low = 3
    }
}
