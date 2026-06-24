using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.AI.Agent.Core;

namespace Slickflow.AI.Agent.Tools
{
    /// <summary>
    /// Proxy tool that calls a remote MCP (Model Context Protocol) server.
    /// One McpClientTool instance represents a single tool on the remote server.
    /// Transport: JSON-RPC 2.0 over HTTP POST to {serverUrl}/mcp
    /// </summary>
    public sealed class McpClientTool : IAgentTool
    {
        private static readonly HttpClient _sharedHttp = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private readonly string _serverUrl;

        public string     Name        { get; }
        public string     Description { get; }
        public JsonObject InputSchema { get; }

        public McpClientTool(
            string     name,
            string     description,
            JsonObject inputSchema,
            string     serverUrl)
        {
            Name        = name        ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? string.Empty;
            InputSchema = inputSchema ?? new JsonObject();
            _serverUrl  = (serverUrl  ?? throw new ArgumentNullException(nameof(serverUrl))).TrimEnd('/');
        }

        /// <summary>
        /// Calls MCP server via JSON-RPC 2.0 tools/call method.
        /// POST {serverUrl}/mcp
        /// </summary>
        public async Task<AgentToolResult> ExecuteAsync(JsonObject args, CancellationToken ct = default)
        {
            var requestBody = new
            {
                jsonrpc = "2.0",
                id      = Guid.NewGuid().ToString("N"),
                method  = "tools/call",
                @params = new
                {
                    name      = Name,
                    arguments = args
                }
            };

            var jsonStr = JsonSerializer.Serialize(requestBody);
            Console.WriteLine($"[MCP] Calling {_serverUrl}/mcp tool={Name}");

            HttpResponseMessage httpResp;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}/mcp")
                {
                    Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
                };
                httpResp = await _sharedHttp.SendAsync(request, ct);
                httpResp.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MCP] HTTP request failed for {_serverUrl}: {ex.Message}");
                return new AgentToolResult(false, string.Empty, ex.Message);
            }

            var responseText = await httpResp.Content.ReadAsStringAsync();

            try
            {
                using var doc = JsonDocument.Parse(responseText);
                var root = doc.RootElement;

                // Check for JSON-RPC error
                if (root.TryGetProperty("error", out var err))
                {
                    var errMsg = err.TryGetProperty("message", out var em)
                        ? em.GetString() ?? "MCP call failed"
                        : "MCP call failed";
                    Console.WriteLine($"[MCP] Tool {Name} returned error: {errMsg}");
                    return new AgentToolResult(false, string.Empty, errMsg);
                }

                // Extract result.content[0].text (standard MCP format)
                var content = root
                    .GetProperty("result")
                    .GetProperty("content")[0]
                    .GetProperty("text")
                    .GetString() ?? string.Empty;

                Console.WriteLine($"[MCP] Tool {Name} returned {content.Length} chars");
                return new AgentToolResult(true, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MCP] Failed to parse response from {_serverUrl}: {ex.Message}");
                return new AgentToolResult(false, string.Empty, $"Failed to parse MCP response: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Discovers all tools on an MCP server via tools/list and returns proxy instances.
    /// </summary>
    public sealed class McpServerClient
    {
        private static readonly HttpClient _sharedHttp = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private readonly string _serverUrl;

        public McpServerClient(string serverUrl)
        {
            _serverUrl = (serverUrl ?? throw new ArgumentNullException(nameof(serverUrl))).TrimEnd('/');
        }

        /// <summary>
        /// Calls tools/list on the MCP server and returns a proxy McpClientTool for each tool.
        /// </summary>
        public async Task<IReadOnlyList<McpClientTool>> DiscoverToolsAsync(CancellationToken ct = default)
        {
            var listRequest = new
            {
                jsonrpc = "2.0",
                id      = "discover",
                method  = "tools/list",
                @params = new { }
            };

            Console.WriteLine($"[MCP] Discovering tools at {_serverUrl}");

            var jsonStr = JsonSerializer.Serialize(listRequest);
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}/mcp")
            {
                Content = new StringContent(jsonStr, Encoding.UTF8, "application/json")
            };

            var resp = await _sharedHttp.SendAsync(request, ct);
            resp.EnsureSuccessStatusCode();

            var responseText = await resp.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseText);
            var toolsJson = doc.RootElement.GetProperty("result").GetProperty("tools");

            var tools = new List<McpClientTool>();
            foreach (var t in toolsJson.EnumerateArray())
            {
                var name   = t.GetProperty("name").GetString() ?? string.Empty;
                var desc   = t.TryGetProperty("description", out var d) ? d.GetString() ?? "" : "";
                var schema = t.TryGetProperty("inputSchema", out var s)
                    ? JsonNode.Parse(s.GetRawText()) as JsonObject ?? new JsonObject()
                    : new JsonObject();

                tools.Add(new McpClientTool(name, desc, schema, _serverUrl));
            }

            Console.WriteLine($"[MCP] Discovered {tools.Count} tools from {_serverUrl}");
            return tools;
        }
    }
}
