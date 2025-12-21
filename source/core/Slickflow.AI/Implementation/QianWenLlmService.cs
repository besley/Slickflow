using Slickflow.AI.Utility;
using Slickflow.WebUtility;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Slickflow.AI.Implementation
{
    public class QianWenLlmService : IAiLlmService
    {
        public async Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl, 
            string apiKey, 
            string modelName, 
            string systemPrompt, 
            string userMessage,
            IList<MultiMediaFile> mediaFileList,
            decimal temperature, 
            int maxTokens, 
            int timeout)
        {
            var responseResult = ResponseResult<AIResponse>.Default();

            // 4. Prepare model parameters
            temperature = temperature > 0 ? temperature : (decimal)0.3;
            maxTokens = maxTokens > 0 ? maxTokens : 2000;

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                throw new InvalidOperationException("UserMessage is not set in AxConfig");
            }

            // 5. Build API endpoint URL
            baseUrl = baseUrl.TrimEnd('/');
            var baseUri = new Uri(baseUrl);
            var basePath = baseUri.AbsolutePath.Trim('/').ToLower();

            string apiEndpoint;
            if (basePath.EndsWith("chat/completions"))
            {
                // BaseUrl already includes the full path
                apiEndpoint = baseUrl;
            }
            else if (basePath.EndsWith("v1") || basePath == "v1")
            {
                // BaseUrl contains /v1, append chat/completions
                apiEndpoint = $"{baseUrl}/chat/completions";
            }
            else if (string.IsNullOrEmpty(basePath))
            {
                // BaseUrl is only the domain, append v1/chat/completions
                apiEndpoint = $"{baseUrl}/v1/chat/completions";
            }
            else
            {
                // Otherwise assume baseUrl is already complete
                apiEndpoint = baseUrl;
            }

            // 6. Create HttpClient (consistent with BpmnApiClient)
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(timeout > 0 ? timeout : 60);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // 7. Build request message list
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

            // 8. Build request payload (per BpmnApiClient)
            var chatRequest = new ChatRequest
            {
                Model = modelName,
                Messages = messages.ToArray(),
                temperature = (double)temperature,
                max_tokens = maxTokens
            };

            // 9. Serialize request and send
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

                // 10. Parse response JSON (per BpmnApiClient)
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

        /// <summary>
        /// Test model connectivity (per BpmnApiClient)
        /// </summary>
        public async Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName)
        {
            // 1. Build API endpoint URL
            var normalizedBaseUrl = baseUrl.TrimEnd('/');
            var baseUri = new Uri(normalizedBaseUrl);
            var basePath = baseUri.AbsolutePath.Trim('/').ToLower();

            string apiEndpoint;
            if (basePath.EndsWith("chat/completions"))
            {
                // BaseUrl already includes the full path
                apiEndpoint = normalizedBaseUrl;
            }
            else if (basePath.EndsWith("v1") || basePath == "v1")
            {
                // BaseUrl contains /v1, append chat/completions
                apiEndpoint = $"{normalizedBaseUrl}/chat/completions";
            }
            else if (string.IsNullOrEmpty(basePath))
            {
                // BaseUrl is only the domain, append v1/chat/completions
                apiEndpoint = $"{normalizedBaseUrl}/v1/chat/completions";
            }
            else
            {
                // Otherwise assume baseUrl is already complete
                apiEndpoint = normalizedBaseUrl;
            }

            // 2. Create HttpClient (per BpmnApiClient)
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(30); // Use shorter timeout for connectivity test
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            // 3. Build request message list
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

            // 4. Build request payload (per BpmnApiClient)
            var chatRequest = new ChatRequest
            {
                Model = modelName,
                Messages = messages.ToArray(),
                temperature = (double)0.3,
                max_tokens = 50
            };

            // 5. Serialize request and send
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

                // 6. Parse response JSON (per BpmnApiClient)
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
