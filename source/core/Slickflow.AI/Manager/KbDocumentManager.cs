using Dapper;
using Slickflow.Data;
using Slickflow.AI.Entity;
using Slickflow.AI.Configuration;
using Slickflow.AI.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NpgsqlTypes;
using Slickflow.AI.Utility;

namespace Slickflow.AI.Manager
{
    /// <summary>
    /// Knowledge Base Document Manager
    /// 知识库文档管理类
    /// </summary>
    public class KbDocumentManager : ManagerBase
    {
        private AiAppConfigProviderOptions _configOptions;

        /// <summary>
        /// Constructor with configuration options
        /// 带配置选项的构造函数
        /// </summary>
        public KbDocumentManager(AiAppConfigProviderOptions configOptions = null)
        {
            _configOptions = configOptions;
        }

        /// <summary>
        /// Create Supabase connection (DEPRECATED - Use HttpClient instead)
        /// 创建 Supabase 数据库连接（已弃用 - 请使用 HttpClient）
        /// This method has been removed. All Supabase operations should use HttpClient REST API.
        /// 此方法已被移除。所有 Supabase 操作应使用 HttpClient REST API。
        /// </summary>
        [Obsolete("This method is deprecated. Use HttpClient REST API instead.")]
        private IDbConnection CreateSupabaseConnection()
        {
            throw new NotImplementedException("CreateSupabaseConnection() has been removed. All Supabase operations should use HttpClient REST API instead. Please refactor calling methods to use HttpClient.");
        }
        #region Get Document Data 获取文档数据
        /// <summary>
        /// Get document by ID
        /// 根据ID获取文档
        /// </summary>
        public KbDocumentEntity GetById(long id)
        {
            IDbConnection conn = CreateSupabaseConnection();
            try
            {
                var entity = Repository.GetById<KbDocumentEntity>(id);
                return entity;
            }
            finally
            {
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Get document by ID with transaction
        /// 根据ID获取文档（带事务）
        /// </summary>
        public KbDocumentEntity GetById(IDbConnection conn, long id, IDbTransaction trans)
        {
            var entity = Repository.GetById<KbDocumentEntity>(conn, id, trans);
            return entity;
        }

        /// <summary>
        /// Get all documents using HttpClient REST API
        /// 使用HttpClient REST API获取所有文档
        /// </summary>
        public List<KbDocumentEntity> GetAll()
        {
            if (_configOptions == null)
            {
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
            {
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
            {
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            }

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var response = httpClient.GetAsync("/rest/v1/biz_documents?select=id,content,metadata,embedding&order=id").Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new InvalidOperationException($"Failed to read biz_documents table: HTTP {response.StatusCode} - {errorContent}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                
                // Use intermediate class to handle metadata as object or string
                // 使用中间类处理 metadata 可能是对象或字符串的情况
                var rawDocuments = JsonConvert.DeserializeObject<List<dynamic>>(content);
                var documents = new List<KbDocumentEntity>();
                
                if (rawDocuments != null)
                {
                    foreach (var rawDoc in rawDocuments)
                    {
                        var doc = new KbDocumentEntity
                        {
                            Id = rawDoc.id != null ? Convert.ToInt64(rawDoc.id) : 0,
                            Content = rawDoc.content?.ToString() ?? string.Empty,
                            Embedding = rawDoc.embedding?.ToString() ?? string.Empty
                        };
                        
                        // Handle metadata - it might be an object or a string
                        // 处理 metadata - 可能是对象或字符串
                        if (rawDoc.metadata != null)
                        {
                            if (rawDoc.metadata is JObject || rawDoc.metadata is JToken)
                            {
                                // If it's a JSON object, serialize it to string
                                // 如果是 JSON 对象，序列化为字符串
                                doc.Metadata = JsonConvert.SerializeObject(rawDoc.metadata);
                            }
                            else
                            {
                                // If it's already a string, use it directly
                                // 如果已经是字符串，直接使用
                                doc.Metadata = rawDoc.metadata.ToString();
                            }
                        }
                        
                        documents.Add(doc);
                    }
                }

                return documents;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read biz_documents table: {ex.Message}", ex);
            }
        }
        #endregion

        #region Save Document 保存文档
        /// <summary>
        /// Save document (insert or update) using HttpClient REST API
        /// 使用HttpClient REST API保存文档（插入或更新）
        /// </summary>
        public KbDocumentEntity SaveDocument(KbDocumentEntity entity)
        {
            if (_configOptions == null)
            {
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
            {
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
            {
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            }

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            try
            {
                object? metaVal = new { };
                if (!string.IsNullOrEmpty(entity.Metadata))
                {
                    try { metaVal = JToken.Parse(entity.Metadata); }
                    catch { metaVal = entity.Metadata; }
                }
                var payload = new Dictionary<string, object?>
                {
                    ["content"] = entity.Content,
                    ["metadata"] = metaVal
                };
                if (entity.IndustryId.HasValue)
                    payload["industry_id"] = entity.IndustryId.Value;

                var json = JsonConvert.SerializeObject(payload);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (entity.Id > 0)
                {
                    // Update
                    response = httpClient.PatchAsync($"/rest/v1/biz_documents?id=eq.{entity.Id}", content).Result;
                }
                else
                {
                    // Insert
                    response = httpClient.PostAsync("/rest/v1/biz_documents", content).Result;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new InvalidOperationException($"Failed to save document: HTTP {response.StatusCode} - {errorContent}");
                }

                var responseContent = response.Content.ReadAsStringAsync().Result;
                // Supabase returns metadata as JSON object (jsonb), not string - deserialize to JArray to avoid type mismatch
                var savedRows = JsonConvert.DeserializeObject<List<JObject>>(responseContent);
                if (savedRows == null || savedRows.Count == 0)
                    return entity;
                var first = savedRows[0];
                entity.Id = first["id"]?.Value<long>() ?? 0;
                return entity;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to save document: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clear all embedding fields in biz_documents (set to null)
        /// 清空 biz_documents 表中所有文档的 embedding 字段
        /// </summary>
        public async Task ClearAllEmbeddingsAsync()
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");

            var documents = GetAll();
            if (documents == null || documents.Count == 0)
                return;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=minimal");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(new { embedding = (object)null });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            foreach (var doc in documents)
            {
                var response = await httpClient.PatchAsync($"/rest/v1/biz_documents?id=eq.{doc.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Failed to clear embedding for Id={doc.Id}: HTTP {response.StatusCode} - {errorContent}");
                }
            }
        }

        /// <summary>
        /// Delete documents where id > minId (e.g. keep first 50 records, delete the rest)
        /// 删除 id 大于 minId 的文档，保留前 minId 条
        /// </summary>
        public async Task<int> DeleteDocumentsWhereIdGreaterThanAsync(long minId)
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.DeleteAsync($"/rest/v1/biz_documents?id=gt.{minId}");
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to delete documents: HTTP {response.StatusCode} - {err}");
            }
            var content = await response.Content.ReadAsStringAsync();
            var deleted = string.IsNullOrEmpty(content) ? 0 : JsonConvert.DeserializeObject<List<JObject>>(content)?.Count ?? 0;
            return deleted;
        }

        /// <summary>
        /// Delete documents by industry IDs (e.g. industry_id > 1 for seed cleanup)
        /// 按行业ID删除文档，用于清理非EV行业的知识库样本
        /// </summary>
        public async Task<int> DeleteDocumentsByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            var ids = industryIds.ToList();
            if (ids.Count == 0) return 0;

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var filter = string.Join(",", ids);
            var response = await httpClient.DeleteAsync($"/rest/v1/biz_documents?industry_id=in.({filter})");
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to delete documents: HTTP {response.StatusCode} - {err}");
            }
            var content = await response.Content.ReadAsStringAsync();
            var deleted = string.IsNullOrEmpty(content) ? 0 : JsonConvert.DeserializeObject<List<JObject>>(content)?.Count ?? 0;
            return deleted;
        }

        /// <summary>
        /// Get documents by industry IDs that have null/empty embedding (need embedding generation)
        /// 获取指定行业且 embedding 为空的文档，用于断点续传
        /// </summary>
        public async Task<List<(long Id, string Content)>> GetDocumentsWithNullEmbeddingByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            var ids = industryIds.ToList();
            if (ids.Count == 0) return new List<(long, string)>();

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var filter = string.Join(",", ids);
            var url = $"/rest/v1/biz_documents?industry_id=in.({filter})&embedding=is.null&select=id,content&order=id";
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to get documents: HTTP {response.StatusCode} - {err}");
            }
            var json = await response.Content.ReadAsStringAsync();
            var rows = JsonConvert.DeserializeObject<List<JObject>>(json);
            var result = new List<(long, string)>();
            if (rows != null)
                foreach (var r in rows)
                {
                    var id = r["id"]?.Value<long>() ?? 0;
                    var content = r["content"]?.ToString() ?? string.Empty;
                    if (id > 0) result.Add((id, content));
                }
            return result;
        }

