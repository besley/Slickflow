using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;

namespace Slickflow.AI.Agent.Core
{
    /// <summary>
    /// Abstract base for Agent nodes.
    /// Subclasses declare tools, system prompt, and input/output variable keys.
    /// The ReAct loop (Reason → Act → Observe) is fully implemented here.
    /// </summary>
    public abstract class AgentNodeBase
    {
        /// <summary>Maximum number of ReAct iterations to prevent infinite loops.</summary>
        protected virtual int MaxIterations => 10;

        /// <summary>Workflow variable key to read the user input from.</summary>
        protected abstract string InputVariableKey { get; }

        /// <summary>Workflow variable key to write the final answer to.</summary>
        protected abstract string OutputVariableKey { get; }

        /// <summary>System prompt defining the agent's role and behavior.</summary>
        protected abstract string SystemPrompt { get; }

        /// <summary>Tool list available to this agent.</summary>
        protected abstract IReadOnlyList<IAgentTool> Tools { get; }

        /// <summary>
        /// Entry point called by AgentMultiTurnService.
        /// Runs the ReAct loop and returns the final text output.
        /// </summary>
        public async Task<string> ExecuteReActAsync(
            IAgentContext    ctx,
            ILlmClient       llmClient,
            CancellationToken ct = default)
        {
            var userInput = ctx.GetVariable<string>(InputVariableKey);
            if (string.IsNullOrWhiteSpace(userInput))
                userInput = ctx.GetVariable<string>("user_message") ?? string.Empty;

            Console.WriteLine($"[Agent] {GetType().Name} starting. Input key='{InputVariableKey}', length={userInput?.Length ?? 0}");

            ctx.AppendMessage(new AgentMessage("user", userInput ?? string.Empty));

            var result = await RunReActLoopAsync(ctx, llmClient, ct);
            ctx.SetVariable(OutputVariableKey, result);

            Console.WriteLine($"[Agent] {GetType().Name} finished. Output key='{OutputVariableKey}', length={result?.Length ?? 0}");
            return result ?? string.Empty;
        }

        // ─── ReAct loop ──────────────────────────────────────────

        private async Task<string> RunReActLoopAsync(
            IAgentContext     ctx,
            ILlmClient        llmClient,
            CancellationToken ct)
        {
            for (int i = 0; i < MaxIterations; i++)
            {
                Console.WriteLine($"[Agent] ReAct iteration {i + 1}/{MaxIterations}");

                var response = await llmClient.CompleteWithToolsAsync(
                    ctx.History, Tools, SystemPrompt, ct);

                if (!response.IsToolCall || response.ToolCalls.Count == 0)
                {
                    // Final answer
                    var finalText = response.TextContent ?? string.Empty;
                    ctx.AppendMessage(new AgentMessage("assistant", finalText));
                    Console.WriteLine($"[Agent] Final answer produced ({finalText.Length} chars)");
                    return finalText;
                }

                if (response.ToolCalls.Count == 1)
                {
                    // Single tool call — existing behavior
                    var toolCall = response.ToolCalls[0];
                    Console.WriteLine($"[Agent] Calling tool '{toolCall.ToolName}' | args={toolCall.Args?.ToJsonString()}");

                    ctx.AppendMessage(new AgentMessage(
                        "assistant",
                        $"[Tool call: {toolCall.ToolName}]",
                        toolCall.Id,
                        toolCall.ToolName));

                    var toolResult = await InvokeToolAsync(toolCall, ct);

                    ctx.AppendMessage(new AgentMessage(
                        "tool",
                        toolResult.Content,
                        toolCall.Id,
                        toolCall.ToolName));

                    var preview = toolResult.Content.Length > 200
                        ? toolResult.Content.Substring(0, 200) + "..."
                        : toolResult.Content;
                    Console.WriteLine($"[Agent] Tool '{toolCall.ToolName}' result: {preview}");
                }
                else
                {
                    // Parallel tool calls — one assistant message, then one tool message per call
                    Console.WriteLine($"[Agent] Parallel tool calls ({response.ToolCalls.Count}): " +
                        string.Join(", ", response.ToolCalls.Select(t => t.ToolName)));

                    ctx.AppendMessage(new AgentMessage("assistant", response.ToolCalls));

                    foreach (var toolCall in response.ToolCalls)
                    {
                        Console.WriteLine($"[Agent] Calling tool '{toolCall.ToolName}' | args={toolCall.Args?.ToJsonString()}");
                        var toolResult = await InvokeToolAsync(toolCall, ct);
                        ctx.AppendMessage(new AgentMessage(
                            "tool",
                            toolResult.Content,
                            toolCall.Id,
                            toolCall.ToolName));
                        var preview = toolResult.Content.Length > 200
                            ? toolResult.Content.Substring(0, 200) + "..."
                            : toolResult.Content;
                        Console.WriteLine($"[Agent] Tool '{toolCall.ToolName}' result: {preview}");
                    }
                }
            }

            Console.WriteLine($"[Agent] Reached max iterations ({MaxIterations}), returning fallback.");
            return "Agent reached maximum iterations without producing a final answer.";
        }

        // ─── Tool dispatch ────────────────────────────────────────

        private async Task<AgentToolResult> InvokeToolAsync(
            ToolCallRequest   req,
            CancellationToken ct)
        {
            var tool = Tools?.FirstOrDefault(t =>
                string.Equals(t.Name, req.ToolName, StringComparison.OrdinalIgnoreCase));

            if (tool == null)
            {
                var errMsg = $"Tool '{req.ToolName}' not registered for this agent.";
                Console.WriteLine($"[Agent] ERROR: {errMsg}");
                return new AgentToolResult(false, string.Empty, errMsg);
            }

            try
            {
                return await tool.ExecuteAsync(req.Args, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Agent] Tool '{req.ToolName}' threw exception: {ex.Message}");
                return new AgentToolResult(false, string.Empty, ex.Message);
            }
        }
    }
}
