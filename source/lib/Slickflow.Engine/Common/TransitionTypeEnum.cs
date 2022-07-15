using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 转移类型
    /// </summary>
    public enum TransitionTypeEnum
    {
        /// <summary>
        /// 前行
        /// </summary>
        Forward = 1,

        /// <summary>
        /// 撤销
        /// </summary>
        Withdrawed = 2,

        /// <summary>
        /// 退回
        /// </summary>
        Sendback = 4,

        /// <summary>
        /// 返签
        /// </summary>
        Reversed = 8,

        /// <summary>
        /// 后退
        /// </summary>
        Backward = 14, //(包括撤销、退回和返签类型)

        /// <summary>
        /// 自身循环
        /// </summary>
        Loop = 16
    }
}
