using System;
using System.Collections.Concurrent;
using Slickflow.Engine.Event;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Registry for ServiceTask LocalMethod binding (BPMN2 ##DelegateExpression semantic).
    /// 服务任务本地方法注册表，用于将 ServiceTask 节点绑定到本地方法。
    /// </summary>
    public interface IServiceTaskDelegateRegistry
    {
        /// <summary>
        /// Default variable name when Func returns plain object (not ServiceTaskResult).
        /// 当 Func 返回普通 object 时，引擎写入的默认变量名。调用方可用此常量读取，避免硬编码。
        /// </summary>
        string DefaultResultVariableName { get; }

        /// <summary>Register an Action delegate.</summary>
        void Register(string key, Action<IEventService> action);

        /// <summary>
        /// Register a Func delegate. Return value is written to a variable:
        /// - If delegate returns <see cref="ServiceTaskResult"/>, uses its VariableName;
        /// - Otherwise uses <see cref="DefaultResultVariableName"/>.
        /// </summary>
        void Register(string key, Func<IEventService, object> func);

        /// <summary>Resolve delegate by key.</summary>
        bool TryResolve(string key, out Delegate resolved);

        /// <summary>Unregister by key.</summary>
        bool Unregister(string key);
    }

    /// <summary>
    /// Thread-safe default implementation of delegate registry.
    /// </summary>
    public class ServiceTaskDelegateRegistry : IServiceTaskDelegateRegistry
    {
        /// <summary>
        /// Default variable name when Func returns plain object. Use this constant when reading
        /// the result to avoid hardcoding the internal name.
        /// 当 Func 返回普通 object 时的默认变量名。读取结果时请使用此常量，避免硬编码。
        /// </summary>
        public const string DefaultResultVariableName = "ServiceResult";

        string IServiceTaskDelegateRegistry.DefaultResultVariableName => DefaultResultVariableName;

        private readonly ConcurrentDictionary<string, Delegate> _delegates =
            new ConcurrentDictionary<string, Delegate>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Global singleton for process-level registration.</summary>
        public static readonly ServiceTaskDelegateRegistry Global = new ServiceTaskDelegateRegistry();

        public void Register(string key, Action<IEventService> d)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            _delegates[key] = d ?? throw new ArgumentNullException(nameof(d));
        }

        public void Register(string key, Func<IEventService, object> d)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            _delegates[key] = d ?? throw new ArgumentNullException(nameof(d));
        }

        public bool TryResolve(string key, out Delegate d)
        {
            d = null;
            if (string.IsNullOrWhiteSpace(key)) return false;
            return _delegates.TryGetValue(key, out d);
        }

        public bool Unregister(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;
            return _delegates.TryRemove(key, out _);
        }
    }
}
