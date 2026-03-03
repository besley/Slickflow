using Slickflow.AI.Entity;
using Slickflow.AI.Utility;
using Slickflow.WebUtility;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// 千问 LLM 服务。baseUrl 由调用方传入，调用方需严格区分配置来源：
    /// - 工作流节点：从 ai_activity_config → ai_model_provider 传入
    /// - 文字生成工作流等：从 AiAppConfiguration (appsettings) 传入，通过 InvokeWithLocalConfigAsync
    /// </summary>
    public class QianWenLlmService : IAiLlmService
    {
        public async Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl,
            string apiKey,
            IList<MultiMediaFile> mediaFileList,
            AiActivityConfigEntity axConfig)
        {
            var modelName = axConfig.ModelName;
            var systemPrompt = axConfig.SystemPrompt;
            var userMessage = axConfig.UserMessage;
            var temperature = axConfig.Temperature;
            var maxTokens = axConfig.MaxTokens;
            var timeout = axConfig.Timeout;
            var responseResult = ResponseResult<AIResponse>.Default();

            // Prepare model parameters
            temperature = temperature > 0 ? temperature : (decimal)0.3;
            maxTokens = maxTokens > 0 ? maxTokens : 2000;

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new InvalidOperationException("UserMessage is not set in AxConfig");
            }

            var apiEndpoint = baseUrl.TrimEnd('/');
            if (!apiEndpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiEndpoint = "https://" + apiEndpoint;

            // Create HttpClient (consistent with BpmnApiClient)
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(timeout > 0 ? timeout : 60);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Build request message list
            var messages = new List<ChatMessage>();
            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                messages.Add(new ChatMessage
                {
                    Role = "system",
                    Content = systemPrompt
                });
            }
            messages.Add(new ChatMessage
            {
                Role = "user",
                Content = userMessage
            });

            // Build request payload (per BpmnApiClient)
            var chatRequest = new ChatRequest
            {
                Model = modelName,
                Messages = messages.ToArray(),
                temperature = (double)temperature,
                max_tokens = maxTokens
            };

            // Serialize request and send
            // Use CancellationToken to enforce timeout
            var timeoutSeconds = timeout > 0 ? timeout : 60;
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
            
            try
            {
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(chatRequest, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }),
                    Encoding.UTF8,
                    "application/json");

                // Create HttpRequestMessage and use SendAsync with cancellationToken
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiEndpoint)
                {
                    Content = jsonContent
                };
                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse response JSON (per BpmnApiClient)
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException("AI model returned empty response");
                }

                var firstChoice = choices[0];
                if (!firstChoice.TryGetProperty("message", out var message) ||
                    !message.TryGetProperty("content", out var content))
                {
                    throw new InvalidOperationException("AI model returned empty message content");
                }

                var result = content.GetString();
                if (string.IsNullOrWhiteSpace(result))
                {
                    throw new InvalidOperationException("AI model returned empty content");
                }

                var aiResponse = new AIResponse
                {
                    Content = result,
                    StatusCode = "200"
                };
                responseResult = ResponseResult<AIResponse>.Success(aiResponse);

                return responseResult;
            }
            catch (HttpRequestException ex)
            {
                // Re-throw with additional context
                var maskedKey = apiKey?.Length > 8
                    ? $"{apiKey[..4]}...{apiKey[^4..]}"
                    : "***";
                var authInfo = $"Authorization=Bearer {maskedKey}";

                var enhancedError = $"{ex.Message}" +
                    $"\nCall context:" +
                    $"\nModel name: {modelName}" +
                    $"\nBaseUrl: {baseUrl}" +
                    $"\nAPI endpoint: {apiEndpoint}" +
                    $"\nAuth mode: {authInfo}" +
                    $"\nTimeout: {httpClient.Timeout.TotalSeconds} seconds" +
                    $"\nSystem prompt: {systemPrompt ?? "(empty)"}" +
                    $"\nUser message: {userMessage}" +
                    $"\nTemperature parameter: {temperature}" +
                    $"\nMax token count: {maxTokens}";

                throw new HttpRequestException(enhancedError, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.CancellationToken.IsCancellationRequested || cts.Token.IsCancellationRequested)
            {
                var maskedKey = apiKey?.Length > 8
                    ? $"{apiKey[..4]}...{apiKey[^4..]}"
                    : "***";
                var authInfo = $"Authorization=Bearer {maskedKey}";

                var enhancedError = $"API request timed out (over {timeoutSeconds} seconds)." +
                    $"\nRequest url: {apiEndpoint}" +
                    $"\nModel name: {modelName}" +
                    $"\nBaseUrl: {baseUrl}" +
                    $"\nAuth mode: {authInfo}" +
                    $"\nTimeout: {timeoutSeconds} seconds" +
                    $"\nPlease check: 1) network connectivity 2) URL correctness 3) firewall settings 4) API availability";

                throw new HttpRequestException(enhancedError, ex);
            }
            finally
            {
                // Dispose CancellationTokenSource
                cts?.Dispose();
            }
        }

        public async Task<ResponseResult<AIResponse>> InvokeAIChatServiceWithMessagesAsync(string baseUrl,
            string apiKey,
            IList<CustomApiMessage> messages,
            AiActivityConfigEntity axConfig)
        {
            if (messages == null || messages.Count == 0)
                throw new ArgumentException("Messages cannot be null or empty", nameof(messages));
            var modelName = axConfig?.ModelName ?? throw new ArgumentNullException(nameof(axConfig));
            var temperature = (axConfig.Temperature > 0 ? axConfig.Temperature : 0.3m);
            var maxTokens = (axConfig.MaxTokens > 0 ? axConfig.MaxTokens : 2048);
            var timeout = (axConfig.Timeout > 0 ? axConfig.Timeout : 60);

            var apiEndpoint = baseUrl.TrimEnd('/');
            if (!apiEndpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiEndpoint = "https://" + apiEndpoint;

            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            var request = new CustomApiChatRequest
            {
                Model = modelName,
                Messages = messages.ToArray(),
                Temperature = (double)temperature,
                MaxTokens = maxTokens,
                ResponseFormat = null
            };
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(request, jsonOptions), Encoding.UTF8, "application/json");
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiEndpoint) { Content = jsonContent };
                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cts.Token);
                    throw new HttpRequestException($"API call failed: {response.StatusCode}. {errorContent}");
                }
                var responseContent = await response.Content.ReadAsStringAsync(cts.Token);
                var result = JsonSerializer.Deserialize<CustomApiChatResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result?.Error != null)
                    throw new InvalidOperationException($"API returned error: {result.Error.Message}");
                var content = result?.Choices?[0]?.Message?.Content?.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(content) && result?.Choices?[0]?.Message?.Content != null)
                {
                    if (result.Choices[0].Message.Content is JsonElement je)
                        content = je.GetString() ?? string.Empty;
                }
                if (string.IsNullOrWhiteSpace(content))
                    throw new InvalidOperationException("API returned empty content");
                return ResponseResult<AIResponse>.Success(new AIResponse { Content = content, StatusCode = "200" });
            }
            finally { cts?.Dispose(); }
        }

        /// <summary>
        /// Test model connectivity (per BpmnApiClient)
        /// </summary>
        public async Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName)
        {
            var apiEndpoint = baseUrl.TrimEnd('/');
            if (!apiEndpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiEndpoint = "https://" + apiEndpoint;

            // Create HttpClient (per BpmnApiClient)
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30); // Use shorter timeout for connectivity test
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // Build request message list
            var messages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Role = "system",
                    Content = "You are a helpful assistant."
                },
                new ChatMessage
                {
                    Role = "user",
                    Content = "Hello"
                }
            };

            // Build request payload (per BpmnApiClient)
            var chatRequest = new ChatRequest
            {
                Model = modelName,
                Messages = messages.ToArray(),
                temperature = (double)0.3,
                max_tokens = 50
            };

            // Serialize request and send
            // Use CancellationToken to enforce timeout
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            
            try
            {
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(chatRequest, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    }),
                    Encoding.UTF8,
                    "application/json");

                // Create HttpRequestMessage and use SendAsync with cancellationToken
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, apiEndpoint)
                {
                    Content = jsonContent
                };
                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                //Parse response JSON (per BpmnApiClient)
                using var jsonDoc = JsonDocument.Parse(responseContent);
                var root = jsonDoc.RootElement;

                if (!root.TryGetProperty("choices", out var choices) || choices.GetArrayLength() == 0)
                {
                    throw new InvalidOperationException("AI model returned empty response");
                }

                var firstChoice = choices[0];
                if (!firstChoice.TryGetProperty("message", out var message) ||
                    !message.TryGetProperty("content", out var content))
                {
                    throw new InvalidOperationException("AI model returned empty message content");
                }

                var result = content.GetString();
                if (string.IsNullOrWhiteSpace(result))
                {
                    throw new InvalidOperationException("AI model returned empty content");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                // Re-throw with additional context
                var maskedKey = apiKey?.Length > 8
                    ? $"{apiKey[..4]}...{apiKey[^4..]}"
                    : "***";
                var authInfo = $"Authorization=Bearer {maskedKey}";

                var enhancedError = $"{ex.Message}" +
                    $"\nTest connection context:" +
                    $"\nModel name: {modelName}" +
                    $"\nBaseUrl: {baseUrl}" +
                    $"\nAPI endpoint: {apiEndpoint}" +
                    $"\nAuth mode: {authInfo}" +
                    $"\nTimeout: {httpClient.Timeout.TotalSeconds} seconds" +
                    $"\nTest message: Hello" +
                    $"\nSystem prompt: You are a helpful assistant.";

                throw new HttpRequestException(enhancedError, ex);
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.CancellationToken.IsCancellationRequested || cts.Token.IsCancellationRequested)
            {
                var maskedKey = apiKey?.Length > 8
                    ? $"{apiKey[..4]}...{apiKey[^4..]}"
                    : "***";
                var authInfo = $"Authorization=Bearer {maskedKey}";

                var enhancedError = $"API request timed out (over 30 seconds)." +
                    $"\nRequest url: {apiEndpoint}" +
                    $"\nModel name: {modelName}" +
                    $"\nBaseUrl: {baseUrl}" +
                    $"\nAuth mode: {authInfo}" +
                    $"\nTimeout: 30 seconds" +
                    $"\nPlease check: 1) network connectivity 2) URL correctness 3) firewall settings 4) API availability";

                throw new HttpRequestException(enhancedError, ex);
            }
            finally
            {
                // Dispose CancellationTokenSource
                cts?.Dispose();
            }
        }
    }
}
