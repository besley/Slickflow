using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Core.Result
{
    /// <summary>
    /// 活动节点执行结果
    /// </summary>
    public class WfNodeMediatedResult : WfExecutedResult
    {
        public WfNodeMediatedFeedback Feedback { get; set; }
    }

    public enum WfNodeMediatedFeedback
    {
        /// <summary>
        /// 串行会签，设置下一个执行节点的任务进入运行状态
        /// </summary>
        ForwardToNextSequenceTask = 1,

        /// <summary>
        /// 并行会签，等待节点到达足够多的完成比例
        /// </summary>
        WaitingForCompletedMore = 2
    }
}
