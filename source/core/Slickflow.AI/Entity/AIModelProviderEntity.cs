using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.AI.Entity
{
    [Table("ai_model_provider")]
    public class AiModelProviderEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("model_provider")]
        public string ModelProvider { get; set; }
        /// <summary>
        /// Model name/identifier e.g. deepseek-v3, gpt-4o
        /// </summary>
        [Column("model_name")]
        public string ModelName { get; set; }
        /// <summary>
        /// Model type (English): text_generation, multimodal, reasoning_model, audio_speech, video_generation, image_generation, vector_model, ranking_model, other
        /// </summary>
        [Column("model_type")]
        public string ModelType { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("base_url")]
        public string BaseUrl { get; set; }
        [Column("api_uuid")]
        public string ApiUUID { get; set; }
        [Column("api_key")]
        public string ApiKey { get; set; }
        [Column("is_active")]
        public Boolean IsActive { get; set; }
        [Column("created_datetime")]
        public DateTime CreatedDateTime { get; set; }
        [Column("updated_datetime")]
        public DateTime UpdatedDateTime { get; set; }
    }
}
