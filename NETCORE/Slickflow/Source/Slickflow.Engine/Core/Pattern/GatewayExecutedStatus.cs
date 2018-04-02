using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Core.Pattern
{
    /// <summary>
    /// 网关执行结果状态类
    /// </summary>
    internal enum GatewayExecutedStatus
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0x0,

        /// <summary>
        /// 发生错误
        /// </summary>
        Failed = 0x1,

        /// <summary>
        /// 执行成功
        /// </summary>
        Successed = 0x2,

        /// <summary>
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 0x8,

        /// <summary>
        /// 后进的节点，不参与XOrJoin节点的运行
        /// </summary>
        FallBehindOfXOrJoin = 0x16

    }
}
