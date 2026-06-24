using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Slickflow.AI.Agent.Core
{
    /// <summary>
    /// Agent execution context: bridges Slickflow workflow variables with the ReAct runtime.
    /// </summary>
    public interface IAgentContext
    {
        string ProcessInstanceId { get; }
        string ActivityId        { get; }

        /// <summary>
        /// Read a workflow variable.
        /// For string T, returns the raw value.
        /// For other types, deserializes the JSON-encoded value.
        /// </summary>
        T? GetVariable<T>(string key);

        /// <summary>
        /// Write a variable. Complex objects are serialized to JSON.
        /// </summary>
        void SetVariable(string key, object value);

        /// <summary>All variables accessible as string dictionary.</summary>
        IReadOnlyDictionary<string, string> Variables { get; }

        /// <summary>In-memory conversation history for this execution.</summary>
        IReadOnlyList<AgentMessage> History { get; }

        /// <summary>Append a message to the history.</summary>
        void AppendMessage(AgentMessage message);
    }

    /// <summary>
    /// Default implementation: wraps the Dictionary&lt;string, string&gt; from
    /// AutoExecutionContext.Variables (the same map WorkflowActivityExecutor uses).
    /// </summary>
    public sealed class AgentContext : IAgentContext
    {
        private readonly Dictionary<string, string> _variables;
        private readonly List<AgentMessage>         _history = new List<AgentMessage>();

        public string ProcessInstanceId { get; }
        public string ActivityId        { get; }

        public AgentContext(
            string processInstanceId,
            string activityId,
            Dictionary<string, string> variables)
        {
            ProcessInstanceId = processInstanceId ?? string.Empty;
            ActivityId        = activityId        ?? string.Empty;
            _variables        = variables         ?? new Dictionary<string, string>();
        }

        public T? GetVariable<T>(string key)
        {
            if (!_variables.TryGetValue(key, out var raw) || raw == null)
                return default;

            if (typeof(T) == typeof(string))
                return (T)(object)raw;

            try
            {
                return JsonSerializer.Deserialize<T>(raw, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return default;
            }
        }

        public void SetVariable(string key, object value)
        {
            if (value == null)
            {
                _variables[key] = string.Empty;
                return;
            }

            if (value is string s)
                _variables[key] = s;
            else
                _variables[key] = JsonSerializer.Serialize(value);
        }

        public IReadOnlyDictionary<string, string> Variables => _variables;

        public IReadOnlyList<AgentMessage> History => _history;

        public void AppendMessage(AgentMessage message)
        {
            if (message != null)
                _history.Add(message);
        }
    }
}
