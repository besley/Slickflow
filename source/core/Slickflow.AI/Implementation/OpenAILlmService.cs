using Slickflow.AI.Entity;
using Slickflow.WebUtility;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Slickflow.AI.Common;

namespace Slickflow.AI.Implementation
{
    /// <summary>
    /// OpenAI 兼容 LLM 服务。baseUrl 由调用方传入，调用方需严格区分配置来源：
    /// - 工作流节点：从 ai_activity_config → ai_model_provider 传入
    /// - 文字生成工作流等：从 AiAppConfiguration (appsettings) 传入，通过 InvokeWithLocalConfigAsync
    /// </summary>
    internal class OpenAILlmService : IAiLlmService
    {
        private static readonly HttpClient _httpClient;

        static OpenAILlmService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

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
            var serviceType = EnumHelper.TryParseEnum<AiServiceTypeEnum>(axConfig.ServiceType);

            var responseResult = ResponseResult<AIResponse>.Default();
            // Validate parameters
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be empty", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(modelName))
                throw new ArgumentException("Model name cannot be empty", nameof(modelName));
            if (string.IsNullOrWhiteSpace(userMessage))
                throw new ArgumentException("User message cannot be empty", nameof(userMessage));
            if (mediaFileList == null || mediaFileList.Count == 0)
                throw new ArgumentException("Media file list cannot be empty", nameof(mediaFileList));

            // Parameter range checks
            if (temperature < 0 || temperature > 2)
                throw new ArgumentException("Temperature must be between 0 and 2", nameof(temperature));
            if (maxTokens <= 0)
                throw new ArgumentException("MaxTokens must be greater than 0", nameof(maxTokens));
            if (timeout <= 0)
                throw new ArgumentException("Timeout must be greater than 0 seconds", nameof(timeout));

            var apiUrl = baseUrl.TrimEnd('/');
            if (!apiUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiUrl = "https://" + apiUrl;
            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out var fullUri))
                throw new InvalidOperationException($"Invalid API URL: {apiUrl}");

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

