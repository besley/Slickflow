using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Slickflow.AI.Agent.Core;

namespace Slickflow.AI.Agent.Registry
{
    /// <summary>
    /// Global registry mapping activity IDs to their registered tool lists.
    /// Follows the same singleton-global pattern as ServiceTaskDelegateRegistry.Global.
    ///
    /// Usage:
    ///   AgentToolRegistry.Global.Register("AGENT_001", tools);
    ///   if (AgentToolRegistry.Global.TryGetTools("AGENT_001", out var tools)) { ... }
    /// </summary>
    public sealed class AgentToolRegistry
    {
        /// <summary>The process-wide singleton instance.</summary>
        public static readonly AgentToolRegistry Global = new AgentToolRegistry();

        private readonly ConcurrentDictionary<string, IReadOnlyList<IAgentTool>> _store
            = new ConcurrentDictionary<string, IReadOnlyList<IAgentTool>>(StringComparer.OrdinalIgnoreCase);

        // Prevent external construction — use Global
        private AgentToolRegistry() { }

        /// <summary>
        /// Register or replace the tool list for an activity.
        /// </summary>
        /// <param name="activityId">The ActivityId from AiActivityConfigEntity.</param>
        /// <param name="tools">Tool list, typically built by AgentToolBuilder.</param>
        public void Register(string activityId, IReadOnlyList<IAgentTool> tools)
        {
            if (string.IsNullOrWhiteSpace(activityId))
                throw new ArgumentNullException(nameof(activityId));
            if (tools == null)
                throw new ArgumentNullException(nameof(tools));

            _store[activityId] = tools;
            Console.WriteLine($"[AgentToolRegistry] Registered {tools.Count} tool(s) for activity '{activityId}'");
        }

        /// <summary>
        /// Try to retrieve the tool list for an activity.
        /// Returns true when found, false otherwise.
        /// </summary>
        public bool TryGetTools(string activityId, out IReadOnlyList<IAgentTool> tools)
        {
            if (string.IsNullOrWhiteSpace(activityId))
            {
                tools = Array.Empty<IAgentTool>();
                return false;
            }
            return _store.TryGetValue(activityId, out tools);
        }

        /// <summary>
        /// Remove all tools registered for an activity.
        /// </summary>
        public void Clear(string activityId)
        {
            if (string.IsNullOrWhiteSpace(activityId)) return;
            _store.TryRemove(activityId, out _);
        }

        /// <summary>Remove all registrations.</summary>
        public void ClearAll() => _store.Clear();
    }
}
