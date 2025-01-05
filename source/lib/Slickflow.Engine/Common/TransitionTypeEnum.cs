using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Transition Type
    /// 转移类型
    /// </summary>
    public enum TransitionTypeEnum
    {
        /// <summary>
        /// Forward
        /// 前行
        /// </summary>
        Forward = 1,

        /// <summary>
        /// Withdrawed
        /// 撤销
        /// </summary>
        Withdrawed = 2,

        /// <summary>
        /// Sendback
        /// 退回
        /// </summary>
        Sendback = 4,

        /// <summary>
        /// Reversed
        /// 返签
        /// </summary>
        Reversed = 8,

        /// <summary>
        /// Backward
        /// including reverse, sendback, and withdrawed types)
        /// 后退
        /// </summary>
        Backward = 14,

        /// <summary>
        /// Loop
        /// 自身循环
        /// </summary>
        Loop = 16
    }
}
