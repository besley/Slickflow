using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Transition Direction Type
    /// 转移方向类型
    /// </summary>
    public enum TransitionDirectionTypeEnum
    {
        /// <summary>
        /// Foward
        /// 前进方向
        /// </summary>
        Forward = 1,

        /// <summary>
        /// Bidirection
        /// 可双向
        /// </summary>
        Bidirection = 2,

        /// <summary>
        /// Loop
        /// 子循环
        /// </summary>
        Loop = 4
    }
}
