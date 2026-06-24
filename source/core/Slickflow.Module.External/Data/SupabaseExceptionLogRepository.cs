using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Slickflow.Module.External.Data
{
    /// <summary>
    /// Inserts exception log records into Supabase table biz_exception_log.
    /// URL and API key from SupabaseConfig. If not configured, InsertAsync no-ops or throws.
    /// </summary>
    public class SupabaseExceptionLogRepository
    {
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public SupabaseExceptionLogRepository(string baseUrl = null, string apiKey = null)
        {
            _baseUrl = (baseUrl ?? SupabaseConfig.Url)?.TrimEnd('/');
            _apiKey = apiKey ?? SupabaseConfig.ApiKey;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_baseUrl ?? "http://localhost");
            _httpClient.DefaultRequestHeaders.Add("apikey", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool IsConfigured => !string.IsNullOrWhiteSpace(_baseUrl) && !string.IsNullOrWhiteSpace(_apiKey);

        /// <summary>
        /// Inserts an exception log row. Does nothing if Supabase is not configured.
        /// </summary>
        public async Task InsertAsync(BizExceptionLogEntity entity)
        {
            if (!IsConfigured) return;

            var payload = new
            {
                event_type_id = entity.EventTypeId,
                priority = entity.Priority,
                severity = entity.Severity ?? "HIGH",
                title = entity.Title ?? "Exception",
                message = Truncate(entity.Message, 2000),
                stack_trace = Truncate(entity.StackTrace, 4000),
                inner_stack_trace = Truncate(entity.InnerStackTrace, 4000),
                request_data = Truncate(entity.RequestData, 2000),
                time_stamp = entity.Timestamp,
                source = Truncate(entity.Source, 200)
            };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "/rest/v1/biz_exception_log") { Content = content };
            await _httpClient.SendAsync(request).ConfigureAwait(false);
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
