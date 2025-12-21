using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// Process branch routing selection
    /// 流程分支路由选择
    /// </summary>
    public enum GatewayDirectionEnum : int
    {
        /// <summary>
        /// None
        /// 未指定
        /// </summary>
        None = 0,

        /// <summary>
        /// Or Split
        /// 或分支
        /// </summary>
        OrSplit = 1,

        /// <summary>
        /// XOr Split
        /// 异或分支
        /// </summary>
        XOrSplit = 2,

        /// <summary>
        /// And Split
        /// 并行分支
        /// </summary>
        AndSplit = 4,

        /// <summary>
        /// And Split for multiple instance
        /// 多实例并行分支
        /// </summary>
        AndSplitMI = 5,

        /// <summary>
        /// Or Split for Approval 
        /// 审核网关分支
        /// </summary>
        ApprovalOrSplit = 6,

        /// <summary>
        /// Or Join
        /// 或合并
        /// </summary>
        OrJoin = 16,

        /// <summary>
        /// XOr Join
        /// 异或合并
        /// </summary>
        XOrJoin = 17,

        /// <summary>
        /// Enhanced Or Join
        /// 增强或合并
        /// </summary>
        EOrJoin = 18,

        /// <summary>
        /// And Join
        /// 并行合并
        /// </summary>
        AndJoin = 64,

        /// <summary>
        /// And Join for multiple instance
        /// 与合并(多实例)
        /// </summary>
        AndJoinMI = 65,
    }
}