            try
            {

                // Build chat message for different agent types, such as LLM / RAG / Agent
                var messages = await LlmChatMessageBuilder.BuildChatMessageContentAsync(
                    serviceType,
                    mediaFileList,
                    systemPrompt,
                    userMessage,
                    axConfig);

                // Build request object
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

                var jsonString = JsonSerializer.Serialize(request, jsonOptions);
                var jsonContent = new StringContent(
                    jsonString,
                    Encoding.UTF8,
                    "application/json");

                // Create HttpRequestMessage
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                {
                    Content = jsonContent
                };

                // Setting Authorization header
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Send request for llm
                // The main code for request processing of AI big model
                HttpResponseMessage? response = null;
                try
                {
                    response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    throw new HttpRequestException($"API request timed out (over {timeout} seconds). API url: {fullUri}.", ex);
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException($"API request failed. API url: {fullUri}. Error: {ex.Message}", ex);
                }

                // Handle response
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cts.Token);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException(
                            $"API key validation failed. API url: {fullUri}, model: {modelName}. Please verify the API key configuration. Details: {errorContent}");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // 400 error with the wrong format
                        throw new HttpRequestException(
                            $"Bad Request (400). This usually indicates an issue with the request format. API url: {fullUri}, model: {modelName}. Details: {errorContent}");
                    }

                    throw new HttpRequestException(
                        $"API call failed, status code: {response.StatusCode}. API url: {fullUri}, model: {modelName}. Details: {errorContent}");
                }

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync(cts.Token);

                // Parse response
                var result = JsonSerializer.Deserialize<CustomApiChatResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Error != null)
                {
                    throw new InvalidOperationException($"API returned error: {result.Error.Message}");
                }

                var content = result?.Choices?[0]?.Message?.Content?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new InvalidOperationException("API returned empty content");
                }

                var aiResponse = new AIResponse
                {
                    Content = content,
                    StatusCode = "200"
                };

                responseResult = ResponseResult<AIResponse>.Success(aiResponse);

                return responseResult;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.CancellationToken.IsCancellationRequested || cts.Token.IsCancellationRequested)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                throw new HttpRequestException(
                    $"API request timed out (over {timeout} seconds). Request url: {requestUrl}, model name: {modelName}. Please check connectivity or increase the timeout.", ex);
            }
            catch (HttpRequestException ex)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                var innerExceptionInfo = ex.InnerException != null
                    ? $" ({ex.InnerException.GetType().Name}: {ex.InnerException.Message})"
                    : "";

                throw new HttpRequestException(
                    $"Unable to reach the API server. Please check: 1) network connectivity 2) URL correctness: {requestUrl} 3) firewall settings 4) API availability{innerExceptionInfo}", ex);
            }
            catch (Exception ex)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                throw new Exception(
                    $"An error occurred while calling the AI service: {ex.Message}. Request url: {requestUrl}, model name: {modelName}.", ex);
            }
            finally
            {
                cts?.Dispose();
            }
        }

        /// <summary>
        /// Invokes the LLM with a pre-built message list (used by RagMultiTurnService for multi-turn RAG with history).
        /// </summary>
        public async Task<ResponseResult<AIResponse>> InvokeAIChatServiceWithMessagesAsync(string baseUrl,
            string apiKey,
            IList<CustomApiMessage> messages,
            AiActivityConfigEntity axConfig)
        {
            if (messages == null || messages.Count == 0)
                throw new ArgumentException("Messages cannot be null or empty", nameof(messages));
            var modelName = axConfig?.ModelName ?? throw new ArgumentNullException(nameof(axConfig));
            var temperature = axConfig.Temperature;
            var maxTokens = axConfig.MaxTokens;
            var timeout = axConfig.Timeout;
            if (temperature < 0 || temperature > 2) temperature = 0.3m;
            if (maxTokens <= 0) maxTokens = 2048;
            if (timeout <= 0) timeout = 60;

            var apiUrl = baseUrl.TrimEnd('/');
            if (!apiUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiUrl = "https://" + apiUrl;
            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out var fullUri))
                throw new InvalidOperationException($"Invalid API URL: {apiUrl}");

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));
            try
            {
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
                var jsonContent = new StringContent(JsonSerializer.Serialize(request, jsonOptions), Encoding.UTF8, "application/json");
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri) { Content = jsonContent };
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
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
                if (string.IsNullOrWhiteSpace(content))
                    throw new InvalidOperationException("API returned empty content");
                return ResponseResult<AIResponse>.Success(new AIResponse { Content = content, StatusCode = "200" });
            }
            finally { cts?.Dispose(); }
        }

        /// <summary>
        /// Generate chat completion (supports multimodal input such as image recognition)
        /// </summary>
        public async Task<string> TestConnectionAsync(string baseUrl, string apiKey, string modelName)
        {
            // Validate parameters
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be empty", nameof(baseUrl));
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API key cannot be empty", nameof(apiKey));
            if (string.IsNullOrWhiteSpace(modelName))
                throw new ArgumentException("Model name cannot be empty", nameof(modelName));

            var apiUrl = baseUrl.TrimEnd('/');
            if (!apiUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                apiUrl = "https://" + apiUrl;
            if (!Uri.TryCreate(apiUrl, UriKind.Absolute, out var fullUri))
                throw new InvalidOperationException($"Invalid API URL: {apiUrl}");

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            
            try
            {
                // Prepare request payload
                var messages = new List<CustomApiMessage>
                {
                    new() { Role = "system", Content = "You are a helpful assistant." },
                    new() { Role = "user", Content = "Respond with 'API connection successful'" }
                };

                var request = new CustomApiChatRequest
                {
                    Model = modelName,
                    Messages = messages.ToArray(),
                    Temperature = 0.3,
                    MaxTokens = 200
                };

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var jsonString = JsonSerializer.Serialize(request, jsonOptions);
                var jsonContent = new StringContent(
                    jsonString,
                    Encoding.UTF8,
                    "application/json");

                // Create HttpRequestMessage without mutating shared HttpClient defaults
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                {
                    Content = jsonContent
                };

                // Set request-specific headers on HttpRequestMessage (per new_test.cs)
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Send request using static HttpClient (per new_test.cs)
                HttpResponseMessage? response = null;
                try
                {
                    response = await _httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
                {
                    throw new HttpRequestException($"API request timed out (over {_httpClient.Timeout.TotalSeconds} seconds). API url: {fullUri}.", ex);
                }
                catch (Exception ex)
                {
                    throw new HttpRequestException($"API request failed. API url: {fullUri}. Error: {ex.Message}", ex);
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cts.Token);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException(
                            $"API key validation failed. API url: {fullUri}, model: {modelName}. Please verify the API key configuration. Details: {errorContent}");
                    }

                    throw new HttpRequestException(
                        $"API call failed, status code: {response.StatusCode}. API url: {fullUri}, model: {modelName}. Details: {errorContent}");
                }

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync(cts.Token);
                
                // Parse response (following new_test.cs approach)
                var result = JsonSerializer.Deserialize<CustomApiChatResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Error != null)
                {
                    throw new InvalidOperationException($"API returned error: {result.Error.Message}");
                }

                var content = result?.Choices?[0]?.Message?.Content?.ToString() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new InvalidOperationException("API returned empty content");
                }

                return content;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException || ex.CancellationToken.IsCancellationRequested || cts.Token.IsCancellationRequested)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                throw new HttpRequestException(
                    $"API request timed out (over 60 seconds). Request url: {requestUrl}, model name: {modelName}. Please check connectivity or increase the timeout.", ex);
            }
            catch (HttpRequestException ex)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                var innerExceptionInfo = ex.InnerException != null 
                    ? $" ({ex.InnerException.GetType().Name}: {ex.InnerException.Message})" 
                    : "";
                
                throw new HttpRequestException(
                    $"Unable to reach the API server. Please check: 1) network connectivity 2) URL correctness: {requestUrl} 3) firewall settings 4) API availability{innerExceptionInfo}", ex);
            }
            catch (Exception ex)
            {
                var requestUrl = fullUri?.ToString() ?? apiUrl;
                throw new Exception(
                    $"Error occurred while testing the connection: {ex.Message}. Request url: {requestUrl}, model name: {modelName}.", ex);
            }
            finally
            {
                // Dispose CancellationTokenSource
                cts?.Dispose();
            }
        }
    }

    /// <summary>
    /// Custom API chat request
    /// </summary>
    public class CustomApiChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public CustomApiMessage[] Messages { get; set; } = Array.Empty<CustomApiMessage>();

        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("response_format")]
        public object? ResponseFormat { get; set; }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }
    }

    /// <summary>
    /// Custom API message
    /// </summary>
    public class CustomApiMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        [JsonPropertyName("content")]
        public object Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// Custom API chat response
    /// </summary>
    public class CustomApiChatResponse
    {
        [JsonPropertyName("choices")]
        public CustomApiChoice[]? Choices { get; set; }

        [JsonPropertyName("error")]
        public CustomApiError? Error { get; set; }
    }

    /// <summary>
    /// Custom API choice
    /// </summary>
    public class CustomApiChoice
    {
        [JsonPropertyName("message")]
        public CustomApiMessage? Message { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }

    /// <summary>
    /// Custom API error
    /// </summary>
    public class CustomApiError
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }
    }
}
