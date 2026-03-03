using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slickflow.AI.Common;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Implementation;
using Slickflow.AI.Manager;
using Slickflow.WebUtility;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// Multi-turn RAG node service: encapsulates memory_turns and history message list,
    /// separated from the generic AI service; builds context-aware requests and invokes the LLM.
    /// </summary>
    public class RagMultiTurnService
    {
        /// <summary>
        /// Invokes the LLM: uses memory_turns from axConfig and sends history merged with the current turn.
        /// </summary>
        /// <param name="axConfig">AI node config (includes memory_turns, default 10)</param>
        /// <param name="mediaFileList">Current turn input (e.g. user_message and other variables)</param>
        /// <param name="history">History message list, each item role + content (deserialized by caller from chat_history etc.)</param>
        /// <returns>LLM reply text</returns>
        public async Task<string> InvokeWithHistoryAsync(
            AiActivityConfigEntity axConfig,
            IList<MultiMediaFile> mediaFileList,
            IList<ChatHistoryMessage> historyChatMessageList)
        {
            if (axConfig == null)
                throw new ArgumentNullException(nameof(axConfig));
            if (string.IsNullOrWhiteSpace(axConfig.ProcessId) || string.IsNullOrWhiteSpace(axConfig.Version) || string.IsNullOrWhiteSpace(axConfig.ActivityId))
                throw new InvalidOperationException("ProcessId, Version, and ActivityId are required in AxConfig");
            if (!axConfig.ModelProviderId.HasValue)
                throw new InvalidOperationException("ModelProviderId is not set in AxConfig");

            var modelProviderManager = new AiModelProviderManager();
            var modelProvider = modelProviderManager.GetById(axConfig.ModelProviderId.Value);
            if (modelProvider == null)
                throw new InvalidOperationException($"AIModelProvider not found for ModelProviderId: {axConfig.ModelProviderId.Value}");
            if (!modelProvider.IsActive)
                throw new InvalidOperationException("AIModelProvider is not active");
            if (string.IsNullOrWhiteSpace(modelProvider.BaseUrl) || string.IsNullOrWhiteSpace(modelProvider.ApiKey))
                throw new InvalidOperationException("BaseUrl and ApiKey are required in AIModelProvider");
            if (string.IsNullOrWhiteSpace(axConfig.ModelName))
                throw new InvalidOperationException("ModelName is not set in AxConfig");

            var baseUrl = modelProvider.BaseUrl.TrimEnd('/');
            var decryptedApiKey = ApiKeyCryptoHelper.Decrypt(modelProvider.ApiKey, modelProvider.ApiUUID);
            var serviceType = EnumHelper.TryParseEnum<AiServiceTypeEnum>(axConfig.ServiceType);
            var systemPrompt = axConfig.SystemPrompt;
            var userMessage = axConfig.UserMessage ?? string.Empty;
            var memoryTurns = axConfig.MemoryTurns > 0 ? axConfig.MemoryTurns : 10;

            var messages = await LlmChatMessageBuilder.BuildChatMessageContentAsync(
                serviceType,
                mediaFileList,
                systemPrompt,
                userMessage,
                axConfig,
                historyChatMessageList,
                memoryTurns);

            var aiLlmService = AiLlmServiceFactory.CreateLargeModelServcie(modelProvider.ModelProvider);
            var response = await aiLlmService.InvokeAIChatServiceWithMessagesAsync(baseUrl, decryptedApiKey, messages, axConfig);

            if (response?.Status == 1 && response.Entity != null)
                return response.Entity.Content ?? string.Empty;
            return string.Empty;
        }
    }
}
