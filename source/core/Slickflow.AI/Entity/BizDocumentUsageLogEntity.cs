using System;
using Newtonsoft.Json;

namespace Slickflow.AI.Entity
{
    /// <summary>
    /// Entity for Supabase table biz_document_usage_log.
    /// Records which documents were matched by RAG similarity search.
    /// </summary>
    public class BizDocumentUsageLogEntity
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("document_id")]
        public long DocumentId { get; set; }

        [JsonProperty("industry_id")]
        public long? IndustryId { get; set; }

        [JsonProperty("message_id")]
        public string MessageId { get; set; }

        [JsonProperty("customer_id")]
        public string CustomerId { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("similarity")]
        public double? Similarity { get; set; }

        [JsonProperty("query_text")]
        public string QueryText { get; set; }

        [JsonProperty("usage_context")]
        public object UsageContext { get; set; }

        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}
