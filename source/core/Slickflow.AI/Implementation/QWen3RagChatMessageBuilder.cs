using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Manager;
using Slickflow.AI.Service;
using Slickflow.WebUtility;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// RAG chat message builder using QWen3-Embedding for vector generation.
    /// 使用 QWen3-Embedding 生成向量的 RAG 消息构建器
    /// 配置仅从 ai_activity_config (axConfig) 及 ai_model_provider 读取，不使用本地 AiAppConfiguration
    /// </summary>
    internal class QWen3RagChatMessageBuilder
    {
        /// <summary>
        /// Build RAG chat message content with QWen3-Embedding for document retrieval.
        /// 使用 QWen3-Embedding 构建 RAG 消息内容。
        /// 配置仅从节点 ai_activity_config 和 ai_model_provider 读取，不读取本地 AiAppConfiguration。
        /// </summary>
        internal static async Task<IList<CustomApiMessage>> BuildRagChatMessageContentAsync(
            IList<MultiMediaFile> mediaFileList,
            string systemPrompt,
            string userMessage,
            AiActivityConfigEntity axConfig,
            IList<ChatHistoryMessage> historyChatMessageList = null,
            int memoryTurns = 10)
        {
            if (axConfig?.RagEmbeddingModelId == null || !axConfig.RagEmbeddingModelId.HasValue)
                throw new InvalidOperationException("QWen3 RAG 需要在节点配置 ai_activity_config 中设置 RagEmbeddingModelId，指向 ai_model_provider 中的向量模型 (model_type=vector_model)。");

            var modelManager = new AiModelProviderManager();
            var provider = modelManager.GetById(axConfig.RagEmbeddingModelId.Value);
            if (provider == null)
                throw new InvalidOperationException($"ai_model_provider 中未找到 Id={axConfig.RagEmbeddingModelId.Value} 的向量模型配置。");
            if (!provider.IsActive)
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 已被禁用，请在节点配置中启用。");

            if (string.IsNullOrWhiteSpace(provider.BaseUrl))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 BaseUrl 不能为空。");
            if (string.IsNullOrWhiteSpace(provider.ApiKey))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 ApiKey 不能为空。");
            if (string.IsNullOrWhiteSpace(provider.ModelName))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 ModelName 不能为空。");

            var decryptedKey = ApiKeyCryptoHelper.Decrypt(provider.ApiKey, provider.ApiUUID);
            var apiUrl = provider.BaseUrl.TrimEnd('/');

            var dimensions = (axConfig.RagEmbeddingDimensions ?? 0) > 0 ? axConfig.RagEmbeddingDimensions.Value : 1536;
            var embeddingGenerator = new QWen3EmbeddingGenerator(decryptedKey, apiUrl, provider.ModelName, dimensions);

            return await RagMessageBuilderCore.BuildRagChatMessageContentAsync(
                embeddingGenerator,
                mediaFileList,
                systemPrompt,
                userMessage,
                axConfig,
                historyChatMessageList,
                memoryTurns);
        }
    }
}
