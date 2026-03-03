using Slickflow.AI.Entity;
using Slickflow.AI.Manager;
using Slickflow.AI.Configuration;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Slickflow.AI.Service
{
    /// <summary>
    /// Knowledge Base Document Service
    /// 知识库文档服务实现
    /// </summary>
    public class KbDocumentService : IKbDocumentService
    {
        private AiAppConfigProviderOptions _configOptions;

        /// <summary>
        /// Constructor with configuration options
        /// 带配置选项的构造函数
        /// </summary>
        public KbDocumentService(AiAppConfigProviderOptions configOptions = null)
        {
            _configOptions = configOptions;
        }
        #region Get Document Data 获取文档数据
        /// <summary>
        /// Get document by ID
        /// 根据ID获取文档
        /// </summary>
        public KbDocumentEntity GetDocumentById(long id)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var entity = documentManager.GetById(id);
            return entity;
        }

        /// <summary>
        /// Get all documents
        /// 获取所有文档
        /// </summary>
        public List<KbDocumentEntity> GetAllDocuments()
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var list = documentManager.GetAll();
            return list;
        }
        #endregion

        #region Vector Search 向量检索
        /// <summary>
        /// Optimized vector similarity search with two-tier strategy
        /// 优化的向量相似度检索（两级策略）
        /// </summary>
        public List<KbDocumentEntity> VectorSimilaritySearchOptimized(string queryEmbedding, 
            decimal highThreshold = 0.8m, int highLimit = 3, 
            decimal lowThreshold = 0.6m, int lowLimit = 2)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var results = documentManager.VectorSimilaritySearchOptimized(queryEmbedding, highThreshold, highLimit, lowThreshold, lowLimit);
            return results;
        }

        /// <summary>
        /// Vector similarity search
        /// 向量相似度检索
        /// </summary>
        public List<KbDocumentEntity> VectorSimilaritySearch(string queryEmbedding, int limit = 10, decimal? threshold = null)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var results = documentManager.VectorSimilaritySearch(queryEmbedding, limit, threshold);
            return results;
        }

        /// <summary>
        /// Hybrid search combining vector similarity and metadata filtering
        /// 混合检索：结合向量相似度和元数据过滤
        /// </summary>
        public List<KbDocumentEntity> HybridSearch(string queryEmbedding, string metadataFilter = null, int limit = 10, decimal? threshold = null)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var results = documentManager.HybridSearch(queryEmbedding, metadataFilter, limit, threshold);
            return results;
        }
        #endregion

        #region CRUD Operations 增删改查操作
        /// <summary>
        /// Insert document
        /// 插入文档
        /// </summary>
        public long InsertDocument(KbDocumentEntity entity)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            long docId = documentManager.Insert(entity);
            return docId;
        }

        /// <summary>
        /// Update document
        /// 更新文档
        /// </summary>
        public void UpdateDocument(KbDocumentEntity entity)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            documentManager.Update(entity);
        }

        /// <summary>
        /// Delete document
        /// 删除文档
        /// </summary>
        public void DeleteDocument(long id)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            documentManager.Delete(id);
        }
        #endregion

        #region Test Connection 测试连接
        /// <summary>
        /// Test connection by reading data from biz_documents table
        /// 通过读取biz_documents表数据来测试连接
        /// </summary>
        public List<KbDocumentEntity> TestReadBizDocuments(int limit = 5)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var documents = documentManager.TestReadBizDocuments(limit);
            return documents;
        }
        #endregion

        #region Get Table Names 获取表名称列表
        /// <summary>
        /// Get all table names from Supabase database
        /// 获取Supabase数据库中的所有表名称
        /// </summary>
        public List<string> GetTableNames()
        {
            var documentManager = new KbDocumentManager(_configOptions);
            var tableNames = documentManager.GetTableNames();
            return tableNames;
        }
        #endregion

        #region Save Document 保存文档
        /// <summary>
        /// Save document (insert or update)
        /// 保存文档（插入或更新）
        /// </summary>
        public KbDocumentEntity SaveDocument(KbDocumentEntity entity)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            return documentManager.SaveDocument(entity);
        }
        #endregion

        #region Search Documents 搜索文档
        /// <summary>
        /// Search documents using match_documents_optimized
        /// 使用match_documents_optimized搜索文档
        /// </summary>
        public List<KbDocumentEntity> SearchDocuments(string query, float threshold = 0.7f, int count = 5)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            return documentManager.SearchDocuments(query, threshold, count);
        }
        #endregion

        #region Generate Metadata 生成元数据
        /// <summary>
        /// Generate metadata JSON from Question, Intent, and Answer
        /// 根据Question、Intent和Answer生成元数据JSON
        /// </summary>
        public JObject GenerateMetadata(string question, string intent, string answer)
        {
            var documentManager = new KbDocumentManager(_configOptions);
            return documentManager.GenerateMetadata(question, intent, answer);
        }
        #endregion
    }
}
