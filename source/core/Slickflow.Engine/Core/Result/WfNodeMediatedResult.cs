using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.Engine.Core.Result
{
    /// <summary>
    /// Node mediator result
    /// 活动节点执行结果
    /// </summary>
    public class WfNodeMediatedResult : WfExecutedResult
    {
        /// <summary>
        /// Feedback
        /// 返回内容
        /// </summary>
        public WfNodeMediatedFeedback Feedback { get; set; }

        /// <summary>
        /// Created method
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public static WfNodeMediatedResult CreateNodeMediatedResultWithException(WfNodeMediatedFeedback feedback)
        {
            var mediatedResult = new WfNodeMediatedResult();
            mediatedResult.Status = WfExecutedStatus.Exception;
            mediatedResult.Feedback = feedback;

            return mediatedResult;
        }

    }

    /// <summary>
    /// Execute feedback enumeration
    /// 执行反馈枚举
    /// </summary>
    public enum WfNodeMediatedFeedback
    {
        /// <summary>
        /// Serial signature will be added to set the task of the next execution node to enter the running state
        /// 串行会(加)签，设置下一个执行节点的任务进入运行状态
        /// </summary>
        ForwardToNextSequenceTask = 1,

        /// <summary>
        /// Parallel lines will be signed, waiting for nodes to reach a sufficient completion rate
        /// 并行会(加)签，等待节点到达足够多的完成比例
        /// </summary>
        WaitingForCompletedMore = 2,

        /// <summary>
        /// When signing, the pass rate requirement was not met
        /// 会(加)签时，没有达到通过率要求
        /// </summary>
        NotEnoughApprovalBranchesCount = 3,

        /// <summary>
        /// Parallel convergence requires other branches
        /// 并行汇合需要其它分支
        /// </summary>
        NeedOtherGatewayBranchesToJoin = 13,

        /// <summary>
        /// OrJoin scenario, the first branch that meets the criteria is completed, and subsequent branches are blocked
        /// OrJoin场景，第一个满足条件的分支完成，其后的被阻止
        /// </summary>
        OrJoinOneBranchHasBeenFinishedWaittingOthers = 14,

        /// <summary>
        /// Intermediate event handling node exception
        /// 中间事件处理节点异常
        /// </summary>
        IntermediateEventFailed = 15,

        /// <summary>
        /// Other unknown factors need to be debugged
        /// 其它未知因素需调试
        /// </summary>
        OtherUnknownReasonToDebug = 16,

        /// <summary>
        /// Unknown type of nodes need to be monitored
        /// 未知类型的节点需监视
        /// </summary>
        UnknownNodeTypeToWatch = 17
    }
}
