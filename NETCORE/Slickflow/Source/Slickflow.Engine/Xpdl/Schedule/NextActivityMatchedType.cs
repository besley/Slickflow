using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl.Schedule
{
    /// <summary>
    /// 下一步活动的调度状态
    /// </summary>
    public enum NextActivityMatchedType
    {
           /// <summary>
        /// 发生错误
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Failed = 1,

        /// <summary>
        /// 执行成功
        /// </summary>
        Successed = 2,

        /// <summary>
        /// 被表达式过滤
        /// </summary>
        NoneTransitionFilteredByCondition = 4,

        /// <summary>
        /// 需要其他必需的并行分支
        /// </summary>
        WaitingForOtherSplitting = 6,

        /// <summary>
        /// 等待同意或者拒绝操作(ApprovalOrSplit)
        /// </summary>
        WaitingForAgreedOrRefused = 7,

        /// <summary>
        /// 没有任何满足条件的路径（OrSplit, XOrSplit)
        /// </summary>
        NoneTransitionMatchedToSplit = 10,

        /// <summary>
        /// 会签节点未满足通过率
        /// </summary>
        FailedPassRateOfMulitipleInstance = 12,

        /// <summary>
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 32,

        /// <summary>
        /// 条件没有匹配(OrJoin, XOrJoin)
        /// </summary>
        NotMadeItselfToJoin = 64,

        /// <summary>
        /// 不能继续运行
        /// </summary>
        CannotContinueRunning = 128
    }
}
