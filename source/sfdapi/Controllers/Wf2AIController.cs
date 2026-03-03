using Microsoft.AspNetCore.Mvc;
using sfdapi.Models;
using sfdapi.Service;
using Slickflow.AI.Entity;
using Slickflow.AI.Service;
using Slickflow.AI.Configuration;
using Slickflow.Engine.Business.Entity;
using Slickflow.Engine.Service;
using Slickflow.Engine.Utility;
using Slickflow.Engine.Xpdl.Common;
using Slickflow.Engine.Xpdl.Template;
using Slickflow.Module.Localize;
using Slickflow.WebUtility;
using System.Collections.Generic;

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
        /// Get model list by model type (e.g. vector_model for embedding models)
        /// 按模型类型获取模型列表（如 vector_model 用于嵌入模型）
        /// </summary>
        [HttpGet]
        public ResponseResult<List<AiModelProviderEntity>> GetModelListByType(string modelType)
        {
            var result = ResponseResult<List<AiModelProviderEntity>>.Default();
            try
            {
                var list = _aiModelDataService.GetModelListByType(modelType ?? string.Empty);
                result = ResponseResult<List<AiModelProviderEntity>>.Success(list);
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
        public ResponseResult<AiActivityConfigEntity> GetAxConfig(string processId, string version, string activityId)
        {
            var result = ResponseResult<AiActivityConfigEntity>.Default();
            try
            {
                if (string.IsNullOrWhiteSpace(processId) || string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(activityId))
                {
                    return ResponseResult<AiActivityConfigEntity>.Success(null);
                }
                var entity = _aiModelDataService.GetAiActivityConfigByProcessVersionActivity(processId, version, activityId);

                result = ResponseResult<AiActivityConfigEntity>.Success(entity);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<AiActivityConfigEntity>.Error(LocalizeHelper.GetDesignerMessage("wf2aicontroller.getaxconfig.error", ex.Message));
            }
            return result;
        }

        public ResponseResult DeleteAxConfig(string processId, string version, string activityId)
        {
            var result = ResponseResult.Default();
            try
            {
                _aiModelDataService.DeleteAiActivityConfig(processId, version, activityId);
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

        /// <summary>
        /// Test connection by reading biz_documents table
        /// 测试连接：读取biz_documents表数据
        /// </summary>
        [HttpGet]
        public ResponseResult<List<KbDocumentEntity>> TestBizDocumentsConnection(int limit = 5)
        {
            var result = ResponseResult<List<KbDocumentEntity>>.Default();
            try
            {
                // Get configuration options
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<List<KbDocumentEntity>>.Error("AI configuration options not found");
                    return result;
                }

                // Create service and test connection
                var kbDocumentService = new KbDocumentService(configOptions);
                var documents = kbDocumentService.TestReadBizDocuments(limit);

                result = ResponseResult<List<KbDocumentEntity>>.Success(documents, $"Successfully read {documents.Count} records from biz_documents table");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<KbDocumentEntity>>.Error($"Failed to read biz_documents table: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Get table names from Supabase database
        /// 获取Supabase数据库中的表名称列表
        /// </summary>
        [HttpGet]
        public ResponseResult<List<string>> GetTableNames()
        {
            var result = ResponseResult<List<string>>.Default();
            try
            {
                // Get configuration options
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<List<string>>.Error("AI configuration options not found");
                    return result;
                }

                // Create service and get table names
                var kbDocumentService = new KbDocumentService(configOptions);
                var tableNames = kbDocumentService.GetTableNames();

                result = ResponseResult<List<string>>.Success(tableNames);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<string>>.Error(ex.Message);
            }
            return result;
        }
        #endregion

        #region Knowledge Base 知识库
        /// <summary>
        /// Get all knowledge base documents
        /// 获取所有知识库文档
        /// </summary>
        [HttpGet]
        public ResponseResult<List<KbDocumentEntity>> GetKnowledgeBaseDocuments()
        {
            var result = ResponseResult<List<KbDocumentEntity>>.Default();
            try
            {
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<List<KbDocumentEntity>>.Error("AI configuration options not found");
                    return result;
                }

                var kbDocumentService = new KbDocumentService(configOptions);
                var documents = kbDocumentService.GetAllDocuments();

                result = ResponseResult<List<KbDocumentEntity>>.Success(documents);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<KbDocumentEntity>>.Error($"Failed to get documents: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Save knowledge base document (insert or update)
        /// 保存知识库文档（插入或更新）
        /// </summary>
        [HttpPost]
        public ResponseResult<KbDocumentEntity> SaveKnowledgeBaseDocument([FromBody] KbDocumentEntity entity)
        {
            var result = ResponseResult<KbDocumentEntity>.Default();
            try
            {
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<KbDocumentEntity>.Error("AI configuration options not found");
                    return result;
                }

                var kbDocumentService = new KbDocumentService(configOptions);
                var savedEntity = kbDocumentService.SaveDocument(entity);

                result = ResponseResult<KbDocumentEntity>.Success(savedEntity, "Document saved successfully");
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<KbDocumentEntity>.Error($"Failed to save document: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Search knowledge base documents using match_documents_optimized
        /// 使用match_documents_optimized搜索知识库文档
        /// </summary>
        [HttpPost]
        public ResponseResult<List<KbDocumentEntity>> SearchKnowledgeBase([FromBody] SearchKnowledgeBaseRequest request)
        {
            var result = ResponseResult<List<KbDocumentEntity>>.Default();
            try
            {
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<List<KbDocumentEntity>>.Error("AI configuration options not found");
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.Query))
                {
                    result = ResponseResult<List<KbDocumentEntity>>.Error("Query is required");
                    return result;
                }

                var kbDocumentService = new KbDocumentService(configOptions);
                var documents = kbDocumentService.SearchDocuments(
                    request.Query,
                    request.Threshold > 0 ? request.Threshold : 0.7f,
                    request.Count > 0 ? request.Count : 5
                );

                result = ResponseResult<List<KbDocumentEntity>>.Success(documents);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<List<KbDocumentEntity>>.Error($"Search failed: {ex.Message}");
            }
            return result;
        }

        /// <summary>
        /// Generate metadata JSON from Question, Intent, and Answer
        /// 根据Question、Intent和Answer生成元数据JSON
        /// </summary>
        [HttpPost]
        public ResponseResult<object> GenerateMetadata([FromBody] GenerateMetadataRequest request)
        {
            var result = ResponseResult<object>.Default();
            try
            {
                var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
                if (configOptions == null)
                {
                    result = ResponseResult<object>.Error("AI configuration options not found");
                    return result;
                }

                var kbDocumentService = new KbDocumentService(configOptions);
                var metadata = kbDocumentService.GenerateMetadata(
                    request.Question ?? string.Empty,
                    request.Intent ?? string.Empty,
                    request.Answer ?? string.Empty
                );

                result = ResponseResult<object>.Success(metadata);
            }
            catch (System.Exception ex)
            {
                result = ResponseResult<object>.Error($"Failed to generate metadata: {ex.Message}");
            }
            return result;
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

    #region Request Models 请求模型
    /// <summary>
    /// Search knowledge base request model
    /// 搜索知识库请求模型
    /// </summary>
    public class SearchKnowledgeBaseRequest
    {
        public string Query { get; set; }
        public float Threshold { get; set; } = 0.7f;
        public int Count { get; set; } = 5;
    }

    /// <summary>
    /// Generate metadata request model
    /// 生成元数据请求模型
    /// </summary>
    public class GenerateMetadataRequest
    {
        public string Question { get; set; }
        public string Intent { get; set; }
        public string Answer { get; set; }
    }
    #endregion
}