using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Entity
{
    [Table("ai_activity_config")]
    public class AiActivityConfigEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("config_uuid")]
        public required string ConfigUUID { get; set; }
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("model_provider_id")]
        public int? ModelProviderId { get; set; }
        [Column("model_name")]
        public string ModelName { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("temperature")]
        public decimal Temperature { get; set; }
        [Column("max_tokens")]
        public int MaxTokens { get; set; }
        [Column("system_prompt")]
        public string SystemPrompt { get; set; }
        [Column("user_message")]
        public string UserMessage { get; set; }
        [Column("response_format")]
        public string ResponseFormat { get; set; }
        [Column("time_out")]
        public int Timeout { get; set; }
        [Column("max_retries")]
        public int MaxRetries { get; set; }
        [Column("error_handling")]
        public string ErrorHandling { get; set; }
        [Column("fallback_agent")]
        public string FallbackAgent { get; set; }
        [Column("log_level")]
        public string LogLevel { get; set; }
        [Column("custom_instructions")]
        public string CustomInstructions { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public DateTime UpdatedDateTime { get; set; }
    }
}
