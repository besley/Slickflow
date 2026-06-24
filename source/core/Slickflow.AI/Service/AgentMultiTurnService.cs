using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Slickflow.AI.Agent.Core;
using Slickflow.AI.Agent.Llm;
using Slickflow.AI.Agent.Memory;
using Slickflow.AI.Agent.Registry;
using Slickflow.AI.Configuration;
using Slickflow.AI.Entity;
using Slickflow.AI.Manager;
using Slickflow.WebUtility;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// Agent-type AI node: executes a full ReAct (Reason → Act → Observe) loop.
    ///
    /// Workflow:
    ///   1. Resolve tool list from AgentToolRegistry using axConfig.ActivityId
    ///   2. Build AgentContext from workflow variables (mediaFileList)
    ///   3. Create OpenAIToolCallLlmClient from the model provider config
    ///   4. Run ReAct loop via an inline AgentNode
    ///   5. Return final text answer
    ///
    /// If no tools are registered for the activity, falls back to a plain LLM call
    /// via LlmMultiTurnService (non-breaking for activities not yet using ReAct).
    /// </summary>
    public class AgentMultiTurnService
    {
        private readonly LlmMultiTurnService _llmMultiTurn = new LlmMultiTurnService();

        /// <summary>
        /// Main entry called by WorkflowActivityExecutor for Agent-type nodes.
        /// </summary>
        public async Task<string> InvokeWithHistoryAsync(
            AiActivityConfigEntity   axConfig,
            IList<MultiMediaFile>    mediaFileList,
            IList<ChatHistoryMessage> historyChatMessageList,
            CancellationToken        ct = default)
        {
            if (axConfig == null)
                throw new ArgumentNullException(nameof(axConfig));

            // Check whether tools are registered for this activity
            if (!AgentToolRegistry.Global.TryGetTools(axConfig.ActivityId, out var tools)
                || tools == null || tools.Count == 0)
            {
                Console.WriteLine($"[AgentMultiTurnService] No tools registered for activity '{axConfig.ActivityId}'. Falling back to plain LLM.");
                return await _llmMultiTurn.InvokeWithHistoryAsync(axConfig, mediaFileList, historyChatMessageList);
            }

            // Resolve model provider
            if (!axConfig.ModelProviderId.HasValue)
                throw new InvalidOperationException($"ModelProviderId is not set for ActivityId={axConfig.ActivityId}");

            var modelProviderManager = new AiModelProviderManager();
            var modelProvider        = modelProviderManager.GetById(axConfig.ModelProviderId.Value);

            if (modelProvider == null)
                throw new InvalidOperationException($"AIModelProvider not found for id={axConfig.ModelProviderId.Value}");
            if (!modelProvider.IsActive)
                throw new InvalidOperationException($"AIModelProvider id={axConfig.ModelProviderId.Value} is not active");
            if (string.IsNullOrWhiteSpace(modelProvider.BaseUrl) || string.IsNullOrWhiteSpace(modelProvider.ApiKey))
                throw new InvalidOperationException("BaseUrl and ApiKey are required in AIModelProvider");

            var baseUrl       = modelProvider.BaseUrl.TrimEnd('/');
            var decryptedKey  = ApiKeyCryptoHelper.Decrypt(modelProvider.ApiKey, modelProvider.ApiUUID);

            // Build AgentContext from mediaFileList (name → content string variables)
            var variables = BuildVariables(mediaFileList);
            var ctx = new AgentContext(
                processInstanceId: axConfig.ProcessId ?? string.Empty,
                activityId:        axConfig.ActivityId,
                variables:         variables);

            // Seed user_message into the context
            EnsureUserMessage(ctx, axConfig, mediaFileList);

            // Create the LLM client
            var llmClient = new OpenAIToolCallLlmClient(baseUrl, decryptedKey, axConfig);

            // Create an inline adapter node with the registered tools and system prompt
            var agentNode = new InlineAgentNode(
                systemPrompt:       axConfig.SystemPrompt ?? string.Empty,
                tools:              tools,
                inputVariableKey:   "user_message",
                outputVariableKey:  "agent_result",
                maxIterations:      axConfig.MemoryTurns > 0 ? axConfig.MemoryTurns : 10);

            Console.WriteLine($"[AgentMultiTurnService] Running ReAct for activity='{axConfig.ActivityId}' tools={tools.Count}");

            var result = await agentNode.ExecuteReActAsync(ctx, llmClient, ct);

            // Record output in shared memory so downstream agents can access it
            AgentConversationMemory.RecordOutput(
                processInstanceId: axConfig.ProcessId ?? string.Empty,
                activityId:        axConfig.ActivityId,
                variableName:      "agent_result",
                value:             result ?? string.Empty);

            return result ?? string.Empty;
        }

        // ─── Static helper: register tools for an activity ────────

        /// <summary>
        /// Convenience method to register tools for an activity.
        /// Delegates to AgentToolRegistry.Global.Register.
        /// </summary>
        public static void RegisterDefaultTools(string activityId, IReadOnlyList<IAgentTool> tools)
            => AgentToolRegistry.Global.Register(activityId, tools);

        // ─── Private helpers ──────────────────────────────────────

        private static Dictionary<string, string> BuildVariables(IList<MultiMediaFile> mediaFileList)
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (mediaFileList == null) return dict;

            foreach (var f in mediaFileList)
            {
                if (!string.IsNullOrWhiteSpace(f?.Name))
                    dict[f.Name] = f.Content ?? string.Empty;
            }
            return dict;
        }

        private static void EnsureUserMessage(
            AgentContext           ctx,
            AiActivityConfigEntity axConfig,
            IList<MultiMediaFile>  mediaFileList)
        {
            // Priority: first mediaFile content → axConfig.UserMessage
            if (ctx.GetVariable<string>("user_message") == null
                || ctx.GetVariable<string>("user_message") == string.Empty)
            {
                string? userMsg = null;
                if (mediaFileList != null && mediaFileList.Count > 0)
                    userMsg = mediaFileList[0]?.Content;
                if (string.IsNullOrWhiteSpace(userMsg))
                    userMsg = axConfig.UserMessage;
                if (!string.IsNullOrWhiteSpace(userMsg))
                    ctx.SetVariable("user_message", userMsg);
            }
        }

        // ─── Inline agent node (no subclass required) ─────────────

        /// <summary>
        /// A concrete AgentNodeBase that is fully configured at construction time,
        /// so callers do not need to create a dedicated subclass per activity.
        /// </summary>
        private sealed class InlineAgentNode : AgentNodeBase
        {
            private readonly string                   _systemPrompt;
            private readonly IReadOnlyList<IAgentTool> _tools;
            private readonly string                   _inputKey;
            private readonly string                   _outputKey;
            private readonly int                      _maxIter;

            protected override string                   SystemPrompt      => _systemPrompt;
            protected override IReadOnlyList<IAgentTool> Tools             => _tools;
            protected override string                   InputVariableKey  => _inputKey;
            protected override string                   OutputVariableKey => _outputKey;
            protected override int                      MaxIterations     => _maxIter;

            public InlineAgentNode(
                string                   systemPrompt,
                IReadOnlyList<IAgentTool> tools,
                string                   inputVariableKey,
                string                   outputVariableKey,
                int                      maxIterations = 10)
            {
                _systemPrompt = systemPrompt;
                _tools        = tools;
                _inputKey     = inputVariableKey;
                _outputKey    = outputVariableKey;
                _maxIter      = maxIterations > 0 ? maxIterations : 10;
            }
        }
    }
}
