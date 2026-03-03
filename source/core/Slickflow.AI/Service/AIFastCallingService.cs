using Slickflow.AI.Entity;
using Slickflow.AI.Implementation;
using Slickflow.AI.Manager;
using Slickflow.AI.Configuration;
using Slickflow.AI.Common;
using Slickflow.WebUtility;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// AI 大模型调用服务。严格区分两种配置来源：
    /// 1) InvokeAIServiceAsync：工作流节点执行，从 ai_activity_config → ai_model_provider 读取
    /// 2) InvokeWithLocalConfigAsync：文字生成工作流等，从 appsettings (AiModelProvider:OpenAI/QianWen) 读取
    /// </summary>
    public class AiFastCallingService : IAiFastCallingService
    {
        public AiFastCallingService()
        {
        }

        #region 工作流节点执行 - 从 ai_activity_config、ai_model_provider 读取
        /// <summary>
        /// 工作流节点执行：BaseUrl、ApiKey 等仅从 ai_activity_config → ai_model_provider 读取，不使用 AiAppConfiguration
        /// </summary>
        public async Task<string> InvokeAIServiceAsync(AiActivityConfigEntity axConfig, IList<MultiMediaFile> mediaFileList)
        {
            if (axConfig == null)
            {
                throw new ArgumentNullException(nameof(axConfig), "configInfo cannot be null");
            }

            // Verify the basic configuration of AxConfig
            if (string.IsNullOrWhiteSpace(axConfig.ProcessId) || string.IsNullOrWhiteSpace(axConfig.Version) || string.IsNullOrWhiteSpace(axConfig.ActivityId))
            {
                throw new InvalidOperationException("ProcessId, Version, and ActivityId are required in AxConfig");
            }

            // Query the ai_model_provider table according to model_provider_id to obtain the baseUrl and apiKey
            if (!axConfig.ModelProviderId.HasValue)
            {
                throw new InvalidOperationException($"ModelProviderId is not set in AxConfig for ProcessId: {axConfig.ProcessId}, Version: {axConfig.Version}, ActivityId: {axConfig.ActivityId}");
            }

            // Get model provider configuration
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
            var response = await aiLargeModelService.InvokeAIChatServiceAsync(baseUrl, decryptedApiKey, mediaFileList, axConfig);

            if (response.Status == 1)
            {
                return response.Entity.Content;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 本地配置场景 - 从 appsettings (AiAppConfiguration) 读取
        /// <summary>
        /// 文字生成工作流等场景：BaseUrl、Endpoint 从 appsettings AiModelProvider:OpenAI 或 QianWen 读取，不使用 ai_activity_config
        /// </summary>
        public async Task<string> InvokeWithLocalConfigAsync(string userMessage, string systemPrompt = null, string provider = "OpenAI")
        {
            var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions ?? throw new InvalidOperationException("AiAppConfiguration 未配置，请检查 appsettings.json AiModelProvider 节。");
            string apiUrl;
            string apiKey;
            string modelName;
            IAiLlmService aiLlmService;

            if (string.Equals(provider, "QianWen", StringComparison.OrdinalIgnoreCase))
            {
                var qw = configOptions.QianWen ?? throw new InvalidOperationException("AiModelProvider:QianWen 未配置。");
                if (string.IsNullOrWhiteSpace(qw.ApiKey) || string.IsNullOrWhiteSpace(qw.BaseUrl) || string.IsNullOrWhiteSpace(qw.Endpoint))
                    throw new InvalidOperationException("QianWen 需配置 ApiKey、BaseUrl、Endpoint。");
                apiUrl = qw.ChatApiUrl;
                apiKey = qw.ApiKey;
                modelName = qw.Model ?? "gpt-4o";
                aiLlmService = AiLlmServiceFactory.CreateLargeModelServcie("QianWen");
            }
            else
            {
                var openAi = configOptions.OpenAI ?? throw new InvalidOperationException("AiModelProvider:OpenAI 未配置。");
                if (string.IsNullOrWhiteSpace(openAi.ApiKey) || string.IsNullOrWhiteSpace(openAi.BaseUrl) || string.IsNullOrWhiteSpace(openAi.ApiUrl))
                    throw new InvalidOperationException("OpenAI 需配置 ApiKey、BaseUrl、ApiUrl。");
                apiUrl = openAi.ChatApiUrl;
                apiKey = openAi.ApiKey;
                modelName = openAi.Model ?? "gpt-4o";
                aiLlmService = AiLlmServiceFactory.CreateLargeModelServcie("OpenAI");
            }

            var messages = new List<CustomApiMessage>();
            if (!string.IsNullOrWhiteSpace(systemPrompt))
                messages.Add(new CustomApiMessage { Role = "system", Content = systemPrompt });
            messages.Add(new CustomApiMessage { Role = "user", Content = userMessage });

            var axConfig = new AiActivityConfigEntity
            {
                ModelName = modelName,
                Temperature = 0.3m,
                MaxTokens = 2048,
                Timeout = 60
            };

            var response = await aiLlmService.InvokeAIChatServiceWithMessagesAsync(apiUrl, apiKey, messages, axConfig);
            if (response?.Status == 1 && response.Entity != null)
                return response.Entity.Content ?? string.Empty;
            return string.Empty;
        }
        #endregion

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
    }
}
