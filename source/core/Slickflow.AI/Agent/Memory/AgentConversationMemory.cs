using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Slickflow.AI.Agent.Memory
{
    /// <summary>
    /// Shared in-memory store for agent outputs within the same process instance.
    /// Allows downstream agents to read outputs produced by upstream agents
    /// using only the process instance ID and variable name.
    ///
    /// Thread-safe via ConcurrentDictionary.
    /// Data is per-process-instance and lives for the duration of the process.
    /// </summary>
    public static class AgentConversationMemory
    {
        // Key: processInstanceId  →  Value: { variableName → value }
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _store
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        // Tracks which activity wrote which variable (for diagnostics / ordering)
        private static readonly ConcurrentDictionary<string, List<string>> _writtenByProcess
            = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Record an output from an agent activity into the shared memory.
        /// </summary>
        /// <param name="processInstanceId">The workflow process instance ID.</param>
        /// <param name="activityId">The agent's activity ID (for audit).</param>
        /// <param name="variableName">The variable name.</param>
        /// <param name="value">The serialized (string) value.</param>
        public static void RecordOutput(
            string processInstanceId,
            string activityId,
            string variableName,
            string value)
        {
            if (string.IsNullOrWhiteSpace(processInstanceId)) throw new ArgumentNullException(nameof(processInstanceId));
            if (string.IsNullOrWhiteSpace(variableName))      throw new ArgumentNullException(nameof(variableName));

            var processStore = _store.GetOrAdd(
                processInstanceId,
                _ => new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase));

            processStore[variableName] = value ?? string.Empty;

            // Track variable names written for this process
            var written = _writtenByProcess.GetOrAdd(processInstanceId, _ => new List<string>());
            lock (written)
            {
                if (!written.Contains(variableName))
                    written.Add(variableName);
            }

            Console.WriteLine($"[AgentMemory] Recorded '{variableName}' for process '{processInstanceId}' (activity='{activityId}')");
        }

        /// <summary>
        /// Retrieve an output variable from shared memory.
        /// Returns null when not found.
        /// </summary>
        public static string? GetOutput(string processInstanceId, string variableName)
        {
            if (string.IsNullOrWhiteSpace(processInstanceId) || string.IsNullOrWhiteSpace(variableName))
                return null;

            if (_store.TryGetValue(processInstanceId, out var processStore)
                && processStore.TryGetValue(variableName, out var value))
                return value;

            return null;
        }

        /// <summary>
        /// Get all variable names written for a given process instance.
        /// </summary>
        public static IReadOnlyList<string> GetWrittenVariables(string processInstanceId)
        {
            if (_writtenByProcess.TryGetValue(processInstanceId, out var list))
            {
                lock (list) { return list.ToArray(); }
            }
            return Array.Empty<string>();
        }

        /// <summary>
        /// Evict all memory for a process instance (call after process completes).
        /// </summary>
        public static void Evict(string processInstanceId)
        {
            _store.TryRemove(processInstanceId, out _);
            _writtenByProcess.TryRemove(processInstanceId, out _);
        }
    }
}
