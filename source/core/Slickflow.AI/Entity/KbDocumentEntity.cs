using System;
using System.Collections.Generic;

namespace Slickflow.AI.Entity
{
    /// <summary>
    /// Knowledge Base Document Entity
    /// 知识库文档实体
    /// </summary>
    [Table("biz_documents")]
    public class KbDocumentEntity
    {
        [Column("id")]
        public long Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("metadata")]
        public string Metadata { get; set; }

        /// <summary>
        /// Industry ID for filtering knowledge base by industry (e.g. 电动汽车新能源).
        /// </summary>
        [Column("industry_id")]
        public long? IndustryId { get; set; }

        /// <summary>
        /// Vector embedding (stored as JSON string, will be converted to float[] when needed)
        /// 向量嵌入（存储为JSON字符串，需要时转换为float[]）
        /// </summary>
        [Column("embedding")]
        public string Embedding { get; set; }
    }
}
