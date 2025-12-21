using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.WebUtility;

namespace Slickflow.AI.Implementation
{
    public interface IAiLlmService
    {
        Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl,
            string apiKey,
            string modelName,
            string systemPrompt,
            string userMessage,
            IList<MultiMediaFile> mediaFileList,
            decimal temperature,
            int maxTokens,
            int timeout);
        Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName);
    }
}
