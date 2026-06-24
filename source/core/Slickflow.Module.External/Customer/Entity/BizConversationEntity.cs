using System;
using Newtonsoft.Json;

namespace Slickflow.Module.External.Customer.Entity
{
    /// <summary>
    /// Entity for Supabase table biz_conversation.
    /// One row per user message + AI response pair (customer–AI chat).
    /// </summary>
    public class BizConversationEntity
    {
        [JsonProperty("conversation_id")]
        public string ConversationId { get; set; }

        /// <summary>Table may use message_id as primary key instead of conversation_id.</summary>
        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        /// <summary>Session identifier for this conversation run; used to split/group rounds when reading history.</summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("process_instance_id")]
        public string ProcessInstanceId { get; set; }

        [JsonProperty("user_message")]
        public string UserMessage { get; set; }

        [JsonProperty("ai_response")]
        public string AiResponse { get; set; }

        [JsonProperty("industry_id")]
        public long? IndustryId { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
