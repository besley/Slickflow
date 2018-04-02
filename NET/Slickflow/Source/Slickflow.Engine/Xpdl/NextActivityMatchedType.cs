using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slickflow.Engine.Xpdl
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
        Failed = 0x1,

        /// <summary>
        /// 执行成功
        /// </summary>
        Successed = 0x2,

        /// <summary>
        /// 被表达式过滤
        /// </summary>
        NoneTransitionFilteredByCondition = 0x4,

        /// <summary>
        /// 需要其他必需的并行分支
        /// </summary>
        WaitingForSplitting = 0x8,

        /// <summary>
        /// 没有任何满足条件的路径（OrSplit, XOrSplit)
        /// </summary>
        NoneTransitionMatchedToSplit = 0x16,

        /// <summary>
        /// 等待其它需要合并的分支
        /// </summary>
        WaitingForOthersJoin = 0x32,

        /// <summary>
        /// 条件没有匹配(OrJoin, XOrJoin)
        /// </summary>
        NotMadeItselfToJoin = 0x64,

        /// <summary>
        /// 不能继续运行
        /// </summary>
        CannotContinueRunning = 0x128
    }
}
