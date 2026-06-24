using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Implementation;
using Slickflow.WebUtility;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// Generates BPMN JSON from natural language using the configured AI provider.
    /// Provider is selected by AiModelProvider.ActiveProvider in appsettings.json.
    /// Returns the raw JSON string; callers convert it to BPMN XML via JsonBpmnConvertor.
    /// </summary>
    public class BpmnGenService
    {
        public async Task<string> GenerateBpmnJsonAsync(string userMessage)
        {
            var options  = ApiKeyCryptoHelper.AiAppConfigOptions
                ?? throw new InvalidOperationException("AI configuration not initialised. Check appsettings.json AiModelProvider section.");
            var provider = (options.ActiveProvider ?? "QianWen").ToUpperInvariant();

            string apiUrl, apiKey, modelName;
            IAiLlmService llmService;

            switch (provider)
            {
                case "OPENAI":
                    var openAi = options.OpenAI
                        ?? throw new InvalidOperationException("AiModelProvider:OpenAI is not configured.");
                    apiUrl   = openAi.ChatApiUrl;
                    apiKey   = openAi.ApiKey;
                    modelName = openAi.Model;
                    llmService = AiLlmServiceFactory.CreateLargeModelServcie("openai");
                    break;

                case "DEEPSEEK":
                    var deepSeek = options.DeepSeek
                        ?? throw new InvalidOperationException("AiModelProvider:DeepSeek is not configured.");
                    apiUrl   = deepSeek.ChatApiUrl;
                    apiKey   = deepSeek.ApiKey;
                    modelName = deepSeek.Model;
                    llmService = AiLlmServiceFactory.CreateLargeModelServcie("deepseek");
                    break;

                default: // QianWen
                    var qianWen = options.QianWen
                        ?? throw new InvalidOperationException("AiModelProvider:QianWen is not configured.");
                    apiUrl   = qianWen.ChatApiUrl;
                    apiKey   = qianWen.ApiKey;
                    modelName = qianWen.Model;
                    llmService = AiLlmServiceFactory.CreateLargeModelServcie("QianWen");
                    break;
            }

            var messages = new List<CustomApiMessage>
            {
                new() { Role = "system", Content = GetBpmnSystemPrompt() },
                new() { Role = "user",   Content = userMessage }
            };

            var axConfig = new AiActivityConfigEntity
            {
                ModelName   = modelName,
                Temperature = 0.7m,
                MaxTokens   = 5000,
                Timeout     = 120
            };

            var response = await llmService.InvokeAIChatServiceWithMessagesAsync(apiUrl, apiKey, messages, axConfig);
            if (response?.Status == 1 && response.Entity != null)
                return response.Entity.Content ?? string.Empty;

            return string.Empty;
        }

        private static string GetBpmnSystemPrompt() => @"# System Role (BPMN 2.0 Modeling Assistant)

You are a BPMN 2.0 modeling assistant. Please generate BPMN 2.0 models based on user descriptions. The output must be a JSON object that strictly follows the JSON Schema specification below.

## Output Requirements

1. Output must be valid JSON only — no markdown code fences, no explanations.
2. Top-level fields required:
   - `Process` (object): process definition
   - `ProcessNodes` (array): list of nodes
   - `SequenceFlows` (array): list of connections

3. ProcessNodes fields:
   - `id` (string): unique node id (e.g. ""StartEvent"", ""Activity_Input"")
   - `name` (string): display name
   - `type` (string): one of ""startEvent"" | ""endEvent"" | ""userTask"" | ""serviceTask"" | ""exclusiveGateway""

4. SequenceFlows fields:
   - `id` (string): unique flow id
   - `sourceRef` (string): source node id
   - `targetRef` (string): target node id

5. Rules:
   - Process must start from startEvent and end at endEvent
   - All sourceRef/targetRef must reference existing ProcessNode ids
   - No position or layout information

## JSON Schema

{
  ""type"": ""object"",
  ""required"": [""Process"", ""ProcessNodes"", ""SequenceFlows""],
  ""properties"": {
    ""Process"": {
      ""type"": ""object"",
      ""required"": [""id"", ""name"", ""isExecutable""],
      ""properties"": {
        ""id"":           { ""type"": ""string"" },
        ""name"":         { ""type"": ""string"" },
        ""isExecutable"": { ""type"": ""boolean"" }
      },
      ""additionalProperties"": false
    },
    ""ProcessNodes"": {
      ""type"": ""array"", ""minItems"": 1,
      ""items"": {
        ""type"": ""object"",
        ""required"": [""id"", ""name"", ""type""],
        ""properties"": {
          ""id"":   { ""type"": ""string"" },
          ""name"": { ""type"": ""string"" },
          ""type"": { ""type"": ""string"", ""enum"": [""startEvent"",""userTask"",""endEvent"",""serviceTask"",""exclusiveGateway""] }
        },
        ""additionalProperties"": false
      }
    },
    ""SequenceFlows"": {
      ""type"": ""array"",
      ""items"": {
        ""type"": ""object"",
        ""required"": [""id"", ""sourceRef"", ""targetRef""],
        ""properties"": {
          ""id"":        { ""type"": ""string"" },
          ""sourceRef"": { ""type"": ""string"" },
          ""targetRef"": { ""type"": ""string"" }
        },
        ""additionalProperties"": false
      }
    }
  },
  ""additionalProperties"": false
}";
    }
}