        /// <summary>
        /// Get document count per industry (for industry_ids), returns industryId -> count
        /// 获取各行业文档数量
        /// </summary>
        public async Task<Dictionary<long, int>> GetDocumentCountByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            var ids = industryIds.ToList();
            if (ids.Count == 0) return new Dictionary<long, int>();

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = new Dictionary<long, int>();
            foreach (var industryId in ids)
            {
                var response = await httpClient.GetAsync($"/rest/v1/biz_documents?industry_id=eq.{industryId}&select=id");
                if (!response.IsSuccessStatusCode)
                {
                    var err = await response.Content.ReadAsStringAsync();
                    throw new InvalidOperationException($"Failed to get document count: HTTP {response.StatusCode} - {err}");
                }
                var json = await response.Content.ReadAsStringAsync();
                var rows = JsonConvert.DeserializeObject<List<JObject>>(json);
                result[industryId] = rows?.Count ?? 0;
            }
            return result;
        }

        /// <summary>
        /// Check if seed is complete: all target industries have 50 docs each with non-null embedding
        /// 检查知识库样本是否已全部生成完成
        /// </summary>
        public async Task<bool> IsSeedCompleteAsync(IEnumerable<long> industryIds, int expectedPerIndustry = 50)
        {
            var counts = await GetDocumentCountByIndustryIdsAsync(industryIds);
            foreach (var industryId in industryIds)
            {
                if (!counts.TryGetValue(industryId, out var c) || c < expectedPerIndustry)
                    return false;
            }
            var needEmbedding = await GetDocumentsWithNullEmbeddingByIndustryIdsAsync(industryIds);
            return needEmbedding.Count == 0;
        }

