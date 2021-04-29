using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Common
{
    /// <summary>
    /// 流程分支路由选择
    /// </summary>
    public enum GatewayDirectionEnum : int
    {
        /// <summary>
        /// 未指定
        /// </summary>
        None = 0,

        /// <summary>
        /// 或分支
        /// </summary>
        OrSplit = 1,

        /// <summary>
        /// 异或分支
        /// </summary>
        XOrSplit = 2,

        /// <summary>
        /// 并行分支
        /// </summary>
        AndSplit = 4,

        /// <summary>
        /// 多实例并行分支
        /// </summary>
        AndSplitMI = 5,

        /// <summary>
        /// 审核网关分支
        /// </summary>
        ApprovalOrSplit = 6,

        /// <summary>
        /// 或合并
        /// </summary>
        OrJoin = 16,

        /// <summary>
        /// 异或合并
        /// </summary>
        XOrJoin = 17,

        /// <summary>
        /// 增强或合并
        /// </summary>
        EOrJoin = 18,

        /// <summary>
        /// 并行合并
        /// </summary>
        AndJoin = 64,

        /// <summary>
        /// 与合并(多实例)
        /// </summary>
        AndJoinMI = 65,
    }

    /// <summary>
    /// 合并通过类型设置
    /// </summary>
    public enum GatewayJoinPassEnum : int
    {
        /// <summary>
        /// 空白
        /// </summary>
        None = 0,

        /// <summary>
        /// 满足分支数目，才可以通过
        /// </summary>
        TokenCountPass = 1,

        /// <summary>
        /// 满足强制必须合并分支，才可以通过
        /// </summary>
        ForcedBranchPass = 2
    }
}
