using Slickflow.AI.Entity;
using Slickflow.WebUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Service
{
    public interface IAiFastCallingService
    {
        Task<string> InvokeAIServiceAsync(AiActivityConfigEntity axConfig, IList<MultiMediaFile> mediaFileList);
        Task<string> TestModelConnectionAsync(string baseUrl, string apiUUID, string apiKey, string modelName);
    }
}
