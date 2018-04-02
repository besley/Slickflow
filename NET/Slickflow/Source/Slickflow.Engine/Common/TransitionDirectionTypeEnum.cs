using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    public enum TransitionDirectionTypeEnum
    {
        /// <summary>
        /// 前进方向
        /// </summary>
        Forward = 1,

        /// <summary>
        /// 可双向
        /// </summary>
        Bidirection = 2,

        /// <summary>
        /// 子循环
        /// </summary>
        Loop = 4
    }
}
