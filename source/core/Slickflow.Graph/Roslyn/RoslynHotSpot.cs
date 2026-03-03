
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Slickflow.Graph.Model;

namespace Slickflow.Graph.Roslyn
{
    /// <summary>
    /// Dynamically execute CSharp code
    /// 动态执行CSharp代码
    /// </summary>
    public class RoslynHotSpot
    {
        /// <summary>
        /// Execute
        /// </summary>
        public static async Task<RoslynExecuteResult> Execute(string codeText)
        {
            var result = RoslynExecuteResult.Default();
            try
            {
                if (!string.IsNullOrEmpty(codeText))
                {
                    // Security validation: Check namespace requirements before execution
                    // 安全验证：执行前检查命名空间要求
                    var validationResult = CodeSecurityValidator.Validate(codeText);
                    if (!validationResult.IsValid)
                    {
                        return RoslynExecuteResult.Error($"Security validation failed: {validationResult.Message}");
                    }

                    object x = await CSharpScript.EvaluateAsync(codeText,
                        ScriptOptions.Default.WithReferences(
                            typeof(Slickflow.Engine.Common.WfAppRunner).Assembly,
                            typeof(Workflow).Assembly)
                            );

                    result = RoslynExecuteResult.Success();
                }
            }
            catch (System.Exception ex)
            {
                result = RoslynExecuteResult.Error(ex.Message);
            }
            return result;
        }
    }
}