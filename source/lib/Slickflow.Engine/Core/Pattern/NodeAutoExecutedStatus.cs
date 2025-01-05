using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// Node Auto Executed Status
    /// 网关执行结果状态类
    /// </summary>
    internal enum NodeAutoExecutedStatus
    {
        /// <summary>
        /// Unknown
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Successed
        /// 执行成功
        /// </summary>
        Successed = 1,

        /// <summary>
        /// Failed
        /// 发生错误
        /// </summary>
        Failed = 2,

        /// <summary>
        /// Waiting for other branches that need to be merged
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 10,

        /// <summary>
        /// Behind nodes do not participate in the operation of XOrJoin nodes
        /// 后进的节点，不参与XOrJoin节点的运行
        /// </summary>
        FallBehindOfXOrJoin = 11,

        /// <summary>
        /// Not forced branch (EORJoin)
        /// 不是强制分支(EOrJoin)
        /// </summary>
        NotForcedBrancheWhenEOrJoin = 13
    }
}
