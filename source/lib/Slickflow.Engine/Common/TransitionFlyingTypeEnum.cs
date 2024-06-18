using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    public enum TransitionFlyingTypeEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        NotFlying = 0,

        /// <summary>
        /// 向前（跳转）
        /// </summary>
        ForwardFlying = 1,

        /// <summary>
        /// 向后（跳转）
        /// </summary>
        BackwardFlying = 2
    }
}
