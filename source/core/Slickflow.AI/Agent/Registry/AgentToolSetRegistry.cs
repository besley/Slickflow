using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Slickflow.AI.Agent.Attributes;
using Slickflow.AI.Agent.Core;
using Slickflow.AI.Agent.Reflection;
using Slickflow.AI.Agent.Tools;

namespace Slickflow.AI.Agent.Registry
{
    /// <summary>
    /// Global registry mapping logical tool-set names to their tool implementations.
    ///
    /// ── Design intent ────────────────────────────────────────────────────────────
    /// The workflow BPMN definition stores only a logical name (toolSetName) per Agent
    /// node — e.g. "SupplierSearch" — keeping the definition business-readable and
    /// independent of any concrete assembly or class name.
    ///
    /// At application startup, each tool set is registered here by name:
    ///   AgentToolSetRegistry.Global.Register("SupplierSearch",
    ///       () => new AgentToolBuilder().Register(...).Build());
    ///
    /// At runtime the engine reads toolSetName from the workflow, looks it up here,
    /// and passes the resolved tools to the agent:
    ///   AgentToolSetRegistry.Global.TryResolveToActivityRegistry("SupplierSearch", activityId);
    ///
    /// ── What lives where ─────────────────────────────────────────────────────────
    /// IN WORKFLOW DEFINITION (BPMN XML):
    ///   toolSetName="SupplierSearch"   ← logical name, set in the properties panel
    ///
    /// IN CODE (this registry + tool implementations):
    ///   Tool C# logic (lambdas/methods) — cannot be stored in XML
    ///   The mapping toolSetName → AgentToolBuilder factory
    ///
    /// ── Linking workflow to code ──────────────────────────────────────────────────
    /// The toolSetName is the contract between the workflow designer and the developer:
    ///   Designer sets: toolSetName = "SupplierSearch"
    ///   Developer registers: AgentToolSetRegistry.Global.Register("SupplierSearch", ...)
    /// If the name doesn't match, TryGetTools returns false and the agent runs tool-free.
    /// </summary>
    public sealed class AgentToolSetRegistry
    {
        /// <summary>The process-wide singleton instance.</summary>
        public static readonly AgentToolSetRegistry Global = new AgentToolSetRegistry();

        // Factory functions so each call produces a fresh tool list (stateless tools are fine
        // as singletons, but stateful tools would need a new instance per run).
        private readonly ConcurrentDictionary<string, Func<IReadOnlyList<IAgentTool>>> _store
            = new ConcurrentDictionary<string, Func<IReadOnlyList<IAgentTool>>>(StringComparer.OrdinalIgnoreCase);

        private AgentToolSetRegistry() { }

        /// <summary>
        /// Register a named tool set from a service class decorated with [AgentToolSet].
        /// The toolSetName is read from the attribute — no string constant needed.
        ///
        /// Example:
        ///   AgentToolSetRegistry.Global.Register&lt;SupplierSearchService&gt;();
        /// </summary>
        public AgentToolSetRegistry Register<T>() where T : class, new()
        {
            var type = typeof(T);
            var attr = type.GetCustomAttributes(typeof(AgentToolSetAttribute), false);
            if (attr.Length == 0)
                throw new InvalidOperationException(
                    $"{type.Name} must be decorated with [AgentToolSet(\"name\")] to be registered this way.");

            var toolSetName = ((AgentToolSetAttribute)attr[0]).Name;
            var tools       = AgentToolReflector.BuildToolsFromType(type);
            _store[toolSetName] = () => tools;
            Console.WriteLine($"[AgentToolSetRegistry] Registered tool set '{toolSetName}' ({tools.Count} tool(s)) from {type.Name}");
            return this;
        }

        /// <summary>
        /// Register a named tool set via a builder factory.
        /// Call this at application/test startup, before any workflow runs.
        /// </summary>
        /// <param name="toolSetName">
        ///   Logical name — must match the toolSetName attribute in the BPMN sf:AiService element.
        /// </param>
        /// <param name="builderFactory">
        ///   A factory that returns a configured AgentToolBuilder.
        ///   Called once per registration; the resulting tool list is cached per registration call.
        /// </param>
        public AgentToolSetRegistry Register(string toolSetName, Func<AgentToolBuilder> builderFactory)
        {
            if (string.IsNullOrWhiteSpace(toolSetName)) throw new ArgumentNullException(nameof(toolSetName));
            if (builderFactory == null)                 throw new ArgumentNullException(nameof(builderFactory));

            // Build once and cache so the tools are ready without rebuilding on every lookup.
            var tools = builderFactory().Build();
            _store[toolSetName] = () => tools;
            Console.WriteLine($"[AgentToolSetRegistry] Registered tool set '{toolSetName}' ({tools.Count} tool(s))");
            return this;
        }

        /// <summary>
        /// Try to get the tool list for a given toolSetName.
        /// Returns empty list when the name is not registered.
        /// </summary>
        public bool TryGetTools(string toolSetName, out IReadOnlyList<IAgentTool> tools)
        {
            if (string.IsNullOrWhiteSpace(toolSetName))
            {
                tools = Array.Empty<IAgentTool>();
                return false;
            }
            if (_store.TryGetValue(toolSetName, out var factory))
            {
                tools = factory();
                return true;
            }
            tools = Array.Empty<IAgentTool>();
            return false;
        }

        /// <summary>
        /// Resolve toolSetName → tools and forward-register them into AgentToolRegistry
        /// under the given activityId.  This bridges the tool-set registry (name-keyed)
        /// with the activity registry (activityId-keyed) used by the workflow engine.
        /// </summary>
        /// <returns>True if the tool set was found and registered.</returns>
        public bool TryResolveToActivityRegistry(string toolSetName, string activityId)
        {
            if (!TryGetTools(toolSetName, out var tools)) return false;
            AgentToolRegistry.Global.Register(activityId, tools);
            return true;
        }

        /// <summary>Remove all registrations.</summary>
        public void ClearAll() => _store.Clear();
    }
}
