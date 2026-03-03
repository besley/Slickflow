using Slickflow.Engine.Common;
using Slickflow.Engine.Xpdl;
using System.Collections.Generic;


namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Execution context for auto (non-persistent) workflow run.
    /// 自动执行（非持久化）工作流的执行上下文。
    /// </summary>
    public class AutoExecutionContext
    {
        public IProcessModel ProcessModel { get; set; }
        public IDictionary<string, string> Variables { get; set; }
        public WfAppRunner Runner { get; set; }

        /// <summary>
        /// Track executed activity IDs to avoid executing the same node multiple times
        /// when multiple branches converge (e.g. join gateway, end node).
        /// 已执行活动ID集合，避免汇聚节点（如汇聚网关、结束节点）被多次执行
        /// </summary>
        public HashSet<string> ExecutedActivityIds { get; set; } = new HashSet<string>();

        /// <summary>
        /// For join nodes (multiple incoming branches): count how many branches have reached.
        /// Execute join (and nodes after it) only when all branches have arrived, so End runs last.
        /// 汇聚节点：记录已到达的分支数，等所有分支到达后再执行，保证 End 最后执行
        /// </summary>
        public Dictionary<string, int> JoinReachedCounts { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// Delegate registry for LocalMethod ServiceTask (BPMN2 ##DelegateExpression semantic).
        /// When null, ServiceTaskDelegateRegistry.Global is used.
        /// </summary>
        public IServiceTaskDelegateRegistry DelegateRegistry { get; set; }

        public AutoExecutionContext()
        {
            Variables = new Dictionary<string, string>();
        }
    }
}
