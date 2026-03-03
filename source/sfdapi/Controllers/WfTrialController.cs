using Microsoft.AspNetCore.Mvc;
using Slickflow.Data;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Xpdl.Template;
using Slickflow.Graph;
using Slickflow.Graph.Model;
using Slickflow.Graph.Roslyn;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    public class WfTrialController : Controller
    {
        /// <summary>
        /// Code creation flowchart
        /// 代码创建流程图
        /// </summary>
        [HttpPost]
        public ResponseResult<ProcessEntity> ExecuteProcessGraph([FromBody] ProcessGraph graph)
        {
            var result = ResponseResult<ProcessEntity>.Default();
            try
            {
                // Security validation: Check namespace requirements before execution
                // 安全验证：执行前检查命名空间要求
                if (graph != null && !string.IsNullOrEmpty(graph.Body))
                {
                    var validationResult = CodeSecurityValidator.Validate(graph.Body);
                    if (!validationResult.IsValid)
                    {
                        return ResponseResult<ProcessEntity>.Error($"Security validation failed: {validationResult.Message}");
                    }
                }

                var roslynBuilder = new RoslynBuilder();
                var roslynResult = roslynBuilder.Execute(graph);
                if (roslynResult.Status == 1)
                {
                    result = ResponseResult<ProcessEntity>.Success(roslynResult.Process);
                }
                else
                {
                    result = ResponseResult<ProcessEntity>.Error(roslynResult.Message);
                }
                //result = ResponseResult<ProcessEntity>.Success(null,
                //    @"The current feature is the basic function of the community edtion. If you need the advanced feature of the process graph, please contact the sales to become the Enterprise edtion customer"
                //);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error($"An error occurred when calling this method, detai:{ex.Message}");
            }
            return result;
        }
    }
}
