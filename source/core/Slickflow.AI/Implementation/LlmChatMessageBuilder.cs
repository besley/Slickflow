using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Slickflow.AI.Common;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Manager;
using Slickflow.AI.Service;

namespace Slickflow.AI.Implementation
{
    internal class LlmChatMessageBuilder
    {
        /// <summary>
        /// Build message content by serviceType.
        /// LLM: Use existing multimodal/text messages as-is.
        /// RAG: Call SupabaseRagService for vector search first, then prepend retrieved documents as context to userMessage.
        /// When history is non-empty and service is RAG, insert the last memoryTurns rounds of history between system and current user message.
        /// </summary>
        internal static async Task<IList<CustomApiMessage>> BuildChatMessageContentAsync(
            AiServiceTypeEnum serviceType,
            IList<MultiMediaFile> mediaFileList,
            string systemPrompt,
            string userMessage,
            AiActivityConfigEntity axConfig,
            IList<ChatHistoryMessage> historyChatMessageList = null,
            int? memoryTurns = null)
        {
            if (serviceType == AiServiceTypeEnum.RAG)
            {
                if (axConfig?.RagEmbeddingModelId == null || !axConfig.RagEmbeddingModelId.HasValue)
                    throw new InvalidOperationException("RAG 节点必须在 ai_activity_config 中设置 RagEmbeddingModelId，指向 ai_model_provider 中的向量模型 (model_type=vector_model)。");

                var modelManager = new AiModelProviderManager();
                var embeddingProvider = modelManager.GetById(axConfig.RagEmbeddingModelId.Value);
                if (embeddingProvider == null)
                    throw new InvalidOperationException($"ai_model_provider 中未找到 Id={axConfig.RagEmbeddingModelId.Value} 的向量模型配置。");
                if (!embeddingProvider.IsActive)
                    throw new InvalidOperationException($"ai_model_provider Id={embeddingProvider.Id} 已被禁用，请在节点配置中启用。");

                var embeddingGenerator = CreateEmbeddingGeneratorFromProvider(embeddingProvider, axConfig);
                var turns = memoryTurns ?? axConfig?.MemoryTurns ?? 10;

                return await RagMessageBuilderCore.BuildRagChatMessageContentAsync(
                    embeddingGenerator, mediaFileList, systemPrompt, userMessage, axConfig, historyChatMessageList, turns);
            }

            return BuildLlmChatMessageContent(mediaFileList, systemPrompt, userMessage);
        }

        /// <summary>
        /// 从 ai_model_provider 创建 embedding 生成器，仅使用节点配置，不读取 AiAppConfiguration
        /// </summary>
        private static IEmbeddingGenerator CreateEmbeddingGeneratorFromProvider(AiModelProviderEntity provider, AiActivityConfigEntity axConfig)
        {
            if (string.IsNullOrWhiteSpace(provider.BaseUrl))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 BaseUrl 不能为空。");
            if (string.IsNullOrWhiteSpace(provider.ApiKey))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 ApiKey 不能为空。");
            if (string.IsNullOrWhiteSpace(provider.ModelName))
                throw new InvalidOperationException($"ai_model_provider Id={provider.Id} 的 ModelName 不能为空。");

            var decryptedKey = ApiKeyCryptoHelper.Decrypt(provider.ApiKey, provider.ApiUUID);
            var apiUrl = provider.BaseUrl.TrimEnd('/');
            var dimensions = (axConfig?.RagEmbeddingDimensions ?? 0) > 0 ? axConfig.RagEmbeddingDimensions.Value : 1536;

            var isQwen = !string.IsNullOrWhiteSpace(provider.ModelProvider) &&
                provider.ModelProvider.IndexOf("qwen", StringComparison.OrdinalIgnoreCase) >= 0;

            if (isQwen)
            {
                return new QWen3EmbeddingGenerator(decryptedKey, apiUrl, provider.ModelName, dimensions);
            }
            return new OpenAiEmbeddingGenerator(decryptedKey, apiUrl, provider.ModelName);
        }

        /// <summary>
        /// Original LLM message construction logic (unchanged).
        /// </summary>
        internal static IList<CustomApiMessage> BuildLlmChatMessageContent(
            IList<MultiMediaFile> mediaFileList,
            string systemPrompt,
            string userMessage)
        {
            // Prepare request message list
            var messages = new List<CustomApiMessage>();

            // Add system prompt when present
            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                messages.Add(new CustomApiMessage
                {
                    Role = "system",
                    Content = systemPrompt
                });
            }

            // Build multimodal message content (image + text). OpenAI format: text first, then image_url
            var imageFileData = (mediaFileList != null && mediaFileList.Count > 0)
                ? mediaFileList[0]
                : null;

            if (imageFileData != null && imageFileData.MeidaType == MultiMediaTypeEnum.Image)
            {
                // Sanitize base64: strip data: prefix if present, keep raw base64 only
                var rawBase64 = imageFileData.Content ?? string.Empty;
                if (rawBase64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    var commaIndex = rawBase64.IndexOf(',');
                    if (commaIndex > 0 && commaIndex < rawBase64.Length - 1)
                    {
                        rawBase64 = rawBase64.Substring(commaIndex + 1);
                    }
                }

                // Infer MIME type from file name or media type (avoid 400 by not forcing all images to image/jpeg)
                var fileName = imageFileData.Name?.ToLowerInvariant() ?? string.Empty;
                var mimeType = "image/jpeg";   // Default to jpeg

                if (fileName.EndsWith(".png"))
                    mimeType = "image/png";
                else if (fileName.EndsWith(".gif"))
                    mimeType = "image/gif";
                else if (fileName.EndsWith(".webp"))
                    mimeType = "image/webp";
                else if (fileName.EndsWith(".bmp"))
                    mimeType = "image/bmp";

                // If Name has no extension, fallback by MultiMediaTypeEnum (reserved for extension). Only image type is supported for vision model.
                var imageUrl = $"data:{mimeType};base64,{rawBase64}";

                var multiModalContent = new List<object>
                    {
                        new { type = "text", text = userMessage },
                        new {
                            type = "image_url",
                            image_url = new {
                                url = imageUrl,
                                detail = "auto"
                            }
                        }
                    };

                messages.Add(new CustomApiMessage
                {
                    Role = "user",
                    Content = multiModalContent
                });
            }
            else
            {
                messages.Add(new CustomApiMessage
                {
                    Role = "user",
                    Content = userMessage
                });
            }
            return messages;
        }

    }
}
