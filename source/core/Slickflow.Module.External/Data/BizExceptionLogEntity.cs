using System;
using Newtonsoft.Json;

namespace Slickflow.Module.External.Data
{
    /// <summary>
    /// Entity for Supabase table biz_exception_log. Records exceptions when saving customer or conversation.
    /// Structure aligned with wf_log for consistency.
    /// </summary>
    public class BizExceptionLogEntity
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("event_type_id")]
        public int EventTypeId { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("severity")]
        public string Severity { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("stack_trace")]
        public string StackTrace { get; set; }

        [JsonProperty("inner_stack_trace")]
        public string InnerStackTrace { get; set; }

        [JsonProperty("request_data")]
        public string RequestData { get; set; }

        [JsonProperty("time_stamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
