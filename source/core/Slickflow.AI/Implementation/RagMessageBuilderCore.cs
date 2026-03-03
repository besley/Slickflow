using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Service;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// Shared RAG message construction logic, used by OpenAI and QWen3 implementations.
    /// 共享的 RAG 消息构建逻辑，供 OpenAI 与 QWen3 实现共用
    /// </summary>
    internal static class RagMessageBuilderCore
    {
        /// <summary>
        /// Build RAG chat message content using the provided embedding generator.
        /// 使用指定的 embedding 生成器构建 RAG 消息内容
        /// </summary>
        internal static async Task<IList<CustomApiMessage>> BuildRagChatMessageContentAsync(
            IEmbeddingGenerator embeddingGenerator,
            IList<MultiMediaFile> mediaFileList,
            string systemPrompt,
            string userMessage,
            AiActivityConfigEntity axConfig,
            IList<ChatHistoryMessage> historyChatMessageList = null,
            int memoryTurns = 10)
        {
            var messages = new List<CustomApiMessage>();

            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                messages.Add(new CustomApiMessage { Role = "system", Content = systemPrompt });
            }

            if (historyChatMessageList != null && historyChatMessageList.Count > 0 && memoryTurns > 0)
            {
                int takeCount = Math.Min(memoryTurns * 2, historyChatMessageList.Count);
                var recent = historyChatMessageList.Count > takeCount
                    ? historyChatMessageList.Skip(historyChatMessageList.Count - takeCount).ToList()
                    : historyChatMessageList;
                foreach (var msg in recent)
                {
                    if (string.IsNullOrWhiteSpace(msg?.Role) || msg.Content == null) continue;
                    var role = msg.Role.Trim().ToLowerInvariant();
                    if (role != "user" && role != "assistant" && role != "system") continue;
                    messages.Add(new CustomApiMessage { Role = role, Content = msg.Content });
                }
            }

            string actualUserQuestion = userMessage;
            if (mediaFileList != null && mediaFileList.Count > 0 && !string.IsNullOrWhiteSpace(mediaFileList[0]?.Content))
            {
                actualUserQuestion = mediaFileList[0].Content;
            }

            var configOptions = ApiKeyCryptoHelper.AiAppConfigOptions;
            var ragService = new SupabaseRagService(configOptions);

            float[] embedding = null;
            try
            {
                embedding = await embeddingGenerator.GenerateEmbeddingAsync(actualUserQuestion);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"RagMessageBuilderCore: embedding generation failed, falling back to no-context mode. {ex}");
            }

            var sb = new StringBuilder();
            sb.AppendLine("You are a RAG assistant. Use the following context to answer the user's question.");
            sb.AppendLine();

            var contactBlock = BuildAlreadyCollectedContactBlock(mediaFileList);
            if (!string.IsNullOrWhiteSpace(contactBlock))
            {
                sb.AppendLine(contactBlock);
                sb.AppendLine();
            }

            sb.AppendLine("Context:");

            if (embedding != null)
            {
                var highThreshold = (float)(axConfig?.RagSimilarityThreshold ?? 0.4m);
                var highCount = axConfig?.RagSearchCount ?? 3;
                var lowThreshold = 0.4f;
                var lowCount = 2;

                long? industryId = TryGetIndustryIdFromMediaFiles(mediaFileList);
                if (!industryId.HasValue && mediaFileList != null && mediaFileList.Count > 0)
                    System.Diagnostics.Debug.WriteLine("[RagMessageBuilderCore] industry_id not in mediaFileList; RAG search will not filter by industry.");

                var searchResults = await ragService.MatchDocumentsOptimizedAsync(
                    embedding,
                    highThreshold,
                    lowThreshold,
                    highCount,
                    lowCount,
                    industryId);

                // Fire-and-forget: log matched documents to biz_document_usage_log (async, non-blocking)
                LogDocumentUsageFireAndForget(searchResults, industryId, actualUserQuestion, mediaFileList, configOptions);

                int index = 1;
                foreach (var doc in searchResults)
                {
                    sb.AppendLine($"[Document {index}] (similarity: {doc.Similarity:F3})");
                    if (!string.IsNullOrWhiteSpace(doc.Content))
                        sb.AppendLine(doc.Content);
                    sb.AppendLine();
                    index++;
                }
            }

            if (!string.IsNullOrWhiteSpace(userMessage))
            {
                sb.AppendLine("Configured User Message:");
                sb.AppendLine(userMessage);
                sb.AppendLine();
            }

            sb.AppendLine("User Question:");
            sb.AppendLine(actualUserQuestion);

            messages.Add(new CustomApiMessage { Role = "user", Content = sb.ToString() });
            return messages;
        }

        private static string BuildAlreadyCollectedContactBlock(IList<MultiMediaFile> mediaFileList)
        {
            if (mediaFileList == null || mediaFileList.Count == 0) return null;
            var customerItem = mediaFileList.FirstOrDefault(m => string.Equals(m?.Name, "customer", StringComparison.OrdinalIgnoreCase));
            if (customerItem?.Content == null) return null;
            var json = customerItem.Content.Trim();
            if (string.IsNullOrWhiteSpace(json)) return null;

            string name = null, phoneNumber = null, mobile = null, wechat = null, email = null;
            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                name = GetString(root, "name");
                phoneNumber = GetString(root, "phone_number");
                mobile = GetString(root, "mobile");
                wechat = GetString(root, "wechat");
                email = GetString(root, "email");
                if (string.IsNullOrWhiteSpace(phoneNumber)) phoneNumber = GetString(root, "phoneNumber");
            }
            catch { return null; }

            var collected = new List<string>();
            if (!string.IsNullOrWhiteSpace(name)) collected.Add($"Name: {name}");
            if (!string.IsNullOrWhiteSpace(wechat)) collected.Add($"WeChat: {wechat}");
            if (!string.IsNullOrWhiteSpace(mobile)) collected.Add($"Mobile: {mobile}");
            if (!string.IsNullOrWhiteSpace(phoneNumber)) collected.Add($"Phone: {phoneNumber}");
            if (!string.IsNullOrWhiteSpace(email)) collected.Add($"Email: {email}");
            if (collected.Count == 0) return null;

            var need = new List<string>();
            if (string.IsNullOrWhiteSpace(name)) need.Add("name/salutation");
            if (string.IsNullOrWhiteSpace(wechat)) need.Add("WeChat ID");
            if (string.IsNullOrWhiteSpace(mobile)) need.Add("mobile number");
            if (string.IsNullOrWhiteSpace(phoneNumber)) need.Add("phone number");
            if (string.IsNullOrWhiteSpace(email)) need.Add("email");

            var sb = new StringBuilder();
            sb.AppendLine("[Already collected contact info (do not ask again)]");
            sb.AppendLine(string.Join("; ", collected));
            sb.AppendLine();
            if (need.Count > 0)
            {
                sb.AppendLine("[Still need to collect (ask politely for only these, one at a time)]");
                sb.AppendLine(string.Join(", ", need));
            }
            else
            {
                sb.AppendLine("[Still need to collect] None.");
            }
            return sb.ToString();
        }

        private static string GetString(JsonElement root, string propertyName)
        {
            if (root.TryGetProperty(propertyName, out var prop))
            {
                var s = prop.GetString();
                return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
            }
            return null;
        }

        /// <summary>
        /// Fire-and-forget logging of matched documents to biz_document_usage_log.
        /// Does not block or throw; errors are logged to Debug.
        /// </summary>
        private static void LogDocumentUsageFireAndForget(
            IList<MatchDocumentResult> searchResults,
            long? industryId,
            string queryText,
            IList<MultiMediaFile> mediaFileList,
            AiAppConfigProviderOptions configOptions)
        {
            if (searchResults == null || searchResults.Count == 0) return;
            if (string.IsNullOrWhiteSpace(configOptions?.SupabaseProjectUrl) || string.IsNullOrWhiteSpace(configOptions?.SupabaseServiceRoleKey))
                return;

            var customerId = TryGetValueFromMediaFiles(mediaFileList, "customer_id");
            var sessionId = TryGetValueFromMediaFiles(mediaFileList, "session_id");

            _ = Task.Run(async () =>
            {
                try
                {
                    var repo = new DocumentUsageLogRepository(configOptions);
                    foreach (var doc in searchResults)
                    {
                        try
                        {
                            await repo.InsertAsync(new BizDocumentUsageLogEntity
                            {
                                DocumentId = doc.Id,
                                IndustryId = industryId,
                                CustomerId = customerId,
                                SessionId = sessionId,
                                Similarity = doc.Similarity,
                                QueryText = queryText
                            }).ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[RagMessageBuilderCore] DocumentUsageLog insert failed for doc {doc.Id}: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[RagMessageBuilderCore] DocumentUsageLog fire-and-forget failed: {ex.Message}");
                }
            });
        }

        private static string TryGetValueFromMediaFiles(IList<MultiMediaFile> mediaFileList, string name)
        {
            if (mediaFileList == null || mediaFileList.Count == 0) return null;
            var item = mediaFileList.FirstOrDefault(m => string.Equals(m?.Name, name, StringComparison.OrdinalIgnoreCase));
            var s = item?.Content?.Trim();
            return string.IsNullOrWhiteSpace(s) ? null : s;
        }

        /// <summary>
        /// Extract industry_id from mediaFileList (from workflow variables) for RAG similarity search filtering.
        /// </summary>
        private static long? TryGetIndustryIdFromMediaFiles(IList<MultiMediaFile> mediaFileList)
        {
            if (mediaFileList == null || mediaFileList.Count == 0) return null;
            var item = mediaFileList.FirstOrDefault(m => string.Equals(m?.Name, "industry_id", StringComparison.OrdinalIgnoreCase));
            if (item?.Content == null) return null;
            var s = item.Content.Trim();
            if (string.IsNullOrWhiteSpace(s)) return null;
            return long.TryParse(s, out var id) ? id : (long?)null;
        }
    }
}
