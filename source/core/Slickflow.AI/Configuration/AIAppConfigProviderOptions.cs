using Microsoft.Extensions.Configuration;

namespace Slickflow.AI.Configuration
{
    /// <summary>
    /// Simplified AI provider options loader.
    /// </summary>
    public class AiAppConfigProviderOptions
    {
        private const string SectionName = "AiModelProvider";
        public string MasterKeyDb { get; set; } = string.Empty;
        public string SupabaseProjectUrl {  get; set; } = string.Empty;
        public string SupabaseServiceRoleKey { get; set; }  = string.Empty;
        public string AiChatMessageWebHook { get; set;  } = string.Empty;
        /// <summary>Active AI provider for BPMN generation: "OpenAI" | "DeepSeek" | "QianWen" (default)</summary>
        public string ActiveProvider { get; set; } = "QianWen";
        /// <summary>RAG embedding provider: "OpenAI" (default) or "QWen3"</summary>
        public string RagEmbeddingProvider { get; set; } = "OpenAI";
        /// <summary>RAG 向量查询输出维度，默认 1536</summary>
        public int RagEmbeddingDimensions { get; set; } = 1536;
        public SfAIOptions? SfAI { get; set; }
        public QianWenOptions? QianWen { get; set; }
        public OpenAIOptions? OpenAI { get; set; }
        public DeepSeekOptions? DeepSeek { get; set; }

        public static AiAppConfigProviderOptions Load(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var section = configuration.GetSection(SectionName);
            if (!section.Exists())
            {
                return new AiAppConfigProviderOptions();
            }

            var options = new AiAppConfigProviderOptions();
            // Bind all matching properties (MasterKey, AIBpmnAppUrl)
            section.Bind(options);
            return options;
        }
    }

    public class SfAIOptions
    {
        public string SfAIBaseUrl { get; set; } = string.Empty;
        public string SfAIBpmnByAIEndpoint { get; set; } = string.Empty;
    }

    public class QianWenOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string ApiKey { get; set; } = string.Empty;
        public string ApiKeyHeaderName { get; set; }
        public string ApiKeyPrefix { get; set; }
        public string Model { get; set; } = "gpt-4o";
        public string Endpoint { get; set; }
        /// <summary>RAG embedding 模型名，默认 text-embedding-v3（QWen3-Embedding）</summary>
        public string EmbeddingModel { get; set; } = "text-embedding-v3";

        public string ChatApiUrl
        {
            get
            {
                var baseUri = new Uri(BaseUrl.TrimEnd('/'));
                var endpoint = Endpoint.TrimStart('/');
                var fullPath = $"{baseUri.AbsoluteUri.TrimEnd('/')}/{endpoint}";
                return fullPath;
            }
        }
    }

    public class OpenAIOptions
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
        public string ApiKeyHeaderName { get; set; }
        public string ApiKeyPrefix { get; set; }
        public string Model { get; set; } = "gpt-4o";
        public string ApiUrl { get; set; }
        public bool UseDashScopeNativeFormat { get; set; }

        public string ChatApiUrl
        {
            get
            {
                var baseUri = new Uri(BaseUrl.TrimEnd('/'));
                var endpoint = ApiUrl.TrimStart('/');
                var fullPath = $"{baseUri.AbsoluteUri.TrimEnd('/')}/{endpoint}";
                return fullPath;
            }
        }
    }

    /// <summary>
    /// DeepSeek 大模型配置。API 与 OpenAI 完全兼容。
    /// </summary>
    public class DeepSeekOptions
    {
        public string BaseUrl { get; set; } = "https://api.deepseek.com";
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "deepseek-chat";
        public string Endpoint { get; set; } = "/v1/chat/completions";

        public string ChatApiUrl =>
            $"{BaseUrl.TrimEnd('/')}/{Endpoint.TrimStart('/')}";
    }
}
