using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 连线并行时的类型：必需、可选
    /// </summary>
    public enum ParallelOptionEnum
    {
        /// <summary>
        /// 可选
        /// </summary>
        Optional = 0,

        /// <summary>
        /// 必需
        /// </summary>
        Necessary = 1
    }
}
