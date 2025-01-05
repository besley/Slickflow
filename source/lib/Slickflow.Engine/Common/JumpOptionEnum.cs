﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Jump Options 
    /// 跳转选项
    /// </summary>
    public enum JumpOptionEnum
    {
        /// <summary>
        /// Default
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// Process initiation location
        /// 流程发起位置
        /// </summary>
        Startup = 1,

        /// <summary>
        /// End position of process
        /// 流程结束位置
        /// </summary>
        End = 2
    }
}
