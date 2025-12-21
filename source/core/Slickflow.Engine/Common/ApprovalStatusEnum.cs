using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Approval Status
    /// 审批状态
    /// </summary>
    public enum ApprovalStatusEnum
    {
        /// <summary>
        /// Refused
        /// 拒绝
        /// </summary>
        Refused = -1,

        /// <summary>
        /// Null (default)
        /// 默认
        /// </summary>
        Null = 0,

        /// <summary>
        /// Agreed
        /// 同意
        /// </summary>
        Agreed = 1,
    }
}
