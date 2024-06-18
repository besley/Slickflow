using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 节点运行状态
    /// </summary>
    public enum ApprovalStatusEnum
    {
        /// <summary>
        /// 拒绝
        /// </summary>
        Refused = -1,

        /// <summary>
        /// 默认
        /// </summary>
        Null = 0,

        /// <summary>
        /// 同意
        /// </summary>
        Agreed = 1,
    }
}
