using Microsoft.AspNetCore.Mvc;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Module.Localize;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    /// <summary>
    /// 流程变量定义控制器（wf_variable 表，对应 BPMN sf:variables）
    /// </summary>
    public class WfVariableController : Controller
    {
        private readonly IWorkflowService _workflowService;

        public WfVariableController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        /// <summary>
        /// 获取指定流程节点下的变量定义列表
        /// </summary>
        /// <param name="processId">流程ID</param>
        /// <param name="version">版本</param>
        /// <param name="activityId">节点ID（活动编码）</param>
        [HttpGet]
        public ResponseResult<IList<VariableEntity>> GetList([FromQuery] string processId, [FromQuery] string version, [FromQuery] string activityId)
        {
            var result = ResponseResult<IList<VariableEntity>>.Default();
            try
            {
                var list = _workflowService.GetVariableDefinitionList(processId ?? "", version ?? "1", activityId ?? "");
                result = ResponseResult<IList<VariableEntity>>.Success(list);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<IList<VariableEntity>>.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 保存指定节点的变量定义列表（先删后增，整节点覆盖）
        /// </summary>
        [HttpPost]
        public ResponseResult SaveList([FromQuery] string processId, [FromQuery] string version, [FromQuery] string activityId, [FromBody] IList<VariableEntity> list)
        {
            var result = ResponseResult.Default();
            try
            {
                if (string.IsNullOrEmpty(processId) || string.IsNullOrEmpty(activityId))
                {
                    result = ResponseResult.Error("processId 与 activityId 不能为空");
                    return result;
                }
                _workflowService.SaveVariableDefinitionList(processId, version ?? "1", activityId, list ?? new List<VariableEntity>());
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 删除一条变量定义
        /// </summary>
        [HttpDelete]
        public ResponseResult Delete([FromQuery] int id)
        {
            var result = ResponseResult.Default();
            try
            {
                _workflowService.DeleteVariableDefinition(id);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(ex.Message);
            }
            return result;
        }
    }
}
