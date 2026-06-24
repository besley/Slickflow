using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.AI.Agent.Core;
using Slickflow.AI.Entity;

namespace Slickflow.AI.Agent.Llm
{
    /// <summary>
    /// Implements ILlmClient using the OpenAI-compatible function-calling format.
    /// Compatible with OpenAI, DeepSeek, QianWen and other OpenAI-format LLMs.
    /// </summary>
    public sealed class OpenAIToolCallLlmClient : ILlmClient
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(120)
        };

        private readonly string                 _baseUrl;
        private readonly string                 _apiKey;
        private readonly AiActivityConfigEntity _axConfig;

        public OpenAIToolCallLlmClient(
            string                 baseUrl,
            string                 apiKey,
            AiActivityConfigEntity axConfig)
        {
            _baseUrl  = (baseUrl  ?? throw new ArgumentNullException(nameof(baseUrl))).TrimEnd('/');
            _apiKey   = apiKey    ?? throw new ArgumentNullException(nameof(apiKey));
            _axConfig = axConfig  ?? throw new ArgumentNullException(nameof(axConfig));
        }

        /// <summary>
        /// Sends the conversation history plus tool definitions to the LLM.
        /// Returns either a tool-call request or a final text response.
        /// </summary>
        public async Task<LlmToolResponse> CompleteWithToolsAsync(
            IEnumerable<AgentMessage> messages,
            IEnumerable<IAgentTool>   tools,
            string?                   systemPrompt = null,
            CancellationToken         ct           = default)
        {
            var modelName   = _axConfig.ModelName;
            var temperature = (double)(_axConfig.Temperature > 0 ? _axConfig.Temperature : 0.3m);
            var maxTokens   = _axConfig.MaxTokens > 0 ? _axConfig.MaxTokens : 2048;

            // Build messages array
            var apiMessages = new List<object>();

            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                apiMessages.Add(new { role = "system", content = systemPrompt });
            }

            foreach (var msg in messages)
            {
                if (msg.Role == "tool")
                {
                    // OpenAI format: role=tool with tool_call_id
                    apiMessages.Add(new
                    {
                        role         = "tool",
                        tool_call_id = msg.ToolCallId ?? string.Empty,
                        content      = msg.Content
                    });
                }
                else if (msg.Role == "assistant" && msg.BatchToolCalls != null)
                {
                    // Assistant message with multiple parallel tool calls
                    apiMessages.Add(new
                    {
                        role       = "assistant",
                        content    = (string?)null,
                        tool_calls = msg.BatchToolCalls.Select(tc => new
                        {
                            id       = tc.Id,
                            type     = "function",
                            function = new
                            {
                                name      = tc.ToolName,
                                arguments = tc.Args?.ToJsonString() ?? "{}"
                            }
                        }).ToArray()
                    });
                }
                else if (msg.Role == "assistant" && msg.ToolCallId != null)
                {
                    // Assistant message that initiated a single tool call
                    apiMessages.Add(new
                    {
                        role       = "assistant",
                        content    = (string?)null,
                        tool_calls = new[]
                        {
                            new
                            {
                                id       = msg.ToolCallId,
                                type     = "function",
                                function = new
                                {
                                    name      = msg.ToolName ?? string.Empty,
                                    arguments = "{}"
                                }
                            }
                        }
                    });
                }
                else
                {
                    apiMessages.Add(new { role = msg.Role, content = msg.Content });
                }
            }

            // Build tools array
            var toolsList = tools?.ToList() ?? new List<IAgentTool>();
            var toolDefs  = toolsList.Select(t => new
            {
                type     = "function",
                function = new
                {
                    name        = t.Name,
                    description = t.Description,
                    parameters  = t.InputSchema
                }
            }).ToList();

            // Build full request
            var requestObj = new Dictionary<string, object>
            {
                ["model"]       = modelName,
                ["messages"]    = apiMessages,
                ["temperature"] = temperature,
                ["max_tokens"]  = maxTokens,
            };

            if (toolDefs.Count > 0)
            {
                requestObj["tools"]       = toolDefs;
                requestObj["tool_choice"] = "auto";
            }

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy        = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition      = JsonIgnoreCondition.WhenWritingNull
            };

            var jsonStr  = JsonSerializer.Serialize(requestObj, jsonOptions);
            var content  = new StringContent(jsonStr, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, _baseUrl)
            {
                Content = content
            };
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Console.WriteLine($"[Agent][LLM] POST {_baseUrl} model={modelName} tools={toolDefs.Count} msgs={apiMessages.Count}");

            HttpResponseMessage response;
            try
            {
                response = await _httpClient.SendAsync(httpRequest, ct);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"LLM HTTP request failed: {ex.Message}", ex);
            }

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"LLM returned {(int)response.StatusCode}: {err}");
            }

            var responseText = await response.Content.ReadAsStringAsync();

            return ParseOpenAiResponse(responseText);
        }

        // ─── Response parsing ────────────────────────────────────

        private static LlmToolResponse ParseOpenAiResponse(string responseText)
        {
            using var doc = JsonDocument.Parse(responseText);
            var root      = doc.RootElement;

            if (root.TryGetProperty("error", out var errProp))
            {
                var errMsg = errProp.TryGetProperty("message", out var em)
                    ? em.GetString() ?? "LLM error"
                    : "LLM error";
                throw new InvalidOperationException($"LLM API error: {errMsg}");
            }

            var choices = root.GetProperty("choices");
            if (choices.GetArrayLength() == 0)
                return new LlmToolResponse(false, string.Empty);

            var first        = choices[0];
            var finishReason = first.TryGetProperty("finish_reason", out var fr) ? fr.GetString() : null;
            var message      = first.GetProperty("message");

            // tool_calls branch — parse ALL entries to support parallel tool calls
            if (finishReason == "tool_calls" && message.TryGetProperty("tool_calls", out var toolCallsElem))
            {
                var allCalls = new List<ToolCallRequest>();
                foreach (var tc in toolCallsElem.EnumerateArray())
                {
                    var id       = tc.GetProperty("id").GetString() ?? Guid.NewGuid().ToString("N");
                    var funcProp = tc.GetProperty("function");
                    var toolName = funcProp.GetProperty("name").GetString() ?? string.Empty;
                    var argsStr  = funcProp.TryGetProperty("arguments", out var argsProp)
                        ? argsProp.GetString() ?? "{}"
                        : "{}";

                    JsonObject argsObj;
                    try   { argsObj = JsonNode.Parse(argsStr) as JsonObject ?? new JsonObject(); }
                    catch { argsObj = new JsonObject(); }

                    Console.WriteLine($"[Agent][LLM] tool_calls: {toolName} id={id}");
                    allCalls.Add(new ToolCallRequest(id, toolName, argsObj));
                }

                return new LlmToolResponse(
                    isToolCall:  true,
                    textContent: null,
                    toolCalls:   allCalls);
            }

            // Text content branch
            var textContent = string.Empty;
            if (message.TryGetProperty("content", out var contentProp) && contentProp.ValueKind != JsonValueKind.Null)
                textContent = contentProp.GetString() ?? string.Empty;

            Console.WriteLine($"[Agent][LLM] text response ({textContent.Length} chars)");
            return new LlmToolResponse(false, textContent);
        }
    }
}
