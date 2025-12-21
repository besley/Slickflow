using Slickflow.WebUtility;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Slickflow.AI.Implementation
{
    internal class OpenAILlmService : IAiLlmService
    {
        private static readonly HttpClient _httpClient;

        static OpenAILlmService()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<ResponseResult<AIResponse>> InvokeAIChatServiceAsync(string baseUrl, string apiKey, string modelName, string systemPrompt, string userMessage,
            IList<MultiMediaFile> mediaFileList, decimal temperature, int maxTokens, int timeout)
        {
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

            // Define variables outside try so they are available in catch
            Uri fullUri = null;
            string normalizedBaseUrl = baseUrl.TrimEnd('/');
            if (!normalizedBaseUrl.StartsWith("http"))
                normalizedBaseUrl = "https://" + normalizedBaseUrl;

            // Use CancellationToken to control timeout
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeout));

            try
            {
                // 修复：必须使用正确的OpenAI API路径
                var endpoint = "v1/chat/completions";

                if (_httpClient.BaseAddress != null)
                {
                    // 如果HttpClient有BaseAddress，使用相对路径
                    fullUri = new Uri(_httpClient.BaseAddress, endpoint);
                }
                else
                {
                    // 否则构建完整URL
                    // 确保baseUrl不包含v1路径
                    if (normalizedBaseUrl.EndsWith("/v1"))
                    {
                        // 如果baseUrl已经包含/v1，移除它
                        normalizedBaseUrl = normalizedBaseUrl.Substring(0, normalizedBaseUrl.Length - 3);
                    }
                    else if (normalizedBaseUrl.EndsWith("/v1/"))
                    {
                        normalizedBaseUrl = normalizedBaseUrl.Substring(0, normalizedBaseUrl.Length - 4);
                    }

                    var fullUrlString = $"{normalizedBaseUrl}/{endpoint}";

                    Console.WriteLine($"Debug: Building URL - BaseURL: {normalizedBaseUrl}, Endpoint: {endpoint}");
                    Console.WriteLine($"Debug: Full URL: {fullUrlString}");

                    if (!Uri.TryCreate(fullUrlString, UriKind.Absolute, out fullUri))
                    {
                        throw new InvalidOperationException($"无法构建有效的API URL: {fullUrlString}");
                    }
                }

                Console.WriteLine($"Debug: Request URL: {fullUri}");

                // Prepare request message list
                var messages = new List<CustomApiMessage>();

                // Add system prompt when present
                if (!string.IsNullOrWhiteSpace(systemPrompt))
                {
                    messages.Add(new CustomApiMessage
                    {
                        Role = "system",
                        Content = systemPrompt
                    });
                }

                // 构建多模态消息内容（支持图片+文本）
                // OpenAI 格式：先 text 后 image_url
                var imageFileData = mediaFileList[0];

                // 1. 清洗 base64 内容：如果前端已经带了 data: 前缀，这里需要去掉，只保留纯 base64
                var rawBase64 = imageFileData.base64Content ?? string.Empty;
                if (rawBase64.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    var commaIndex = rawBase64.IndexOf(',');
                    if (commaIndex > 0 && commaIndex < rawBase64.Length - 1)
                    {
                        rawBase64 = rawBase64.Substring(commaIndex + 1);
                    }
                }

                // 2. 根据文件名或媒体类型推断正确的 MIME Type（避免所有图片都用 image/jpeg 导致 400）
                var fileName = imageFileData.Name?.ToLowerInvariant() ?? string.Empty;
                var mimeType = "image/jpeg";   // 默认按 jpeg 处理

                if (fileName.EndsWith(".png"))
                    mimeType = "image/png";
                else if (fileName.EndsWith(".gif"))
                    mimeType = "image/gif";
                else if (fileName.EndsWith(".webp"))
                    mimeType = "image/webp";
                else if (fileName.EndsWith(".bmp"))
                    mimeType = "image/bmp";

                // 如果 Name 中没有后缀，可以根据 MultiMediaTypeEnum 再兜底判断（预留扩展）
                // 当前仅处理图片类型，其他类型暂不支持传给 vision 模型

                var imageUrl = $"data:{mimeType};base64,{rawBase64}";

                var multiModalContent = new List<object>
                    {
                        new { type = "text", text = userMessage },
                        new {
                            type = "image_url",
                            image_url = new {
                                url = imageUrl,
                                detail = "auto"
                            }
                        }
                    };

                messages.Add(new CustomApiMessage
                {
                    Role = "user",
                    Content = multiModalContent
                });

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
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                };

                var jsonString = JsonSerializer.Serialize(request, jsonOptions);

                // 调试：打印请求JSON
                Console.WriteLine($"Debug: Request JSON: {jsonString}");

                var jsonContent = new StringContent(
                    jsonString,
                    Encoding.UTF8,
                    "application/json");

                // Create HttpRequestMessage
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullUri)
                {
                    Content = jsonContent
                };

                // 设置正确的Authorization header
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // 添加OpenAI-Organization头部（如果需要）
                // httpRequest.Headers.Add("OpenAI-Organization", "your-organization-id");

                // 调试：打印请求头
                Console.WriteLine($"Debug: Request Headers:");
                foreach (var header in httpRequest.Headers)
                {
                    if (header.Key == "Authorization")
                    {
                        Console.WriteLine($"  {header.Key}: Bearer ***");
                    }
                    else
                    {
                        Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                    }
                }

                // Send request
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

                    // 添加详细的错误信息输出
                    Console.WriteLine($"API Error Response: {errorContent}");

                    // 调试：打印响应头
                    Console.WriteLine($"Debug: Response Headers:");
                    foreach (var header in response.Headers)
                    {
                        Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
                    }

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException(
                            $"API key validation failed. API url: {fullUri}, model: {modelName}. Please verify the API key configuration. Details: {errorContent}");
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        // 400错误通常表示请求格式有问题
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
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
                throw new HttpRequestException(
                    $"API request timed out (over {timeout} seconds). Request url: {requestUrl}, model name: {modelName}. Please check connectivity or increase the timeout.", ex);
            }
            catch (HttpRequestException ex)
            {
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
                var innerExceptionInfo = ex.InnerException != null
                    ? $" ({ex.InnerException.GetType().Name}: {ex.InnerException.Message})"
                    : "";

                throw new HttpRequestException(
                    $"Unable to reach the API server. Please check: 1) network connectivity 2) URL correctness: {requestUrl} 3) firewall settings 4) API availability{innerExceptionInfo}", ex);
            }
            catch (Exception ex)
            {
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
                throw new Exception(
                    $"An error occurred while calling the AI service: {ex.Message}. Request url: {requestUrl}, model name: {modelName}.", ex);
            }
            finally
            {
                // Dispose CancellationTokenSource
                cts?.Dispose();
            }
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

            // Define variables outside try so they are available in catch
            Uri fullUri = null;
            string normalizedBaseUrl = baseUrl.TrimEnd('/');
            if (!normalizedBaseUrl.StartsWith("http"))
                normalizedBaseUrl = "https://" + normalizedBaseUrl;
            
            // Use CancellationToken to control timeout
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            
            try
            {
                // Build the correct URL
                var baseUri = new Uri(normalizedBaseUrl);
                var basePath = baseUri.AbsolutePath.Trim('/').ToLower();

                // Decide full URL based on baseUrl path
                if (basePath.EndsWith("chat/completions"))
                {
                    fullUri = baseUri;
                }
                else if (basePath.EndsWith("v1") || basePath == "v1")
                {
                    fullUri = new Uri($"{normalizedBaseUrl}/chat/completions");
                }
                else if (string.IsNullOrEmpty(basePath))
                {
                    fullUri = new Uri($"{normalizedBaseUrl}/v1/chat/completions");
                }
                else
                {
                    fullUri = new Uri($"{normalizedBaseUrl}/chat/completions");
                }

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
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
                throw new HttpRequestException(
                    $"API request timed out (over 60 seconds). Request url: {requestUrl}, model name: {modelName}. Please check connectivity or increase the timeout.", ex);
            }
            catch (HttpRequestException ex)
            {
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
                var innerExceptionInfo = ex.InnerException != null 
                    ? $" ({ex.InnerException.GetType().Name}: {ex.InnerException.Message})" 
                    : "";
                
                throw new HttpRequestException(
                    $"Unable to reach the API server. Please check: 1) network connectivity 2) URL correctness: {requestUrl} 3) firewall settings 4) API availability{innerExceptionInfo}", ex);
            }
            catch (Exception ex)
            {
                var requestUrl = fullUri?.ToString() ?? normalizedBaseUrl;
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
