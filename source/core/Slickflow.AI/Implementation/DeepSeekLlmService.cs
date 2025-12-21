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
        public Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl, string apiKey, string modelName, string systemPrompt, string userMessage,
            IList<MultiMediaFile> mediaFileList, decimal temperature, int maxTokens, int timeout)
        {
            throw new NotImplementedException();
        }

        public Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName)
        {
            throw new NotImplementedException();
        }
    }
}
