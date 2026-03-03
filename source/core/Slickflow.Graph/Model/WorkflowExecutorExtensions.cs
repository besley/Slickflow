using System;
using System.Collections.Concurrent;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Executor;

namespace Slickflow.Graph.Model
{
    /// <summary>
    /// Extension methods to bridge Slickflow.Graph.Workflow (graph model)
    /// with Slickflow.Engine.Executor.WorkflowExecutor (runtime executor).
    /// </summary>
    public static class WorkflowExecutorExtensions
    {
        // Simple in-memory cache for process definitions built from graph workflows.
        // Key format: "{ProcessId}:{Version}"
        private static readonly ConcurrentDictionary<string, ProcessEntity> _processCache =
            new ConcurrentDictionary<string, ProcessEntity>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Uses a graph-defined workflow as the process definition for the executor, without database lookup.
        /// This method:
        /// 1) Builds the BPMN XML into an in-memory ProcessEntity (no insert).
        /// 2) Caches the ProcessEntity by process code + version.
        /// 3) Delegates to the new UseProcess(ProcessEntity) overload on the executor.
        /// </summary>
        /// <param name="executor">Workflow executor (chain-style API)</param>
        /// <param name="workflow">Graph Workflow definition</param>
        /// <returns>IWorkflowExecutor for fluent chaining</returns>
        public static IWorkflowExecutor UseProcess(this IWorkflowExecutor executor, Workflow workflow)
        {
            if (executor == null) throw new ArgumentNullException(nameof(executor));
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            var processId = workflow.ProcessId ?? throw new InvalidOperationException("Workflow.ProcessId is null.");
            var version = workflow.Version ?? "1";
            var cacheKey = $"{processId}:{version}";

            // Get or build the in-memory ProcessEntity
            var entity = _processCache.GetOrAdd(cacheKey, _ => workflow.BuildInMemory());

            // Reuse the new UseProcess(ProcessEntity) overload, avoiding any database roundtrip.
            return executor.UseProcess(entity);
        }
    }
}

