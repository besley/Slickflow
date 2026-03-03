using Slickflow.AI.Entity;
using System;
using System.Collections.Generic;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// Knowledge Base Document Service Interface
    /// 知识库文档服务接口
    /// </summary>
    public interface IKbDocumentService
    {
        /// <summary>
        /// Get document by ID
        /// 根据ID获取文档
        /// </summary>
        KbDocumentEntity GetDocumentById(long id);

        /// <summary>
        /// Get all documents
        /// 获取所有文档
        /// </summary>
        List<KbDocumentEntity> GetAllDocuments();

        /// <summary>
        /// Optimized vector similarity search with two-tier strategy
        /// 优化的向量相似度检索（两级策略）
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding (JSON array string)</param>
        /// <param name="highThreshold">High similarity threshold (default: 0.8)</param>
        /// <param name="highLimit">Maximum results for high similarity search (default: 3)</param>
        /// <param name="lowThreshold">Low similarity threshold for fallback (default: 0.6)</param>
        /// <param name="lowLimit">Maximum results for low similarity search (default: 2)</param>
        /// <returns>List of similar documents</returns>
        List<KbDocumentEntity> VectorSimilaritySearchOptimized(string queryEmbedding, 
            decimal highThreshold = 0.8m, int highLimit = 3, 
            decimal lowThreshold = 0.6m, int lowLimit = 2);

        /// <summary>
        /// Vector similarity search
        /// 向量相似度检索
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding (JSON array string)</param>
        /// <param name="limit">Maximum number of results</param>
        /// <param name="threshold">Similarity threshold (0-1)</param>
        /// <returns>List of similar documents</returns>
        List<KbDocumentEntity> VectorSimilaritySearch(string queryEmbedding, int limit = 10, decimal? threshold = null);

        /// <summary>
        /// Hybrid search combining vector similarity and metadata filtering
        /// 混合检索：结合向量相似度和元数据过滤
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding</param>
        /// <param name="metadataFilter">Metadata filter as JSON string</param>
        /// <param name="limit">Maximum number of results</param>
        /// <param name="threshold">Similarity threshold</param>
        /// <returns>List of similar documents</returns>
        List<KbDocumentEntity> HybridSearch(string queryEmbedding, string metadataFilter = null, int limit = 10, decimal? threshold = null);

        /// <summary>
        /// Insert document
        /// 插入文档
        /// </summary>
        long InsertDocument(KbDocumentEntity entity);

        /// <summary>
        /// Update document
        /// 更新文档
        /// </summary>
        void UpdateDocument(KbDocumentEntity entity);

        /// <summary>
        /// Delete document
        /// 删除文档
        /// </summary>
        void DeleteDocument(long id);

        /// <summary>
        /// Get all table names from Supabase database
        /// 获取Supabase数据库中的所有表名称
        /// </summary>
        List<string> GetTableNames();
    }
}
