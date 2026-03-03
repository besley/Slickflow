using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// QWen3-Embedding 向量生成器，使用 DashScope 兼容模式 API
    /// BaseUrl、Model、Endpoint 均从配置文件读取，不在代码中指定默认值
    /// </summary>
    public class QWen3EmbeddingGenerator : Slickflow.AI.Service.IEmbeddingGenerator
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;
        private readonly string _model;
        private readonly int? _dimensions;

        /// <param name="apiKey">API Key</param>
        /// <param name="apiUrl">完整 embedding API 地址（ai_model_provider.BaseUrl），不做二次 Endpoint 拼接</param>
        /// <param name="model">模型名称</param>
        /// <param name="dimensions">RAG 向量查询输出维度，可选</param>
        public QWen3EmbeddingGenerator(string apiKey, string apiUrl, string model, int? dimensions = null)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("ApiKey is required.", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(apiUrl))
                throw new ArgumentException("ApiUrl (full embedding API address) is required.", nameof(apiUrl));
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model is required.", nameof(model));

            _apiKey = apiKey;
            _apiUrl = apiUrl.TrimEnd('/');
            _model = model;
            _dimensions = dimensions;
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or empty", nameof(text));

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 参考官方示例：input 为字符串数组，encoding_format 为 float；RAG 时可选 dimensions
            object embeddingRequest = _dimensions.HasValue
                ? new { model = _model, input = new[] { text }, encoding_format = "float", dimensions = _dimensions.Value }
                : new { model = _model, input = new[] { text }, encoding_format = "float" };

            var requestJson = JsonConvert.SerializeObject(embeddingRequest);
            var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(_apiUrl, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"QWen3 Embedding API 调用失败: HTTP {response.StatusCode} - {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseContent);
            if (!doc.RootElement.TryGetProperty("data", out var dataElement) ||
                dataElement.ValueKind != JsonValueKind.Array ||
                dataElement.GetArrayLength() == 0)
            {
                throw new InvalidOperationException("Embedding response does not contain data array.");
            }

            var first = dataElement[0];
            if (!first.TryGetProperty("embedding", out var embeddingElement) ||
                embeddingElement.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidOperationException("Embedding response does not contain embedding array.");
            }

            var vector = new float[embeddingElement.GetArrayLength()];
            for (var i = 0; i < embeddingElement.GetArrayLength(); i++)
            {
                vector[i] = (float)embeddingElement[i].GetDouble();
            }

            return vector;
        }
    }
}
