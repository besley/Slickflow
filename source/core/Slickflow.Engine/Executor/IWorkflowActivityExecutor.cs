using Slickflow.Engine.Core.Result;
using Slickflow.Engine.Xpdl.Entity;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Executes a single workflow activity (services, AI, script, end).
    /// 执行单个工作流活动节点（服务、AI、脚本、结束）。
    /// </summary>
    public interface IWorkflowActivityExecutor
    {
        /// <summary>
        /// Execute one activity: service node, AI service node, script, etc.
        /// 执行单个活动：服务节点、AI 服务节点、脚本等
        /// </summary>
        void ExecuteActivity(Activity currentActivity, AutoExecutionContext context, WfExecutedResult result);

        /// <summary>
        /// Execute end activity / 执行结束节点
        /// </summary>
        void ExecuteEndActivity(Activity endActivity, AutoExecutionContext context);
    }
}
