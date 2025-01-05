using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process State
    /// 流程状态
    /// </summary>
    public enum ProcessStateEnum
    {
        /// <summary>
        /// Not Startup
        /// 未启动，流程记录为空
        /// </summary>
        NotStart = 0,

        /// <summary>
        /// Ready
        /// 准备状态
        /// </summary>
        Ready = 1,

        /// <summary>
        /// Running
        /// 运行状态
        /// </summary>
        Running = 2,

        /// <summary>
        /// Completed
        /// 完成
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Suspended
        /// 挂起
        /// </summary>
        Suspended = 5,

        /// <summary>
        /// Canceled
        /// 取消
        /// </summary>
        Canceled = 6,

        /// <summary>
        /// Discarded
        /// 废弃
        /// </summary>
        Discarded = 7,

        /// <summary>
        /// Terminated
        /// 自然终止（比如超期）
        /// </summary>
        Terminated = 8
    }
}
