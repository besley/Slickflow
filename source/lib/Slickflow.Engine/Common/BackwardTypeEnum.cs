using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 节点退回枚举类型
    /// </summary>
    public enum BackwardTypeEnum
    {
        /// <summary>
        /// 撤销
        /// </summary>
        Withdrawed = 1,

        /// <summary>
        /// 退回
        /// </summary>
        Sendback = 2,

        /// <summary>
        /// 返签
        /// </summary>
        Reversed = 3
    }
}
