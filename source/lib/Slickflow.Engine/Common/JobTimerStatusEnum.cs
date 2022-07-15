using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 作业定时器状态
    /// </summary>
    public enum JobTimerStatusEnum
    {
        /// <summary>
        /// 空
        /// </summary>
        None = 0,
        /// <summary>
        /// 预备
        /// </summary>
        Ready = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        Completed = 4
    }
}
