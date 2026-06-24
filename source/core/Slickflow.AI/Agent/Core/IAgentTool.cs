using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Slickflow.AI.Agent.Core
{
    /// <summary>
    /// Result returned by a tool execution.
    /// </summary>
    public sealed class AgentToolResult
    {
        public bool    Success      { get; }
        public string  Content      { get; }
        public string? ErrorMessage { get; }

        public AgentToolResult(bool success, string content, string? errorMessage = null)
        {
            Success      = success;
            Content      = content ?? string.Empty;
            ErrorMessage = errorMessage;
        }
    }

    /// <summary>
    /// Single message in the ReAct conversation history.
    /// Role values: "user" | "assistant" | "tool"
    /// </summary>
    public sealed class AgentMessage
    {
        public string  Role       { get; }
        public string  Content    { get; }
        public string? ToolCallId { get; }
        public string? ToolName   { get; }
        /// <summary>Non-null when this assistant message represents multiple parallel tool calls.</summary>
        public IReadOnlyList<ToolCallRequest>? BatchToolCalls { get; }

        public AgentMessage(
            string  role,
            string  content,
            string? toolCallId = null,
            string? toolName   = null)
        {
            Role       = role;
            Content    = content ?? string.Empty;
            ToolCallId = toolCallId;
            ToolName   = toolName;
        }

        /// <summary>Creates an assistant message that carries multiple parallel tool calls.</summary>
        public AgentMessage(string role, IReadOnlyList<ToolCallRequest> batchToolCalls)
        {
            Role           = role;
            Content        = string.Empty;
            BatchToolCalls = batchToolCalls;
        }
    }

    /// <summary>
    /// A tool-call request returned by the LLM.
    /// </summary>
    public sealed class ToolCallRequest
    {
        public string     Id       { get; }
        public string     ToolName { get; }
        public JsonObject Args     { get; }

        public ToolCallRequest(string id, string toolName, JsonObject args)
        {
            Id       = id;
            ToolName = toolName;
            Args     = args;
        }
    }

    /// <summary>
    /// Response from one LLM completion call.
    /// Either a final text answer or one-or-more tool-call requests (parallel calls supported).
    /// </summary>
    public sealed class LlmToolResponse
    {
        public bool IsToolCall  { get; }
        public string? TextContent { get; }
        /// <summary>All tool calls requested in this response (may be >1 for parallel calls).</summary>
        public IReadOnlyList<ToolCallRequest> ToolCalls { get; }
        /// <summary>First tool call — convenience accessor for the single-call case.</summary>
        public ToolCallRequest? ToolCall => ToolCalls.Count > 0 ? ToolCalls[0] : null;

        /// <summary>Single tool call or no tool call (backward-compatible constructor).</summary>
        public LlmToolResponse(bool isToolCall, string? textContent, ToolCallRequest? toolCall = null)
        {
            IsToolCall  = isToolCall;
            TextContent = textContent;
            ToolCalls   = toolCall != null
                ? new[] { toolCall }
                : Array.Empty<ToolCallRequest>();
        }

        /// <summary>Multiple parallel tool calls.</summary>
        public LlmToolResponse(bool isToolCall, string? textContent, IReadOnlyList<ToolCallRequest> toolCalls)
        {
            IsToolCall  = isToolCall;
            TextContent = textContent;
            ToolCalls   = toolCalls ?? Array.Empty<ToolCallRequest>();
        }
    }

    /// <summary>
    /// Unified tool interface — both FunctionCallingTool and McpClientTool implement this.
    /// </summary>
    public interface IAgentTool
    {
        /// <summary>Unique tool name used by the LLM when requesting a call.</summary>
        string     Name        { get; }

        /// <summary>Human/LLM-readable description of what the tool does.</summary>
        string     Description { get; }

        /// <summary>JSON Schema describing the tool's input parameters.</summary>
        JsonObject InputSchema { get; }

        /// <summary>Execute the tool with the given arguments.</summary>
        Task<AgentToolResult> ExecuteAsync(JsonObject args, CancellationToken ct = default);
    }

    /// <summary>
    /// LLM client interface — send messages with tool definitions, get back a completion.
    /// </summary>
    public interface ILlmClient
    {
        Task<LlmToolResponse> CompleteWithToolsAsync(
            IEnumerable<AgentMessage> messages,
            IEnumerable<IAgentTool>   tools,
            string?                   systemPrompt = null,
            CancellationToken         ct           = default);
    }
}
