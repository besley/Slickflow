using System;

namespace Slickflow.Engine.Executor
{
    /// <summary>
    /// Result wrapper for Func delegate: lets the delegate explicitly specify
    /// the output variable name instead of using the engine default.
    /// Func 委托的返回值包装：允许显式指定输出变量名，避免依赖引擎内部的默认约定。
    /// </summary>
    /// <example>
    /// <code>
    /// reg.Register("CalcAmount", (IEventService svc) =>
    /// {
    ///     var total = 100.5;
    ///     return new ServiceTaskResult("OrderTotal", total);  // 写入 OrderTotal，而非 ServiceResult
    /// });
    /// </code>
    /// </example>
    public class ServiceTaskResult
    {
        /// <summary>
        /// Variable name to write the value into.
        /// 写入的变量名。
        /// </summary>
        public string VariableName { get; }

        /// <summary>
        /// The value to write.
        /// 要写入的值。
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Creates a result with explicit variable name.
        /// </summary>
        /// <param name="variableName">Variable name for the result (e.g. "OrderTotal").</param>
        /// <param name="value">The value to write.</param>
        public ServiceTaskResult(string variableName, object value)
        {
            VariableName = variableName ?? throw new ArgumentNullException(nameof(variableName));
            Value = value;
        }
    }
}
