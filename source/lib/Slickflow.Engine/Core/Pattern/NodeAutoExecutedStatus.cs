using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 网关执行结果状态类
    /// </summary>
    internal enum NodeAutoExecutedStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 执行成功
        /// </summary>
        Successed = 1,

        /// <summary>
        /// 发生错误
        /// </summary>
        Failed = 2,

        /// <summary>
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 10,

        /// <summary>
        /// 后进的节点，不参与XOrJoin节点的运行
        /// </summary>
        FallBehindOfXOrJoin = 11,

        /// <summary>
        /// 不是强制分支(EOrJoin)
        /// </summary>
        NotForcedBrancheWhenEOrJoin = 13
    }
}
