using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.AI.Agent.Core;

namespace Slickflow.AI.Agent.Tools
{
    /// <summary>
    /// Wraps a local C# function as an IAgentTool for LLM function calling.
    /// </summary>
    public sealed class FunctionCallingTool : IAgentTool
    {
        private readonly Func<JsonObject, CancellationToken, Task<AgentToolResult>> _handler;

        public string     Name        { get; }
        public string     Description { get; }
        public JsonObject InputSchema { get; }

        public FunctionCallingTool(
            string     name,
            string     description,
            JsonObject inputSchema,
            Func<JsonObject, CancellationToken, Task<AgentToolResult>> handler)
        {
            Name        = name        ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            InputSchema = inputSchema ?? new JsonObject();
            _handler    = handler     ?? throw new ArgumentNullException(nameof(handler));
        }

        public Task<AgentToolResult> ExecuteAsync(JsonObject args, CancellationToken ct)
            => _handler(args, ct);

        /// <summary>
        /// Create a tool from a synchronous handler (auto-wrapped as async).
        /// </summary>
        public static FunctionCallingTool FromSync(
            string     name,
            string     description,
            JsonObject schema,
            Func<JsonObject, AgentToolResult> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            return new FunctionCallingTool(
                name, description, schema,
                (args, _) => Task.FromResult(handler(args)));
        }
    }

    /// <summary>
    /// Fluent builder for assembling a list of IAgentTool instances.
    /// Follows the Slickflow registry pattern.
    /// </summary>
    public sealed class AgentToolBuilder
    {
        private readonly List<IAgentTool> _tools = new List<IAgentTool>();

        /// <summary>Register an asynchronous tool.</summary>
        public AgentToolBuilder Register(
            string     name,
            string     description,
            JsonObject schema,
            Func<JsonObject, CancellationToken, Task<AgentToolResult>> handler)
        {
            _tools.Add(new FunctionCallingTool(name, description, schema, handler));
            return this;
        }

        /// <summary>Register a synchronous tool (auto-wrapped as async).</summary>
        public AgentToolBuilder Register(
            string     name,
            string     description,
            JsonObject schema,
            Func<JsonObject, AgentToolResult> handler)
        {
            _tools.Add(FunctionCallingTool.FromSync(name, description, schema, handler));
            return this;
        }

        /// <summary>Build the final immutable tool list.</summary>
        public IReadOnlyList<IAgentTool> Build() => _tools.AsReadOnly();
    }
}
