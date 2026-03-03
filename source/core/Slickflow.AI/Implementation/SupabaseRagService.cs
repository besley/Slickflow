using Slickflow.AI.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// Supabase RAG service
    /// 使用 Supabase REST API 调用向量检索函数 match_documents_optimized
    /// </summary>
    public class SupabaseRagService
    {
        private readonly AiAppConfigProviderOptions _configOptions;

        public SupabaseRagService(AiAppConfigProviderOptions configOptions)
        {
            _configOptions = configOptions ?? throw new ArgumentNullException(nameof(configOptions));
        }

        // Vector retrieval method
        /// <param name="industryId">Optional industry ID to filter knowledge base by industry (e.g. 电动汽车新能源). When null, no industry filter is applied.</param>
        public async Task<List<MatchDocumentResult>> MatchDocumentsOptimizedAsync(
            float[] queryEmbedding,
            float matchThresholdHigh = 0.8f,
            float matchThresholdLow = 0.6f,
            int matchCountHigh = 5,
            int matchCountLow = 3,
            long? industryId = null)
        {
            if (queryEmbedding == null || queryEmbedding.Length == 0)
                throw new ArgumentException("queryEmbedding cannot be null or empty", nameof(queryEmbedding));

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");

            // Check URL format
            if (!Uri.TryCreate(_configOptions.SupabaseProjectUrl, UriKind.Absolute, out var uri))
            {
                throw new InvalidOperationException($"SupabaseProjectUrl 格式无效: {_configOptions.SupabaseProjectUrl}");
            }

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Construct the RPC call request body (Note: Supabase RPC requires the snake_case parameter name)
            var requestBody = new Dictionary<string, object>
            {
                { "query_embedding", queryEmbedding },
                { "match_threshold_high", matchThresholdHigh },
                { "match_threshold_low", matchThresholdLow },
                { "match_count_high", matchCountHigh },
                { "match_count_low", matchCountLow }
            };
            if (industryId.HasValue)
            {
                requestBody["filter_industry_id"] = industryId.Value;
            }

            // Use Newton soft. Json to ensure that the parameter name remains snake_case
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // POST /rest/v1/rpc/biz_match_documents_optimized
                // 必须使用 biz_match_documents_optimized（含 filter_industry_id 参数），见 docs/database/match_documents_optimized_add_industry_id.sql
                var requestUrl = "/rest/v1/rpc/biz_match_documents_optimized";

                var response = await httpClient.PostAsync(requestUrl, content);

                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(
                        $"Failed to call RPC biz_match_documents_optimized: HTTP {response.StatusCode} - {responseJson}. Ensure you ran docs/database/match_documents_optimized_add_industry_id.sql");
                }

                // Check whether it is an empty array
                if (responseJson.Trim() == "[]")
                {
                    return new List<MatchDocumentResult>();
                }

                // Try multiple deserialization methods
                List<MatchDocumentResult>? results = JsonConvert.DeserializeObject<List<MatchDocumentResult>>(responseJson);
                return results;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calling Supabase biz_match_documents_optimized: {ex.Message}", ex);
            }
        }
    }

    public class MatchDocumentResult
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("metadata")]
        public object? Metadata { get; set; }

        [JsonProperty("similarity")]
        public float Similarity { get; set; }
    }
}