        /// <summary>
        /// Update document embedding only via REST API (for batch embedding regeneration)
        /// 仅更新文档 embedding 字段（用于批量重新生成向量）
        /// </summary>
        public async Task UpdateEmbeddingAsync(long documentId, float[] embedding)
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            if (embedding == null || embedding.Length == 0)
                throw new ArgumentException("Embedding cannot be null or empty", nameof(embedding));

            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=minimal");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonConvert.SerializeObject(new { embedding });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"/rest/v1/biz_documents?id=eq.{documentId}", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to update embedding: HTTP {response.StatusCode} - {errorContent}");
            }
        }
        #endregion

        #region Search Documents 搜索文档
        /// <summary>
        /// Search documents using match_documents_optimized RPC function
        /// 使用match_documents_optimized RPC函数搜索文档
        /// </summary>
        public List<KbDocumentEntity> SearchDocuments(string query, float threshold = 0.7f, int count = 5)
        {
            if (_configOptions == null)
            {
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
            {
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
            {
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            }

            // Generate embedding for query
            // Check OpenAI configuration
            if (_configOptions.OpenAI == null || string.IsNullOrWhiteSpace(_configOptions.OpenAI.ApiKey))
            {
                throw new InvalidOperationException("OpenAI ApiKey is not configured.");
            }

            var ragService = new SupabaseRagService(_configOptions);
            var embeddingTask = EmbeddingGenUtility.GenerateEmbeddingContent(_configOptions.OpenAI.ApiKey, query);
            var queryEmbedding = embeddingTask.Result;

            // Call match_documents_optimized
            var matchTask = ragService.MatchDocumentsOptimizedAsync(
                queryEmbedding,
                matchThresholdHigh: threshold,
                matchThresholdLow: threshold * 0.8f,
                matchCountHigh: count,
                matchCountLow: count
            );
            var matchResults = matchTask.Result;

            // Convert MatchDocumentResult to KbDocumentEntity
            // Process similar to GetAll to ensure consistent format
            var documents = new List<KbDocumentEntity>();
            foreach (var result in matchResults)
            {
                var doc = new KbDocumentEntity
                {
                    Id = result.Id,
                    Content = result.Content ?? string.Empty
                };
                
                // Handle metadata - it might be an object or a string
                // 处理 metadata - 可能是对象或字符串
                if (result.Metadata != null)
                {
                    if (result.Metadata is JObject || result.Metadata is JToken)
                    {
                        // If it's a JSON object, serialize it to string
                        // 如果是 JSON 对象，序列化为字符串
                        doc.Metadata = JsonConvert.SerializeObject(result.Metadata);
                    }
                    else if (result.Metadata is string)
                    {
                        // If it's already a string, use it directly
                        // 如果已经是字符串，直接使用
                        doc.Metadata = result.Metadata as string;
                    }
                    else
                    {
                        // Convert to string
                        doc.Metadata = JsonConvert.SerializeObject(result.Metadata);
                    }
                }
                
                documents.Add(doc);
            }

            return documents;
        }
        #endregion

        #region Vector Search 向量检索
        /// <summary>
        /// Optimized vector similarity search with two-tier strategy
        /// 优化的向量相似度检索（两级策略）
        /// First tries high similarity threshold, then falls back to lower threshold if no results found
        /// 首先尝试高相似度阈值，如果没有结果则降级到较低阈值
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding (JSON array string)</param>
        /// <param name="highThreshold">High similarity threshold (default: 0.8)</param>
        /// <param name="highLimit">Maximum results for high similarity search (default: 3)</param>
        /// <param name="lowThreshold">Low similarity threshold for fallback (default: 0.6)</param>
        /// <param name="lowLimit">Maximum results for low similarity search (default: 2)</param>
        /// <returns>List of similar documents</returns>
        public List<KbDocumentEntity> VectorSimilaritySearchOptimized(string queryEmbedding, 
            decimal highThreshold = 0.8m, int highLimit = 3, 
            decimal lowThreshold = 0.6m, int lowLimit = 2)
        {
            IDbConnection conn = CreateSupabaseConnection();
            try
            {
                // First query: High similarity (priority execution)
                string highSql = @"
                    SELECT 
                        id,
                        content,
                        metadata,
                        embedding
                    FROM biz_documents
                    WHERE embedding IS NOT NULL
                        AND 1 - (embedding <=> @queryEmbedding::vector) >= @highThreshold
                    ORDER BY embedding <=> @queryEmbedding::vector
                    LIMIT @highLimit";

                var highParameters = new 
                { 
                    queryEmbedding = queryEmbedding, 
                    highThreshold = highThreshold, 
                    highLimit = highLimit 
                };

                var highResults = conn.Query<KbDocumentEntity>(highSql, highParameters).ToList();

                // If high similarity results found, return them immediately
                if (highResults != null && highResults.Count > 0)
                {
                    return highResults;
                }

                // Second query: Low similarity (backup solution)
                string lowSql = @"
                    SELECT 
                        id,
                        content,
                        metadata,
                        embedding
                    FROM biz_documents
                    WHERE embedding IS NOT NULL
                        AND 1 - (embedding <=> @queryEmbedding::vector) >= @lowThreshold
                    ORDER BY embedding <=> @queryEmbedding::vector
                    LIMIT @lowLimit";

                var lowParameters = new 
                { 
                    queryEmbedding = queryEmbedding, 
                    lowThreshold = lowThreshold, 
                    lowLimit = lowLimit 
                };

                var lowResults = conn.Query<KbDocumentEntity>(lowSql, lowParameters).ToList();
                return lowResults;
            }
            finally
            {
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Vector similarity search using cosine distance
        /// 使用余弦相似度进行向量检索
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding (JSON array string)</param>
        /// <param name="limit">Maximum number of results</param>
        /// <param name="threshold">Similarity threshold (0-1, higher means more similar)</param>
        /// <returns>List of similar documents</returns>
        public List<KbDocumentEntity> VectorSimilaritySearch(string queryEmbedding, int limit = 10, decimal? threshold = null)
        {
            IDbConnection conn = CreateSupabaseConnection();
            try
            {
                string sql = @"
                    SELECT 
                        id,
                        content,
                        metadata,
                        embedding
                    FROM biz_documents
                    WHERE embedding IS NOT NULL
                    ORDER BY embedding <=> @queryEmbedding::vector
                    LIMIT @limit";

                if (threshold.HasValue)
                {
                    sql = @"
                        SELECT 
                            id,
                            content,
                            metadata,
                            embedding
                        FROM biz_documents
                        WHERE embedding IS NOT NULL
                            AND 1 - (embedding <=> @queryEmbedding::vector) >= @threshold
                        ORDER BY embedding <=> @queryEmbedding::vector
                        LIMIT @limit";
                }

                var parameters = new { queryEmbedding = queryEmbedding, limit = limit, threshold = threshold };
                var results = conn.Query<KbDocumentEntity>(sql, parameters).ToList();
                return results;
            }
            finally
            {
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Vector similarity search using cosine distance with transaction
        /// 使用余弦相似度进行向量检索（带事务）
        /// </summary>
        public List<KbDocumentEntity> VectorSimilaritySearch(IDbConnection conn, string queryEmbedding, int limit = 10, decimal? threshold = null, IDbTransaction trans = null)
        {
            string sql = @"
                SELECT 
                    id,
                    content,
                    metadata,
                    embedding
                FROM biz_documents
                WHERE embedding IS NOT NULL
                ORDER BY embedding <=> @queryEmbedding::vector
                LIMIT @limit";

            if (threshold.HasValue)
            {
                sql = @"
                    SELECT 
                        id,
                        content,
                        metadata,
                        embedding
                    FROM biz_documents
                    WHERE embedding IS NOT NULL
                        AND 1 - (embedding <=> @queryEmbedding::vector) >= @threshold
                    ORDER BY embedding <=> @queryEmbedding::vector
                    LIMIT @limit";
            }

            var parameters = new { queryEmbedding = queryEmbedding, limit = limit, threshold = threshold };
            var results = conn.Query<KbDocumentEntity>(sql, parameters, trans).ToList();
            return results;
        }

        /// <summary>
        /// Hybrid search combining vector similarity and metadata filtering
        /// 混合检索：结合向量相似度和元数据过滤
        /// </summary>
        /// <param name="queryEmbedding">Query vector embedding</param>
        /// <param name="metadataFilter">Metadata filter as JSON string</param>
        /// <param name="limit">Maximum number of results</param>
        /// <param name="threshold">Similarity threshold</param>
        /// <returns>List of similar documents</returns>
        public List<KbDocumentEntity> HybridSearch(string queryEmbedding, string metadataFilter = null, int limit = 10, decimal? threshold = null)
        {
            IDbConnection conn = CreateSupabaseConnection();
            try
            {
                string sql = @"
                    SELECT 
                        id,
                        content,
                        metadata,
                        embedding
                    FROM biz_documents
                    WHERE embedding IS NOT NULL";

                var parameters = new DynamicParameters();
                parameters.Add("queryEmbedding", queryEmbedding);
                parameters.Add("limit", limit);

                if (threshold.HasValue)
                {
                    sql += " AND 1 - (embedding <=> @queryEmbedding::vector) >= @threshold";
                    parameters.Add("threshold", threshold.Value);
                }

                if (!string.IsNullOrWhiteSpace(metadataFilter))
                {
                    sql += " AND metadata @> @metadataFilter::jsonb";
                    parameters.Add("metadataFilter", metadataFilter);
                }

                sql += " ORDER BY embedding <=> @queryEmbedding::vector LIMIT @limit";

                var results = conn.Query<KbDocumentEntity>(sql, parameters).ToList();
                return results;
            }
            finally
            {
                conn?.Dispose();
            }
        }
        #endregion

        #region CRUD Operations 增删改查操作
        /// <summary>
        /// Insert document
        /// 插入文档
        /// </summary>
        public long Insert(KbDocumentEntity entity)
        {
            IDbConnection conn = CreateSupabaseConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                long docId = Insert(conn, entity, trans);
                trans.Commit();
                return docId;
            }
            catch (System.Exception ex)
            {
                trans?.Rollback();
                throw;
            }
            finally
            {
                trans?.Dispose();
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Insert document with transaction
        /// 插入文档（带事务）
        /// </summary>
        public long Insert(IDbConnection conn, KbDocumentEntity entity, IDbTransaction trans)
        {
            // Note: PostgreSQL will auto-generate the ID for bigserial
            // For vector embedding, it should be passed as a PostgreSQL vector literal or array
            var id = Repository.Insert<KbDocumentEntity>(conn, entity, trans).Id;
            return id;
        }

        /// <summary>
        /// Update document
        /// 更新文档
        /// </summary>
        public void Update(KbDocumentEntity entity)
        {
            IDbConnection conn = CreateSupabaseConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();
                Update(conn, entity, trans);
                trans.Commit();
            }
            catch (System.Exception)
            {
                trans?.Rollback();
                throw;
            }
            finally
            {
                trans?.Dispose();
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Update document with transaction
        /// 更新文档（带事务）
        /// </summary>
        public void Update(IDbConnection conn, KbDocumentEntity entity, IDbTransaction trans)
        {
            string sql = @"
                UPDATE biz_documents
                SET content = @Content,
                    metadata = @Metadata::jsonb,
                    embedding = @Embedding::vector
                WHERE id = @Id";

            var parameters = new
            {
                Id = entity.Id,
                Content = entity.Content,
                Metadata = entity.Metadata,
                Embedding = entity.Embedding
            };

            conn.Execute(sql, parameters, trans);
        }

        /// <summary>
        /// Delete document
        /// 删除文档
        /// </summary>
        public void Delete(long id)
        {
            IDbConnection conn = CreateSupabaseConnection();
            IDbTransaction trans = null;
            try
            {
                trans = conn.BeginTransaction();

                string sql = @"DELETE FROM biz_documents WHERE id = @Id";
                conn.Execute(sql, new { Id = id }, trans);

                trans.Commit();
            }
            catch (System.Exception ex)
            {
                trans?.Rollback();
                throw;
            }
            finally
            {
                trans?.Dispose();
                conn?.Dispose();
            }
        }

        /// <summary>
        /// Delete document with transaction
        /// 删除文档（带事务）
        /// </summary>
        public void Delete(IDbConnection conn, long id, IDbTransaction trans)
        {
            string sql = @"DELETE FROM biz_documents WHERE id = @Id";
            conn.Execute(sql, new { Id = id }, trans);
        }
        #endregion

        #region Test Connection 测试连接
        /// <summary>
        /// Test connection by reading data from biz_documents table using REST API
        /// 使用REST API通过读取biz_documents表数据来测试连接
        /// Reference: D:\Cloud365\SolveX\kb-docuemnts project
        /// </summary>
        public List<KbDocumentEntity> TestReadBizDocuments(int limit = 5)
        {
            if (_configOptions == null)
            {
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured. Please provide configuration options.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
            {
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
            {
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            }

            // Use REST API approach similar to reference project
            // 使用REST API方法，参考项目：D:\Cloud365\SolveX\kb-docuemnts
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Query biz_documents table using Supabase REST API
                // 使用Supabase REST API查询biz_documents表
                // Format: GET /rest/v1/biz_documents?select=id,content,metadata,embedding&limit=5&order=id
                var response = httpClient.GetAsync($"/rest/v1/biz_documents?select=id,content,metadata,embedding&limit={limit}&order=id").Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new InvalidOperationException($"Failed to read biz_documents table: HTTP {response.StatusCode} - {errorContent}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var documents = JsonConvert.DeserializeObject<List<KbDocumentEntity>>(content);

                return documents;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read biz_documents table: {ex.Message}", ex);
            }
        }
        #endregion

        #region Get Table Names 获取表名称列表
        /// <summary>
        /// Get all table names from Supabase database using REST API
        /// 使用REST API获取Supabase数据库中的所有表名称
        /// Reference: D:\Cloud365\SolveX\kb-docuemnts\DataViewer\Program.cs
        /// 参考项目：D:\Cloud365\SolveX\kb-docuemnts\DataViewer\Program.cs
        /// Note: Requires list_tables() RPC function in Supabase
        /// 注意：需要在Supabase中创建 list_tables() RPC 函数
        /// </summary>
        public List<string> GetTableNames()
        {
            if (_configOptions == null)
            {
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured. Please provide configuration options.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
            {
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
            {
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");
            }

            // Use REST API approach similar to reference project
            // 使用REST API方法，参考项目：D:\Cloud365\SolveX\kb-docuemnts
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            httpClient.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Call RPC function list_tables
                // RPC function returns: [{ table_name: "xxx", table_schema: "public" }, ...]
                // 调用RPC函数 list_tables
                // RPC函数返回：[{ table_name: "xxx", table_schema: "public" }, ...]
                var rpcRequest = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync("/rest/v1/rpc/list_tables", rpcRequest).Result;
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = response.Content.ReadAsStringAsync().Result;
                    throw new InvalidOperationException($"Failed to call RPC function list_tables: HTTP {response.StatusCode} - {errorContent}");
                }

                var content = response.Content.ReadAsStringAsync().Result;
                var tableNames = new List<string>();

                // Parse response as dictionary list (similar to reference project)
                // 解析响应为字典列表（与参考项目类似）
                // The RPC function returns: [{ "table_name": "xxx", "table_schema": "public" }, ...]
                // RPC函数返回：[{ "table_name": "xxx", "table_schema": "public" }, ...]
                using (JsonDocument doc = JsonDocument.Parse(content))
                {
                    foreach (JsonElement element in doc.RootElement.EnumerateArray())
                    {
                        // Try to find table_name field (case-insensitive)
                        // 尝试查找 table_name 字段（不区分大小写）
                        string tableName = "";
                        
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            foreach (JsonProperty property in element.EnumerateObject())
                            {
                                if (property.Name.Equals("table_name", StringComparison.OrdinalIgnoreCase))
                                {
                                    tableName = property.Value.GetString() ?? "";
                                    break;
                                }
                            }
                            
                            // If not found, try other possible keys
                            // 如果没找到，尝试其他可能的键名
                            if (string.IsNullOrEmpty(tableName))
                            {
                                if (element.TryGetProperty("TableName", out var tableNameProp))
                                    tableName = tableNameProp.GetString() ?? "";
                                else if (element.TryGetProperty("tablename", out var tablenameProp))
                                    tableName = tablenameProp.GetString() ?? "";
                                else if (element.TryGetProperty("name", out var nameProp))
                                    tableName = nameProp.GetString() ?? "";
                            }
                        }
                        
                        if (!string.IsNullOrEmpty(tableName))
                        {
                            tableNames.Add(tableName);
                        }
                    }
                }

                return tableNames;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get table names from Supabase: {ex.Message}", ex);
            }
        }
        #endregion

        #region Generate Metadata 生成元数据
        /// <summary>
        /// Generate metadata JSON from Question, Intent, and Answer
        /// 根据Question、Intent和Answer生成元数据JSON
        /// </summary>
        public JObject GenerateMetadata(string question, string intent, string answer)
        {
            var metadata = new JObject();
            
            if (!string.IsNullOrWhiteSpace(question))
            {
                metadata["question"] = question;
            }
            
            if (!string.IsNullOrWhiteSpace(intent))
            {
                metadata["intent"] = intent;
            }
            
            if (!string.IsNullOrWhiteSpace(answer))
            {
                metadata["answer"] = answer;
            }

            // Add timestamp
            metadata["created_at"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            
            return metadata;
        }
        #endregion
    }
}
