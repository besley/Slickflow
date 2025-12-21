using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Business.Entity;
using Slickflow.Data;
using Slickflow.Engine.Xpdl.Template;
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
                result = ResponseResult<ProcessEntity>.Success(null,
                    @"The current feature is the basic function of the community edtion. If you need the advanced feature of the process graph, please contact the sales to become the Enterprise edtion customer"
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<ProcessEntity>.Error($"An error occurred when calling this method, detai:{ex.Message}");
            }
            return result;
        }
    }
}
