using Slickflow.AI.Entity;
using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// AI 大模型调用服务。严格区分两种配置来源：
    /// 1) InvokeAIServiceAsync：工作流节点执行，从 ai_activity_config → ai_model_provider 读取 BaseUrl/ApiKey
    /// 2) InvokeWithLocalConfigAsync：文字生成工作流等场景，从本地 appsettings (AiAppConfiguration) 读取 BaseUrl/Endpoint
    /// </summary>
    public interface IAiFastCallingService
    {
        /// <summary>工作流节点执行：从 ai_activity_config、ai_model_provider 读取配置</summary>
        Task<string> InvokeAIServiceAsync(AiActivityConfigEntity axConfig, IList<MultiMediaFile> mediaFileList);

        /// <summary>本地配置场景（如文字生成工作流）：从 appsettings AiModelProvider:OpenAI/QianWen 读取 BaseUrl、Endpoint</summary>
        Task<string> InvokeWithLocalConfigAsync(string userMessage, string systemPrompt = null, string provider = "OpenAI");

        Task<string> TestModelConnectionAsync(string baseUrl, string apiUUID, string apiKey, string modelName);
    }
}
