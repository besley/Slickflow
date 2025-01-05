using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Message Direction Type
    /// 消息接收类型
    /// </summary>
    public enum MessageDirectionEnum : int
    {
        /// <summary>
        /// Default
        /// 默认
        /// </summary>
        None = 0,

        /// <summary>
        /// Catch
        /// 捕获消息
        /// </summary>
        Catch = 1,

        /// <summary>
        /// Throw
        /// 抛出消息
        /// </summary>
        Throw = 2
    }
}
