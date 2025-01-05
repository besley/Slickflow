using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Transition Flying Type
    /// </summary>
    public enum TransitionFlyingTypeEnum
    {
        /// <summary>
        /// Not Flying
        /// 默认
        /// </summary>
        NotFlying = 0,

        /// <summary>
        /// Forward Flying
        /// 向前（跳转）
        /// </summary>
        ForwardFlying = 1,

        /// <summary>
        /// Backward Flying
        /// 向后（跳转）
        /// </summary>
        BackwardFlying = 2
    }
}
