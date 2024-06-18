using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// 事件发生类型
    /// </summary>
    public enum FireTypeEnum
    {
        /// <summary>
        /// 空类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 执行前
        /// </summary>
        Before = 1,

        /// <summary>
        /// 执行后
        /// </summary>
        After = 2
    }
}
