using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 消息接收类型
    /// </summary>
    public enum MessageDirectionEnum : int
    {
        /// <summary>
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// 捕获消息
        /// </summary>
        Catch = 1,

        /// <summary>
        /// 抛出消息
        /// </summary>
        Throw = 2
    }
}
