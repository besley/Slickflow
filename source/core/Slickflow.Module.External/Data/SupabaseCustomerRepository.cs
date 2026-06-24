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
    /// Queries, inserts and updates biz_customer table in Supabase via REST API.
    /// URL and API key come from SupabaseConfig (host appsettings.json AiModelProvider section or env).
    /// </summary>
    public class SupabaseCustomerRepository
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public SupabaseCustomerRepository(string baseUrl = null, string apiKey = null)
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
        /// Gets first row by wechat (index on wechat recommended).
        /// </summary>
        public async Task<BizCustomerEntity> GetByWechatAsync(string wechat)
        {
            if (string.IsNullOrWhiteSpace(wechat)) return null;
            var list = await GetByFilterAsync("wechat", wechat).ConfigureAwait(false);
            return list?.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Gets first row by phone_number (index on phone_number recommended).
        /// </summary>
        public async Task<BizCustomerEntity> GetByPhoneNumberAsync(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return null;
            var list = await GetByFilterAsync("phone_number", phoneNumber).ConfigureAwait(false);
            return list?.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Looks up by wechat first, then by phone number if not found.
        /// </summary>
        public async Task<BizCustomerEntity> GetByWechatOrPhoneAsync(string wechat, string phoneNumber)
        {
            var byWechat = await GetByWechatAsync(wechat).ConfigureAwait(false);
            if (byWechat != null) return byWechat;
            return await GetByPhoneNumberAsync(phoneNumber).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets customer row by customer_id (primary key).
        /// </summary>
        public async Task<BizCustomerEntity> GetByIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId)) return null;
            var list = await GetByFilterAsync("customer_id", customerId).ConfigureAwait(false);
            return list?.Count > 0 ? list[0] : null;
        }

        private async Task<List<BizCustomerEntity>> GetByFilterAsync(string column, string value)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured: set env SUPABASE_URL and SUPABASE_ANON_KEY (or SUPABASE_SERVICE_ROLE_KEY).");
            var url = $"/rest/v1/biz_customer?{column}=eq.{Uri.EscapeDataString(value)}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Accept", "application/json");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase query failed: {response.StatusCode} - {json}");
            return string.IsNullOrWhiteSpace(json) || json == "[]"
                ? new List<BizCustomerEntity>()
                : JsonConvert.DeserializeObject<List<BizCustomerEntity>>(json);
        }

        /// <summary>
        /// Inserts a new customer; returns entity with customer_id when Supabase returns representation.
        /// Table biz_customer: customer_id, name, phone_number, mobile, wechat, email, stage, created_at, updated_at (no age, pool).
        /// </summary>
        public async Task<BizCustomerEntity> InsertAsync(BizCustomerEntity entity)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            if (string.IsNullOrWhiteSpace(entity.CustomerId))
                entity.CustomerId = "cust-" + Guid.NewGuid().ToString("N");
            entity.UpdatedAt = entity.CreatedAt = DateTime.UtcNow;
            var payload = new Dictionary<string, object>
            {
                ["customer_id"] = entity.CustomerId,
                ["created_at"] = entity.CreatedAt,
                ["updated_at"] = entity.UpdatedAt
            };
            if (entity.Name != null) payload["name"] = entity.Name;
            if (entity.PhoneNumber != null) payload["phone_number"] = entity.PhoneNumber;
            if (entity.Mobile != null) payload["mobile"] = entity.Mobile;
            if (entity.Wechat != null) payload["wechat"] = entity.Wechat;
            if (entity.Email != null) payload["email"] = entity.Email;
            if (entity.Stage != null) payload["stage"] = entity.Stage;
            if (entity.IndustryId.HasValue) payload["industry_id"] = entity.IndustryId.Value;
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/biz_customer") { Content = content };
            request.Headers.Add("Prefer", "return=representation");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase insert failed: {response.StatusCode} - {responseJson}");
            var list = JsonConvert.DeserializeObject<List<BizCustomerEntity>>(responseJson ?? "[]");
            return list?.Count > 0 ? list[0] : entity;
        }

        /// <summary>
        /// Updates customer by customer_id (only non-null fields are sent).
        /// </summary>
        public async Task<BizCustomerEntity> UpdateAsync(string customerId, BizCustomerEntity updates)
        {
            if (string.IsNullOrWhiteSpace(_baseUrl) || string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("Supabase not configured.");
            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentNullException(nameof(customerId));
            updates.UpdatedAt = DateTime.UtcNow;
            var payload = new Dictionary<string, object>();
            if (updates.Name != null) payload["name"] = updates.Name;
            if (updates.PhoneNumber != null) payload["phone_number"] = updates.PhoneNumber;
            if (updates.Mobile != null) payload["mobile"] = updates.Mobile;
            if (updates.Wechat != null) payload["wechat"] = updates.Wechat;
            if (updates.Email != null) payload["email"] = updates.Email;
            if (updates.Stage != null) payload["stage"] = updates.Stage;
            if (updates.IndustryId.HasValue) payload["industry_id"] = updates.IndustryId.Value;
            payload["updated_at"] = updates.UpdatedAt;
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"/rest/v1/biz_customer?customer_id=eq.{Uri.EscapeDataString(customerId)}") { Content = content };
            request.Headers.Add("Prefer", "return=representation");
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"Supabase update failed: {response.StatusCode} - {responseJson}");
            var list = JsonConvert.DeserializeObject<List<BizCustomerEntity>>(responseJson ?? "[]");
            return list?.Count > 0 ? list[0] : null;
        }
    }
}
