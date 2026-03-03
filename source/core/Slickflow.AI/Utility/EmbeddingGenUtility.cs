using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Slickflow.AI.Utility
{
    internal class EmbeddingGenUtility
    {
        internal static async Task<float[]> GenerateEmbeddingContent(string openAiApiKey, string question)
        {
            return await GenerateEmbeddingContent(openAiApiKey, question, null, null);
        }

        /// <summary>
        /// OpenAI 兼容 embedding API。baseUrl 为完整 API 地址时直接使用，不做二次 Endpoint 拼接。
        /// </summary>
        internal static async Task<float[]> GenerateEmbeddingContent(string openAiApiKey, string question, string? baseUrl, string? modelName)
        {
            var url = string.IsNullOrWhiteSpace(baseUrl) ? "https://api.openai.com/v1/embeddings" : baseUrl.TrimEnd('/');
            var model = string.IsNullOrWhiteSpace(modelName) ? "text-embedding-3-small" : modelName;

            using var openAiHttpClient = new HttpClient();
            openAiHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
            openAiHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var embeddingRequest = new
                {
                    input = question,
                    model = model
                };

                var requestJson = JsonConvert.SerializeObject(embeddingRequest);
                var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await openAiHttpClient.PostAsync(url, requestContent);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"OpenAI API调用失败: HTTP {response.StatusCode} - {errorContent}");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                // 解析返回 JSON：{ "data": [ { "embedding": [ ... ] } ], ... }
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
                for (int i = 0; i < embeddingElement.GetArrayLength(); i++)
                {
                    // OpenAI 返回的是 double，这里转换为 float
                    vector[i] = (float)embeddingElement[i].GetDouble();
                }

                return vector;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"向量生成失败: {ex.Message}");
            }
        }
    }
}
