using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// Next Activity Matched Type
    /// 下一步活动的调度状态
    /// </summary>
    public enum NextActivityMatchedType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Failed
        /// </summary>
        Failed = 1,

        /// <summary>
        /// Successed
        /// </summary>
        Successed = 2,

        /// <summary>
        /// Filter by expression
        /// 被表达式过滤
        /// </summary>
        NoneTransitionFilteredByCondition = 4,

        /// <summary>
        /// Other necessary parallel branches are required
        /// 需要其他必需的并行分支
        /// </summary>
        WaitingForOtherSplitting = 6,

        /// <summary>
        /// Waiting for approval or rejection of the operation (ApprovalorSplit)
        /// 等待同意或者拒绝操作(ApprovalOrSplit)
        /// </summary>
        WaitingForAgreedOrRefused = 7,

        /// <summary>
        /// There are no paths that meet the conditions (OrSplit, XOrSplit)
        /// 没有任何满足条件的路径（OrSplit, XOrSplit)
        /// </summary>
        NoneTransitionMatchedToSplit = 10,

        /// <summary>
        /// The signing node does not meet the pass rate
        /// 会签节点未满足通过率
        /// </summary>
        FailedPassRateOfMulitipleInstance = 12,

        /// <summary>
        /// Waiting for other branches that need to be merged
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 32,

        /// <summary>
        /// Condition not matched (OrJoin, XOrJoin)
        /// 条件没有匹配(OrJoin, XOrJoin)
        /// </summary>
        NotMadeItselfToJoin = 64,

        /// <summary>
        /// Cannot continue running
        /// 不能继续运行
        /// </summary>
        CannotContinueRunning = 128
    }
}
