using Slickflow.AI.Entity;
using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Implementation
{
    public class DeepSeekLlmService : IAiLlmService
    {
        public Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl,
            string apiKey,
            IList<MultiMediaFile> mediaFileList,
            AiActivityConfigEntity axConfig)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseResult<AIResponse>> InvokeAIChatServiceWithMessagesAsync(string baseUrl,
            string apiKey,
            IList<CustomApiMessage> messages,
            AiActivityConfigEntity axConfig)
        {
            throw new NotImplementedException("Multi-turn RAG with history is currently supported only for OpenAI-compatible provider.");
        }

        public Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName)
        {
            throw new NotImplementedException();
        }
    }
}
