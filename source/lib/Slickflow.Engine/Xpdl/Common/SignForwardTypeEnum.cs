using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 加签类型
    /// </summary>
    public enum SignForwardTypeEnum
    {
        /// <summary>
        /// 不加签
        /// </summary>
        None = 0,

        /// <summary>
        /// 前加签
        /// </summary>
        SignForwardBefore = 1,

        /// <summary>
        /// 后加签
        /// </summary>
        SignForwardBehind = 2,

        /// <summary>
        /// 并行加签
        /// </summary>
        SignForwardParallel = 3
    }
}
