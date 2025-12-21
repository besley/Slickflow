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
