using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slickflow.WebUtility;
using Slickflow.AI.Entity;

namespace Slickflow.AI.Implementation
{
    public interface IAiLlmService
    {
        Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl,
            string apiKey,
            IList<MultiMediaFile> mediaFileList,
            AiActivityConfigEntity axConfig);

        /// <summary>
        /// Invokes the LLM with a pre-built message list (for multi-turn RAG; RagMultiTurnService passes messages with history).
        /// </summary>
        Task<ResponseResult<AIResponse>> InvokeAIChatServiceWithMessagesAsync(string baseUrl,
            string apiKey,
            IList<CustomApiMessage> messages,
            AiActivityConfigEntity axConfig);

        Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName);
    }
}
