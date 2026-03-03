using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// Inserts document usage log records into biz_document_usage_log via Supabase REST API.
    /// Uses AiAppConfigProviderOptions (SupabaseProjectUrl, SupabaseServiceRoleKey).
    /// </summary>
    public class DocumentUsageLogRepository
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public DocumentUsageLogRepository(AiAppConfigProviderOptions configOptions)
        {
            if (configOptions == null)
                throw new ArgumentNullException(nameof(configOptions));
            if (string.IsNullOrWhiteSpace(configOptions.SupabaseProjectUrl))
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            if (string.IsNullOrWhiteSpace(configOptions.SupabaseServiceRoleKey))
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");

            _baseUrl = configOptions.SupabaseProjectUrl.TrimEnd('/');
            _apiKey = configOptions.SupabaseServiceRoleKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Inserts a document usage log record. Fire-and-forget safe; catches and logs errors.
        /// </summary>
        public async Task InsertAsync(BizDocumentUsageLogEntity entity)
        {
            var payload = new Dictionary<string, object>
            {
                ["document_id"] = entity.DocumentId,
                ["similarity"] = entity.Similarity ?? 0.0
            };
            if (entity.IndustryId.HasValue)
                payload["industry_id"] = entity.IndustryId.Value;
            if (!string.IsNullOrEmpty(entity.MessageId))
                payload["message_id"] = entity.MessageId;
            if (!string.IsNullOrEmpty(entity.CustomerId))
                payload["customer_id"] = entity.CustomerId;
            if (!string.IsNullOrEmpty(entity.SessionId))
                payload["session_id"] = entity.SessionId;
            if (!string.IsNullOrEmpty(entity.QueryText))
                payload["query_text"] = entity.QueryText;
            if (entity.UsageContext != null)
                payload["usage_context"] = entity.UsageContext;

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/biz_document_usage_log") { Content = content };
            request.Headers.Add("Prefer", "return=minimal");

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw new InvalidOperationException(
                    $"DocumentUsageLog insert failed: {response.StatusCode} - {responseJson}. Ensure biz_document_usage_log table exists (run docs/database/biz_document_usage_log.sql).");
            }
        }

        /// <summary>
        /// Gets usage logs by document_id (for maintenance/analytics).
        /// </summary>
        public async Task<List<BizDocumentUsageLogEntity>> GetByDocumentIdAsync(long documentId, int limit = 100)
        {
            var url = $"/rest/v1/biz_document_usage_log?document_id=eq.{documentId}&order=created_at.desc&limit={limit}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"DocumentUsageLog query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizDocumentUsageLogEntity>()
                : JsonConvert.DeserializeObject<List<BizDocumentUsageLogEntity>>(json) ?? new List<BizDocumentUsageLogEntity>();
        }

        /// <summary>
        /// Gets usage logs by industry_id (for maintenance/analytics).
        /// </summary>
        public async Task<List<BizDocumentUsageLogEntity>> GetByIndustryIdAsync(long industryId, int limit = 100)
        {
            var url = $"/rest/v1/biz_document_usage_log?industry_id=eq.{industryId}&order=created_at.desc&limit={limit}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"DocumentUsageLog query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizDocumentUsageLogEntity>()
                : JsonConvert.DeserializeObject<List<BizDocumentUsageLogEntity>>(json) ?? new List<BizDocumentUsageLogEntity>();
        }
    }
}
