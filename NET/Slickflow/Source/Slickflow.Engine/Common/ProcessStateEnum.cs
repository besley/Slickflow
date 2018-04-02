using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    public enum ProcessStateEnum
    {
        /// <summary>
        /// 未启动，流程记录为空
        /// </summary>
        NotStart = 0,

        /// <summary>
        /// 准备状态
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 运行状态
        /// </summary>
        Running = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Completed = 4,

        /// <summary>
        /// 挂起
        /// </summary>
        Suspended = 5,

        /// <summary>
        /// 取消
        /// </summary>
        Canceled = 6,

        /// <summary>
        /// 终止
        /// </summary>
        Discarded = 7
    }
}
