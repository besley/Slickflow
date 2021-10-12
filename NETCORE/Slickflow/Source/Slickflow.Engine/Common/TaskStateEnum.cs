using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 任务状态类型
    /// </summary>
    public enum TaskStateEnum
    {
        /// <summary>
        /// 等待办理
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// 办理状态
        /// </summary>
        Reading = 2,

        /// <summary>
        /// 正常完成
        /// </summary>
        Completed = 4,

        /// <summary>
        /// 撤销
        /// </summary>
        Withdrawed = 8,

        /// <summary>
        /// 退回
        /// </summary>
        SendBacked = 9,

        /// <summary>
        /// 多人可以办理，当别人办理完后，置关闭状态
        /// </summary>
        Closed = 32,

        /// <summary>
        /// 没有办理，直接取消
        /// </summary>
        Canceled = 48
    }
}
