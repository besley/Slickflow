using Microsoft.AspNetCore.Mvc;
using sfdapi.Models;
using sfdapi.Services;
using Slickflow.AI.Entity;
using Slickflow.AI.Service;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;
using Slickflow.Module.Localize;
using Slickflow.WebUtility;

namespace sfdapi.Controllers
{
    /// <summary>
    /// Ai service controller
    /// 智能服务
    /// </summary>
    public class Wf2AiController : Controller
    {
        #region Propery and Construct
        private IWorkflowService _workflowService;
        private IAiModelDataService _aiModelDataService;
        private IAiFastCallingService _aiFastCallingService;
        private SfBpmnServiceBuilder _bpmnServiceBuilder;
        public Wf2AiController(IWorkflowService workflowService, 
            IAiModelDataService aiModelDataService,
            IAiFastCallingService aIFastCallingService,
            SfBpmnServiceBuilder builder) 
        { 
            _workflowService = workflowService;
            _aiModelDataService = aiModelDataService;
            _aiFastCallingService = aIFastCallingService;
            _bpmnServiceBuilder = builder;
        }
        #endregion

        #region AI Model Service
        public async Task<ResponseResult<AiModelProviderEntity>> SaveModel([FromBody] AiModelProviderEntity modelEntity)
        {
            var result = ResponseResult<AiModelProviderEntity>.Default();
            try
            {
                if (modelEntity.Id == 0)
                {
                    var newModelId = Task.Run(() => _aiModelDataService.CreateModel(modelEntity)).Result;
                    // Get the saved entity to return complete data
                    var savedEntity = _aiModelDataService.GetModelById(newModelId);
                    result = ResponseResult<AiModelProviderEntity>.Success(savedEntity,
                        LocalizeHelper.GetDesignerMessage("wf2aicontroller.savemodel.success")
                    );
                }
                else
                {
                    var isUpdated = Task.Run(() => _aiModelDataService.UpdateModel(modelEntity)).Result;
                    if (isUpdated)
                    {
                        // Get the updated entity to return complete data
                        var updatedEntity = _aiModelDataService.GetModelById(modelEntity.Id);
                        result = ResponseResult<AiModelProviderEntity>.Success(updatedEntity,
                            LocalizeHelper.GetDesignerMessage("wf2aicontroller.savemodel.success")
                        );
                    }
                    else
                    {
                        result = ResponseResult<AiModelProviderEntity>.Error(
                            LocalizeHelper.GetDesignerMessage("wf2aicontroller.savemodel.error", "Update failed")
                        );
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<AiModelProviderEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.savemodel.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get model entity
        /// 获取ai model记录
        /// </summary>
        [HttpGet]
        public ResponseResult<AiModelProviderEntity> GetModel(int id)
        {
            var result = ResponseResult<AiModelProviderEntity>.Default();
            try
            {
                var entity = _aiModelDataService.GetModelById(id);

                result = ResponseResult<AiModelProviderEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<AiModelProviderEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.getmodel.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get model list 
        /// 获取模型记录列表
        /// </summary>
        [HttpGet]
        public ResponseResult<List<AiModelProviderEntity>> GetModelList()
        {
            var result = ResponseResult<List<AiModelProviderEntity>>.Default();
            try
            {
                var entity = _aiModelDataService.GetModelListAll();

                result = ResponseResult<List<AiModelProviderEntity>>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<AiModelProviderEntity>>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.getmodellist.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Delete model
        /// 删除模型数据
        /// </summary>
        [HttpGet]
        public ResponseResult DeleteModel(int id)
        {
            var result = ResponseResult.Default();
            try
            {
                _aiModelDataService.DeleteModel(id);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.deletemodel.error", ex.Message));
            }
            return result;
        }
        #endregion

        #region AxConfig Service 
        /// <summary>
        /// Create axconfig record
        /// 创建AI节点配置信息
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult<AiActivityConfigEntity>> SaveAxConfig([FromBody] AiActivityConfigEntity entity)
        {
            var result = ResponseResult<AiActivityConfigEntity>.Default();
            try
            {
                _aiModelDataService.SaveAiActivityConfigInfo(entity);
                result = ResponseResult<AiActivityConfigEntity>.Success(entity,
                    LocalizeHelper.GetDesignerMessage("wf2aicontroller.saveaxconfig.success")
                );
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<AiActivityConfigEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.saveaxconfig.error", ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get agent entity
        /// 获取智能体记录
        /// </summary>
        [HttpGet]
        public ResponseResult<AiActivityConfigEntity> GetAxConfig(string id)
        {
            var result = ResponseResult<AiActivityConfigEntity>.Default();
            try
            { 
                var entity = _aiModelDataService.GetAiActivityConfigByUUID(id);

                result = ResponseResult<AiActivityConfigEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<AiActivityConfigEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.getaxconfig.error", ex.Message));
            }
            return result;
        }

        public ResponseResult DeleteAxConfig(string id)
        {
            var result = ResponseResult.Default();
            try
            {
                _aiModelDataService.DeleteAiActivityConfig(id);
                result = ResponseResult.Success();
            }
            catch (System.Exception ex)
            {
                result = ResponseResult.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.deleteaxconfig.error", ex.Message));
            }
            return result;
        }

        [HttpGet]
        public string EncryptLocalApiKey()
        {
            //var result = ApiKeyCryptoHelper.Encrypt("xxx", ApiKeyCryptoHelper.AiAppConfigOptions.MasterKeyDb);
            return string.Empty;
        }
        #endregion

        #region Test Connection
        /// <summary>
        /// Test model connection
        /// 测试模型连接
        /// </summary>
        [HttpPost]
        public async Task<ResponseResult<string>> TestModelConnection([FromBody] TestModelConnectionRequest request)
        {
            var result = ResponseResult<string>.Default();
            try
            {
                if (string.IsNullOrWhiteSpace(request.BaseUrl))
                {
                    result = ResponseResult<string>.Error("BaseUrl is required");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.ApiUUID))
                {
                    result = ResponseResult<string>.Error("ApiUUID is required");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.ApiKey))
                {
                    result = ResponseResult<string>.Error("ApiKey is required");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.ModelProvider))
                {
                    result = ResponseResult<string>.Error("ModelProvider is required");
                    return result;
                }

                var testResult = await _aiFastCallingService.TestModelConnectionAsync(request.BaseUrl,
                    request.ApiUUID,
                    request.ApiKey,
                    request.ModelProvider);
                result = ResponseResult<string>.Success(testResult, "Connection test successful");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<string>.Error($"Connection test failed: {ex.Message}");
            }
            return result;
        }
        #endregion
    }
}