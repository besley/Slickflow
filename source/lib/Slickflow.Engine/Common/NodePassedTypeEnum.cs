using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Node Approval type
    /// 节点通过类型
    /// </summary>
    public enum NodePassedTypeEnum
    {
        /// <summary>
        /// Default
        /// 缺省状态
        /// </summary>
        Default = 0,

        /// <summary>
        /// Approval passed
        /// 允许通过
        /// </summary>
        Passed = 1,

        /// <summary>
        /// Approval not passed
        /// 拒绝状态
        /// </summary>
        NotPassed = 2,

        /// <summary>
        /// More approval and consent are needed
        /// 需要更多审批同意
        /// </summary>
        NeedToBeMoreApproved = 3
    }
}
