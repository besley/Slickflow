using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 跳转选项
    /// </summary>
    public enum JumpOptionEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 流程发起位置
        /// </summary>
        Startup = 1,

        /// <summary>
        /// 流程结束位置
        /// </summary>
        End = 2
    }
}
