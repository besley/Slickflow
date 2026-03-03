using Slickflow.AI.Utility;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// OpenAI 兼容 embedding 向量生成器，支持从 ai_model_provider 读取 BaseUrl、ModelName
    /// </summary>
    public class OpenAiEmbeddingGenerator : Slickflow.AI.Service.IEmbeddingGenerator
    {
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _modelName;

        public OpenAiEmbeddingGenerator(string apiKey, string? baseUrl = null, string? modelName = null)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? null : baseUrl;
            _modelName = string.IsNullOrWhiteSpace(modelName) ? null : modelName;
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            return await EmbeddingGenUtility.GenerateEmbeddingContent(_apiKey, text, _baseUrl, _modelName);
        }
    }
}
