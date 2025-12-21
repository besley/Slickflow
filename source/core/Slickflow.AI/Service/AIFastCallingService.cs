using Slickflow.AI.Entity;
using Slickflow.AI.Implementation;
using Slickflow.AI.Manager;
using Slickflow.AI.Configuration;
using Slickflow.WebUtility;

namespace Slickflow.AI.Service
{
    public class AiFastCallingService : IAiFastCallingService
    {
        public AiFastCallingService()
        {
        }

        #region Calling AI Service, mainly include LLLM
        public async Task<string> InvokeAIServiceAsync(AiActivityConfigEntity axConfig, IList<MultiMediaFile> mediaFileList)
        {
            if (axConfig == null)
            {
                throw new ArgumentNullException(nameof(axConfig), "configInfo cannot be null");
            }

            // 1. 验证 AxConfig 基本配置
            if (string.IsNullOrWhiteSpace(axConfig.ConfigUUID))
            {
                throw new InvalidOperationException("ConfigUUID is not set in AxConfig");
            }

            // 2. 根据 model_provider_id 查询 ai_model_provider 表获取 baseUrl 和 apiKey
            if (!axConfig.ModelProviderId.HasValue)
            {
                throw new InvalidOperationException($"ModelProviderId is not set in AxConfig for configUUID: {axConfig.ConfigUUID}");
            }

            // 3. 获取模型提供者配置
            var modelProviderManager = new AiModelProviderManager();
            var modelProvider = modelProviderManager.GetById(axConfig.ModelProviderId.Value);

            if (modelProvider == null)
            {
                throw new InvalidOperationException($"AIModelProvider not found for ModelProviderId: {axConfig.ModelProviderId.Value}");
            }

            if (!modelProvider.IsActive)
            {
                throw new InvalidOperationException($"AIModelProvider with Id {axConfig.ModelProviderId.Value} is not active");
            }

            if (string.IsNullOrWhiteSpace(modelProvider.BaseUrl))
            {
                throw new InvalidOperationException($"BaseUrl is not set in AIModelProvider for Id: {axConfig.ModelProviderId.Value}");
            }

            if (string.IsNullOrWhiteSpace(modelProvider.ApiKey))
            {
                throw new InvalidOperationException($"ApiKey is not set in AIModelProvider for Id: {axConfig.ModelProviderId.Value}");
            }

            if (string.IsNullOrEmpty(axConfig.ModelName))
            {
                throw new InvalidOperationException("ModelName is not set in AxConfig");
            }

            var aiLargeModelService = AiLlmServiceFactory.CreateLargeModelServcie(modelProvider.ModelProvider);

            var baseUrl = modelProvider.BaseUrl;
            var decryptedApiKey = ApiKeyCryptoHelper.Decrypt(modelProvider.ApiKey, modelProvider.ApiUUID);       //get decrypt api key
            var modelName = axConfig.ModelName;
            var systemPrompt = axConfig.SystemPrompt;
            var userMessage = axConfig.UserMessage;
            var temperature = axConfig.Temperature;
            var maxTokens = axConfig.MaxTokens;
            var timeout = axConfig.Timeout;

            //using decryptedApiKey
            var response = await aiLargeModelService.InvokeAIChatServiceAsync(baseUrl, decryptedApiKey, modelName, systemPrompt, userMessage,
                mediaFileList, temperature, maxTokens, timeout);

            if (response.Status == 1)
            {
                return response.Entity.Content;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<string> TestModelConnectionAsync(string baseUrl, string apiUUID, string apiKey, string modelProvider)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("BaseUrl cannot be null or empty", nameof(baseUrl));
            }

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentException("ApiKey cannot be null or empty", nameof(apiKey));
            }

            if (string.IsNullOrWhiteSpace(modelProvider))
            {
                throw new ArgumentException("ModelName cannot be null or empty", nameof(modelProvider));
            }

            var decryptedApiKey = ApiKeyCryptoHelper.Decrypt(apiKey, apiUUID);
            var aiChatService = AiLlmServiceFactory.CreateLargeModelServcieTesting(modelProvider, out string modelName);

            //using decryptedApiKey
            var response = await aiChatService.TestConnectionAsync(baseUrl, decryptedApiKey, modelName);

            return response;
        }
        #endregion
    }
}
