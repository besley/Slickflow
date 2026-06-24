using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slickflow.Module.External.Customer.Entity;

namespace Slickflow.Module.External.Data
{
    /// <summary>
    /// Inserts and queries biz_conversation table in Supabase via REST API.
    /// URL and API key from SupabaseConfig (host appsettings.json AiModelProvider or env).
    /// </summary>
    public class SupabaseConversationRepository
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public SupabaseConversationRepository(string baseUrl = null, string apiKey = null)
        {
            _baseUrl = (baseUrl ?? SupabaseConfig.Url)?.TrimEnd('/');
            _apiKey = apiKey ?? SupabaseConfig.ApiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl ?? "http://localhost");
            _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Inserts a conversation record; returns the entity with conversation_id and created_at when Supabase returns representation.
        /// Sends only explicit columns so tables without optional columns still work.
        /// </summary>
        public async Task<BizConversationEntity> InsertAsync(BizConversationEntity entity)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            if (string.IsNullOrWhiteSpace(entity.ConversationId))
                entity.ConversationId = "conv-" + Guid.NewGuid().ToString("N");
            entity.CreatedAt = DateTime.UtcNow;
            // Build payload to match your table. Table may have message_id (required), user_message, ai_response, created_at, customer_id.
            var messageId = entity.ConversationId;
            if (string.IsNullOrWhiteSpace(messageId))
                messageId = "msg-" + Guid.NewGuid().ToString("N");
            var payload = new Dictionary<string, object>
            {
                ["message_id"] = messageId,
                ["user_message"] = entity.UserMessage ?? "",
                ["ai_response"] = entity.AiResponse ?? "",
                ["created_at"] = entity.CreatedAt
            };
            if (!string.IsNullOrEmpty(entity.CustomerId))
                payload["customer_id"] = entity.CustomerId;
            if (!string.IsNullOrEmpty(entity.SessionId))
                payload["session_id"] = entity.SessionId;
            if (entity.IndustryId.HasValue)
                payload["industry_id"] = entity.IndustryId.Value;
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/biz_conversation") { Content = content };
            request.Headers.Add("Prefer", "return=representation");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase insert failed: {response.StatusCode} - {responseJson}");
            var list = JsonConvert.DeserializeObject<List<BizConversationEntity>>(responseJson ?? "[]");
            return list?.Count > 0 ? list[0] : entity;
        }

        /// <summary>
        /// Gets conversation by conversation_id.
        /// </summary>
        public async Task<BizConversationEntity> GetByIdAsync(string conversationId)
        {
            if (string.IsNullOrWhiteSpace(conversationId)) return null;
            var list = await GetByFilterAsync("conversation_id", conversationId).ConfigureAwait(false);
            return list?.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Gets all conversation rows for a process instance (order by created_at ascending).
        /// </summary>
        public async Task<List<BizConversationEntity>> GetByProcessInstanceIdAsync(string processInstanceId)
        {
            if (string.IsNullOrWhiteSpace(processInstanceId)) return new List<BizConversationEntity>();
            var list = await GetByFilterAsync("process_instance_id", processInstanceId, "created_at", "asc").ConfigureAwait(false);
            return list ?? new List<BizConversationEntity>();
        }

        /// <summary>
        /// Gets conversation rows for a customer (order by created_at descending).
        /// </summary>
        public async Task<List<BizConversationEntity>> GetByCustomerIdAsync(string customerId, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return new List<BizConversationEntity>();
            var list = await GetByFilterAsync("customer_id", customerId, "created_at", "desc", limit).ConfigureAwait(false);
            return list ?? new List<BizConversationEntity>();
        }

        /// <summary>
        /// Gets conversation rows for a customer within the last N days (created_at &gt;= sinceUtc), order by created_at descending.
        /// </summary>
        public async Task<List<BizConversationEntity>> GetByCustomerIdSinceAsync(string customerId, DateTime sinceUtc, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return new List<BizConversationEntity>();
            var list = await GetByCustomerIdAndSinceAsync(customerId, sinceUtc, limit).ConfigureAwait(false);
            return list ?? new List<BizConversationEntity>();
        }

        /// <summary>
        /// Gets conversation rows for a customer and session (order by created_at descending).
        /// Ensures only the current session's history is loaded for multi-turn context.
        /// </summary>
        public async Task<List<BizConversationEntity>> GetByCustomerIdAndSessionIdAsync(string customerId, string sessionId, int limit = 100)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return new List<BizConversationEntity>();
            if (string.IsNullOrWhiteSpace(sessionId)) return new List<BizConversationEntity>();
            var list = await GetByCustomerIdAndSessionIdInternalAsync(customerId.Trim(), sessionId.Trim(), limit).ConfigureAwait(false);
            return list ?? new List<BizConversationEntity>();
        }

        private async Task<List<BizConversationEntity>> GetByFilterAsync(string column, string value, string orderBy = null, string orderDir = "asc", int? limit = null)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            var url = $"/rest/v1/biz_conversation?{column}=eq.{Uri.EscapeDataString(value)}";
            if (!string.IsNullOrWhiteSpace(orderBy))
                url += $"&order={orderBy}.{orderDir}";
            if (limit.HasValue)
                url += $"&limit={limit.Value}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizConversationEntity>()
                : JsonConvert.DeserializeObject<List<BizConversationEntity>>(json);
        }

        private async Task<List<BizConversationEntity>> GetByCustomerIdAndSinceAsync(string customerId, DateTime sinceUtc, int limit)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            // PostgREST: customer_id=eq.xxx & created_at=gte.iso_timestamp
            var sinceIso = sinceUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture);
            var url = $"/rest/v1/biz_conversation?customer_id=eq.{Uri.EscapeDataString(customerId)}&created_at=gte.{Uri.EscapeDataString(sinceIso)}&order=created_at.desc&limit={limit}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizConversationEntity>()
                : JsonConvert.DeserializeObject<List<BizConversationEntity>>(json);
        }

        private async Task<List<BizConversationEntity>> GetByCustomerIdAndSessionIdInternalAsync(string customerId, string sessionId, int limit)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            // PostgREST: customer_id=eq.xxx & session_id=eq.yyy
            var url = $"/rest/v1/biz_conversation?customer_id=eq.{Uri.EscapeDataString(customerId)}&session_id=eq.{Uri.EscapeDataString(sessionId)}&order=created_at.desc&limit={limit}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizConversationEntity>()
                : JsonConvert.DeserializeObject<List<BizConversationEntity>>(json);
        }
    }
}
