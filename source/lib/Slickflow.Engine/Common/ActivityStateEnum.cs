using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Activity State
    /// 节点运行状态
    /// </summary>
    public enum ActivityStateEnum
    {
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
        /// 完成状态
        /// </summary>
        Completed = 4,

        /// <summary>
        /// Suspended
        /// 挂起
        /// </summary>
        Suspended = 5,
        
        /// <summary>
        /// Withdrawed
        /// 撤销状态
        /// </summary>
        Withdrawed = 6,

        /// <summary>
        /// Sendbacked
        /// 退回状态
        /// </summary>
        Sendbacked = 7,

        /// <summary>
        /// Cancelled
        /// 系统内部标志取消
        /// </summary>
        Cancelled=8,

        /// <summary>
        /// Terminated
        /// 非正常结束（比如超时自动终结）
        /// </summary>
        Terminated = 9
    }
}
