using System;
using System.Collections.Generic;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 图形类型
    /// </summary>
    public enum ProcessTemplateType
    {
        /// <summary>
        /// 空白
        /// </summary>
        Blank = 0,

        /// <summary>
        /// 简单流程
        /// </summary>
        Simple = 1,

        /// <summary>
        /// 序列
        /// </summary>
        Sequence = 2,

        /// <summary>
        /// 并行网关
        /// </summary>
        Parallel = 3,

        /// <summary>
        /// 会签
        /// </summary>
        MultipleInstance = 4,

        /// <summary>
        /// 并行分支容器
        /// </summary>
        AndSplitMI = 5,

        /// <summary>
        /// 子流程
        /// </summary>
        SubProcess = 6,

        /// <summary>
        /// 复杂模式
        /// </summary>
        Complex = 21,

        /// <summary>
        /// 批处理更新
        /// </summary>
        BatchUpdate = 30,

        /// <summary>
        /// Startup Process
        /// </summary>
        RunProcess = 40,
    }
}
