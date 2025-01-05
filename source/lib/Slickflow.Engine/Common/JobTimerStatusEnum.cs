using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Job Timer Status
    /// 作业定时器状态
    /// </summary>
    public enum JobTimerStatusEnum
    {
        /// <summary>
        /// None
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// Ready
        /// 预备
        /// </summary>
        Ready = 1,

        /// <summary>
        /// Completed
        /// 关闭
        /// </summary>
        Completed = 4
    }
}
