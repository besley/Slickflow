using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 日志事件类型
    /// </summary>
    public enum LogEventType
    {
        Warnning = 0,
        Exception = 1,
        Error = 2,
    }

    /// <summary>
    /// 日志优先级
    /// </summary>
    public enum LogPriority
    {
        Emergency = 0,
        High = 1,
        Normal = 2,
        Low = 3
    }
}
