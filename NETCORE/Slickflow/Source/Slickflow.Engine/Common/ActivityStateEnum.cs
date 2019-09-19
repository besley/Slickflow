using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 节点运行状态
    /// </summary>
    public enum ActivityStateEnum
    {
        /// <summary>
        /// 准备状态
        /// </summary>
        Ready = 1,

        /// <summary>
        /// 运行状态
        /// </summary>
        Running = 2,

        /// <summary>
        /// 完成状态
        /// </summary>
        Completed = 4,

        /// <summary>
        /// 挂起
        /// </summary>
        Suspended = 5,
        
        /// <summary>
        /// 撤销状态
        /// </summary>
        Withdrawed = 6,

        /// <summary>
        /// 退回状态
        /// </summary>
        Sendbacked = 7,

        /// <summary>
        /// 系统内部标志取消
        /// </summary>
        Cancelled=8,

        /// <summary>
        /// 非正常结束（比如超时自动终结）
        /// </summary>
        Terminated = 9
    }
}
