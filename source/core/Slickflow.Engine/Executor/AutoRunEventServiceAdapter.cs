using Slickflow.Data;
using Slickflow.Engine.Common;
using Slickflow.Engine.Event;
using System;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// IEventService adapter for auto-run: variable resolution from AutoExecutionContext.Variables only.
    /// 自动执行时的事件服务适配器：仅从 AutoExecutionContext.Variables 解析变量。
    /// </summary>
    internal sealed class AutoRunEventServiceAdapter : IEventService
    {
        private readonly AutoExecutionContext _context;

        public AutoRunEventServiceAdapter(AutoExecutionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int GetProcessInstanceId() => 0;

        public IDbSession GetSession() => null;

        public string GetVariable(ProcessVariableScopeEnum variableType, string name)
        {
            return GetVariableByScopePriority(name);
        }

        public string GetVariableByScopePriority(string name)
        {
            if (_context?.Variables == null || string.IsNullOrWhiteSpace(name))
                return string.Empty;
            return _context.Variables.TryGetValue(name.Trim(), out var v) ? (v ?? string.Empty) : string.Empty;
        }

        public void SaveVariable(ProcessVariableScopeEnum variableType, string name, string value)
        {
            if (_context?.Variables != null && !string.IsNullOrWhiteSpace(name))
                _context.Variables[name.Trim()] = value ?? string.Empty;
        }

        public string GetCondition(string name) => string.Empty;

        public void SetCondition(string name, string value) { }

        public T GetInstance<T>(int id) where T : class => null;
    }
}
