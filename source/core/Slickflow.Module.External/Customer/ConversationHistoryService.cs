using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Slickflow.Engine.Common;
using Slickflow.Engine.External;
using Slickflow.Module.External.Customer.Entity;
using Slickflow.Module.External.Data;

namespace Slickflow.Module.External.Customer
{
    /// <summary>
    /// Rebuilds multi-turn chat_history from persisted biz_conversation rows in Supabase.
    /// This service is intended to run BEFORE any AI (RAG / LLM) node in the workflow,
    /// so that AutoExecutionContext.Variables["chat_history"] is populated for every run,
    /// even though AutoExecutionContext is recreated per request.
    /// </summary>
    public class ConversationHistoryService : ExternalServiceBase, IExternalService
    {
        /// <summary>
        /// Lightweight DTO used for chat_history JSON:
        /// [{ "role": "user"|"assistant", "content": "..." }, ...]
        /// </summary>
        private class HistoryItem
        {
            [JsonProperty("role")]
            public string Role { get; set; }

            [JsonProperty("content")]
            public string Content { get; set; }
        }

        public override void Execute()
        {
            if (EventService == null)
            {
                System.Diagnostics.Debug.WriteLine("ConversationHistoryService: EventService is null, skip.");
                return;
            }

            try
            {
                // Load history by customer_id and session_id so we get only the current session's messages (latest history).
                var customerId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "customer_id");
                var sessionId = EventService.GetVariable(ProcessVariableScopeEnum.Process, "session_id");
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    System.Diagnostics.Debug.WriteLine("ConversationHistoryService: customer_id is empty, skip.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    System.Diagnostics.Debug.WriteLine("ConversationHistoryService: session_id is empty, skip (history is loaded by customer_id + session_id).");
                    return;
                }

                var repo = new SupabaseConversationRepository();
                var historyRows = repo.GetByCustomerIdAndSessionIdAsync(customerId.Trim(), sessionId.Trim(), limit: 40).GetAwaiter().GetResult();

                if (historyRows == null || historyRows.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("ConversationHistoryService: no conversation rows found, skip.");
                    return;
                }

                // Supabase returns rows in descending order; reverse to chronological.
                historyRows.Reverse();

                var list = new List<HistoryItem>();
                foreach (var row in historyRows)
                {
                    if (!string.IsNullOrWhiteSpace(row.UserMessage))
                    {
                        list.Add(new HistoryItem
                        {
                            Role = "user",
                            Content = row.UserMessage
                        });
                    }

                    if (!string.IsNullOrWhiteSpace(row.AiResponse))
                    {
                        list.Add(new HistoryItem
                        {
                            Role = "assistant",
                            Content = row.AiResponse
                        });
                    }
                }

                if (list.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("ConversationHistoryService: built history list is empty, skip.");
                    return;
                }

                var json = JsonConvert.SerializeObject(list);
                EventService.SaveVariable(ProcessVariableScopeEnum.Process, "chat_history", json);
                System.Diagnostics.Debug.WriteLine("ConversationHistoryService: chat_history variable updated from biz_conversation.");
            }
            catch (Exception ex)
            {
                // Do not break workflow when history reconstruction fails; AI nodes can still run but without history.
                System.Diagnostics.Debug.WriteLine($"ConversationHistoryService: failed to rebuild chat_history: {ex.Message}");
            }
        }
    }
}

