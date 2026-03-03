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
        [Column("process_id")]
        public string ProcessId { get; set; }
        [Column("version")]
        public string Version { get; set; }
        [Column("activity_id")]
        public string ActivityId { get; set; }
        [Column("service_type")]
        public string ServiceType { get; set; }
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
        [Column("rag_search_strategy")]
        public string RagSearchStrategy { get; set; }
        [Column("rag_search_count")]
        public int? RagSearchCount { get; set; }
        [Column("rag_similarity_threshold")]
        public decimal? RagSimilarityThreshold { get; set; }
        [Column("rag_search_mode")]
        public string RagSearchMode { get; set; }
        [Column("rag_function")]
        public string RagFunction { get; set; }
        /// <summary>
        /// Embedding model provider id from ai_model_provider (model_type=vector_model)
        /// </summary>
        [Column("rag_embedding_model_id")]
        public int? RagEmbeddingModelId { get; set; }
        /// <summary>
        /// Legacy: embedding model name, used when RagEmbeddingModelId is not set
        /// </summary>
        [Column("rag_embedding_model")]
        public string RagEmbeddingModel { get; set; }
        /// <summary>
        /// Embedding output dimensions for RAG vector query (default 1536)
        /// </summary>
        [Column("rag_embedding_dimensions")]
        public int? RagEmbeddingDimensions { get; set; } = 1536;
        /// <summary>
        /// Multi-turn context memory: number of conversation turns to keep (default 10). Aligns with AI property "memory turns" in model parameters.
        /// </summary>
        [Column("memory_turns")]
        public int MemoryTurns { get; set; } = 10;
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public DateTime UpdatedDateTime { get; set; }
    }
}
