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
        public SfAIOptions? SfAI { get; set; }
        public QianWenOptions? QianWen { get; set; }
        public OpenAIOptions? OpenAI { get; set; }

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
}
