using Slickflow.Data;
using Slickflow.AI.Entity;
using Slickflow.AI.Configuration;
using Slickflow.AI.Implementation;
using Slickflow.AI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Slickflow.AI.Manager
{
    /// <summary>
    /// Knowledge Base Document Manager
    /// 知识库文档管理类（所有操作通过 Supabase REST API 完成）
    /// </summary>
    public class KbDocumentManager : ManagerBase
    {
        private readonly AiAppConfigProviderOptions _configOptions;

        public KbDocumentManager(AiAppConfigProviderOptions configOptions = null)
        {
            _configOptions = configOptions;
        }

        private HttpClient CreateHttpClient()
        {
            if (_configOptions == null)
                throw new InvalidOperationException("AiAppConfigProviderOptions is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseProjectUrl))
                throw new InvalidOperationException("SupabaseProjectUrl is not configured.");
            if (string.IsNullOrWhiteSpace(_configOptions.SupabaseServiceRoleKey))
                throw new InvalidOperationException("SupabaseServiceRoleKey is not configured.");

            var client = new HttpClient();
            client.BaseAddress = new Uri(_configOptions.SupabaseProjectUrl.TrimEnd('/'));
            client.DefaultRequestHeaders.Add("apikey", _configOptions.SupabaseServiceRoleKey);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_configOptions.SupabaseServiceRoleKey}");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        #region Get Document Data

        public List<KbDocumentEntity> GetAll()
        {
            using var httpClient = CreateHttpClient();
            var response = httpClient.GetAsync("/rest/v1/biz_documents?select=id,content,metadata,embedding&order=id").Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                throw new InvalidOperationException($"Failed to read biz_documents table: HTTP {response.StatusCode} - {errorContent}");
            }

            var content = response.Content.ReadAsStringAsync().Result;
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

                    if (rawDoc.metadata != null)
                    {
                        doc.Metadata = (rawDoc.metadata is JObject || rawDoc.metadata is JToken)
                            ? JsonConvert.SerializeObject(rawDoc.metadata)
                            : rawDoc.metadata.ToString();
                    }

                    documents.Add(doc);
                }
            }

            return documents;
        }

        public List<KbDocumentEntity> TestReadBizDocuments(int limit = 5)
        {
            using var httpClient = CreateHttpClient();
            var response = httpClient.GetAsync($"/rest/v1/biz_documents?select=id,content,metadata,embedding&limit={limit}&order=id").Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                throw new InvalidOperationException($"Failed to read biz_documents table: HTTP {response.StatusCode} - {errorContent}");
            }

            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<List<KbDocumentEntity>>(content);
        }

        #endregion

        #region Save Document

        public KbDocumentEntity SaveDocument(KbDocumentEntity entity)
        {
            using var httpClient = CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            object metaVal = new { };
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
                response = httpClient.PatchAsync($"/rest/v1/biz_documents?id=eq.{entity.Id}", content).Result;
            else
                response = httpClient.PostAsync("/rest/v1/biz_documents", content).Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                throw new InvalidOperationException($"Failed to save document: HTTP {response.StatusCode} - {errorContent}");
            }

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var savedRows = JsonConvert.DeserializeObject<List<JObject>>(responseContent);
            if (savedRows != null && savedRows.Count > 0)
                entity.Id = savedRows[0]["id"]?.Value<long>() ?? 0;

            return entity;
        }

        public async Task UpdateEmbeddingAsync(long documentId, float[] embedding)
        {
            if (embedding == null || embedding.Length == 0)
                throw new ArgumentException("Embedding cannot be null or empty", nameof(embedding));

            using var httpClient = CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=minimal");

            var json = JsonConvert.SerializeObject(new { embedding });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PatchAsync($"/rest/v1/biz_documents?id=eq.{documentId}", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to update embedding: HTTP {response.StatusCode} - {errorContent}");
            }
        }

        public async Task ClearAllEmbeddingsAsync()
        {
            var documents = GetAll();
            if (documents == null || documents.Count == 0)
                return;

            using var httpClient = CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=minimal");

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

        #endregion

        #region Delete Documents

        public async Task<int> DeleteDocumentsWhereIdGreaterThanAsync(long minId)
        {
            using var httpClient = CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            var response = await httpClient.DeleteAsync($"/rest/v1/biz_documents?id=gt.{minId}");
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to delete documents: HTTP {response.StatusCode} - {err}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(content) ? 0 : JsonConvert.DeserializeObject<List<JObject>>(content)?.Count ?? 0;
        }

        public async Task<int> DeleteDocumentsByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            var ids = industryIds.ToList();
            if (ids.Count == 0) return 0;

            using var httpClient = CreateHttpClient();
            httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");

            var filter = string.Join(",", ids);
            var response = await httpClient.DeleteAsync($"/rest/v1/biz_documents?industry_id=in.({filter})");
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Failed to delete documents: HTTP {response.StatusCode} - {err}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(content) ? 0 : JsonConvert.DeserializeObject<List<JObject>>(content)?.Count ?? 0;
        }

        #endregion

        #region Embedding Status

        public async Task<List<(long Id, string Content)>> GetDocumentsWithNullEmbeddingByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            var ids = industryIds.ToList();
            if (ids.Count == 0) return new List<(long, string)>();

            using var httpClient = CreateHttpClient();
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

        public async Task<Dictionary<long, int>> GetDocumentCountByIndustryIdsAsync(IEnumerable<long> industryIds)
        {
            var ids = industryIds.ToList();
            if (ids.Count == 0) return new Dictionary<long, int>();

            using var httpClient = CreateHttpClient();
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

        #endregion

        #region Search Documents

        public List<KbDocumentEntity> SearchDocuments(string query, float threshold = 0.7f, int count = 5)
        {
            var ragService = new SupabaseRagService(_configOptions);
            float[] queryEmbedding;

            var provider = (_configOptions?.RagEmbeddingProvider ?? "OpenAI").Trim();
            if (provider.Equals("QWen3", StringComparison.OrdinalIgnoreCase))
            {
                if (_configOptions?.QianWen == null || string.IsNullOrWhiteSpace(_configOptions.QianWen.ApiKey))
                    throw new InvalidOperationException("RagEmbeddingProvider=QWen3 but QianWen.ApiKey is not configured.");

                var embeddingUrl = _configOptions.QianWen.BaseUrl.TrimEnd('/') + "/v1/embeddings";
                var embeddingModel = string.IsNullOrWhiteSpace(_configOptions.QianWen.EmbeddingModel)
                    ? "text-embedding-v3"
                    : _configOptions.QianWen.EmbeddingModel;
                var dimensions = _configOptions.RagEmbeddingDimensions > 0 ? _configOptions.RagEmbeddingDimensions : (int?)null;

                var generator = new QWen3EmbeddingGenerator(
                    _configOptions.QianWen.ApiKey, embeddingUrl, embeddingModel, dimensions);
                queryEmbedding = generator.GenerateEmbeddingAsync(query).Result;
            }
            else
            {
                if (_configOptions?.OpenAI == null || string.IsNullOrWhiteSpace(_configOptions.OpenAI.ApiKey))
                    throw new InvalidOperationException("RagEmbeddingProvider=OpenAI but OpenAI.ApiKey is not configured.");

                queryEmbedding = EmbeddingGenUtility.GenerateEmbeddingContent(_configOptions.OpenAI.ApiKey, query).Result;
            }

            var matchResults = ragService.MatchDocumentsOptimizedAsync(
                queryEmbedding,
                matchThresholdHigh: threshold,
                matchThresholdLow: threshold * 0.8f,
                matchCountHigh: count,
                matchCountLow: count).Result;

            var documents = new List<KbDocumentEntity>();
            foreach (var result in matchResults)
            {
                var doc = new KbDocumentEntity
                {
                    Id = result.Id,
                    Content = result.Content ?? string.Empty
                };

                if (result.Metadata != null)
                {
                    doc.Metadata = (result.Metadata is JObject || result.Metadata is JToken)
                        ? JsonConvert.SerializeObject(result.Metadata)
                        : result.Metadata is string s ? s : JsonConvert.SerializeObject(result.Metadata);
                }

                documents.Add(doc);
            }

            return documents;
        }

        #endregion

        #region Metadata & Table Info

        public JObject GenerateMetadata(string question, string intent, string answer)
        {
            var metadata = new JObject();
            if (!string.IsNullOrWhiteSpace(question)) metadata["question"] = question;
            if (!string.IsNullOrWhiteSpace(intent)) metadata["intent"] = intent;
            if (!string.IsNullOrWhiteSpace(answer)) metadata["answer"] = answer;
            metadata["created_at"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            return metadata;
        }

        public List<string> GetTableNames()
        {
            using var httpClient = CreateHttpClient();
            var rpcRequest = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("/rest/v1/rpc/list_tables", rpcRequest).Result;

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = response.Content.ReadAsStringAsync().Result;
                throw new InvalidOperationException($"Failed to call RPC function list_tables: HTTP {response.StatusCode} - {errorContent}");
            }

            var content = response.Content.ReadAsStringAsync().Result;
            var tableNames = new List<string>();

            using var doc = JsonDocument.Parse(content);
            foreach (var element in doc.RootElement.EnumerateArray())
            {
                if (element.ValueKind != JsonValueKind.Object) continue;
                string tableName = string.Empty;
                foreach (var property in element.EnumerateObject())
                {
                    if (property.Name.Equals("table_name", StringComparison.OrdinalIgnoreCase))
                    {
                        tableName = property.Value.GetString() ?? string.Empty;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(tableName))
                    tableNames.Add(tableName);
            }

            return tableNames;
        }

        #endregion
    }
}
