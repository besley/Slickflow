using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Slickflow.AI.Implementation
{
    public static class AiLlmServiceFactory
    {
        public static IAiLlmService CreateLargeModelServcie(string modelProvider)
        {
            if (modelProvider.Contains("openai", StringComparison.OrdinalIgnoreCase))
            {
                return new OpenAILlmService();
            }
            else if (modelProvider.Contains("wen", StringComparison.OrdinalIgnoreCase))
            {
                return new QianWenLlmService();
            }
            else if (modelProvider.Contains("deepseek", StringComparison.OrdinalIgnoreCase))
            {
                return new DeepSeekLlmService();
            }
            else
            {
                throw new ApplicationException($"There isnt matched method to invoke, the current model provider: {modelProvider}");
            }
        }

        public static IAiLlmService CreateLargeModelServcieTesting(string modelProvider, out string modelName)
        {
            if (modelProvider.Contains("openai", StringComparison.OrdinalIgnoreCase))
            {
                modelName = "gpt-4o";
                return new OpenAILlmService();
            }
            else if (modelProvider.Contains("wen", StringComparison.OrdinalIgnoreCase))
            {
                modelName = "qwen-plus";
                return new QianWenLlmService();
            }
            else if (modelProvider.Contains("deepseek", StringComparison.OrdinalIgnoreCase))
            {
                modelName = "deepseek-chat";
                return new DeepSeekLlmService();
            }
            else
            {
                throw new ApplicationException($"There isnt matched method to invoke, the current model provider: {modelProvider}");
            }
        }
    }
}
