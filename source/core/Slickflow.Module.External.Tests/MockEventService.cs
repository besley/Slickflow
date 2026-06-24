using System;
using System.Collections.Generic;
using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;

namespace Slickflow.Module.External.Tests
{
    /// <summary>
    /// In-memory IEventService for testing CustomerExtractService, CustomerSaveService and MessageService
    /// without a real workflow engine. Variables are stored in a dictionary.
    /// </summary>
    public sealed class MockEventService : IEventService
    {
        private readonly Dictionary<string, string> _variables = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly int _processInstanceId;

        public MockEventService(int processInstanceId = 90001)
        {
            _processInstanceId = processInstanceId;
        }

        public int GetProcessInstanceId() => _processInstanceId;

        public IDbSession GetSession() => null;

        public string GetVariable(ProcessVariableScopeEnum variableType, string name)
        {
            return GetVariableByScopePriority(name);
        }

        public string GetVariableByScopePriority(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            return _variables.TryGetValue(name.Trim(), out var v) ? (v ?? string.Empty) : string.Empty;
        }

        public void SaveVariable(ProcessVariableScopeEnum variableType, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _variables[name.Trim()] = value ?? string.Empty;
        }

        public string GetCondition(string name) => string.Empty;

        public void SetCondition(string name, string value) { }

        public T GetInstance<T>(int id) where T : class => null;

        /// <summary>
        /// Set process variable (for test setup).
        /// </summary>
        public void SetVariable(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name)) return;
            _variables[name.Trim()] = value ?? string.Empty;
        }

        /// <summary>
        /// Get current variables (for test assertion).
        /// </summary>
        public IReadOnlyDictionary<string, string> Variables => _variables;
    }
}
