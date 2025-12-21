using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Xpdl.Common
{
    /// <summary>
    /// Process template type
    /// 流程模板类型
    /// </summary>
    public enum ProcessTemplateType
    {
        /// <summary>
        /// Blank
        /// </summary>
        Blank = 0,

        /// <summary>
        /// Default
        /// </summary>
        Default = 1,

        /// <summary>
        /// Simple
        /// </summary>
        STD_Simple = 2,

        /// <summary>
        /// Sequence
        /// </summary>
        Sequence = 3,

        /// <summary>
        /// Parallel Gateway
        /// </summary>
        Parallel = 4,

        /// <summary>
        /// Multiple Instance
        /// </summary>
        MultipleInstance = 5,

        /// <summary>
        /// And Split Multiple Instance Container
        /// </summary>
        AndSplitMI = 6,

        /// <summary>
        /// Sub process
        /// </summary>
        SubProcess = 7,

        /// <summary>
        /// Complex pattern
        /// </summary>
        Complex = 11,

        /// <summary>
        /// Conditional
        /// </summary>
        Conditional = 13,

        /// <summary>
        /// Process Modify
        /// </summary>
        ProcessModify = 15,

        /// <summary>
        /// Run Process
        /// </summary>
        RunProcess = 17,
    }
}
