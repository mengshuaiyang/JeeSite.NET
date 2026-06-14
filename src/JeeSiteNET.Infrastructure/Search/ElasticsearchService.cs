using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using JeeSiteNET.Core.Search;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Infrastructure.Search;

/// <summary>
/// Elasticsearch 搜索服务实现：提供文档写入/删除/多字段模糊搜索与健康检查
/// </summary>
public class ElasticsearchService : ISearchService
{
    /// <summary>
    /// ES 客户端
    /// </summary>
    private readonly ElasticsearchClient _client;
    /// <summary>
    /// 日志记录器
    /// </summary>
    private readonly ILogger<ElasticsearchService> _logger;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="client">ElasticsearchClient 实例</param>
    /// <param name="logger">日志记录器</param>
    public ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
    {
        _client = client;
        _logger = logger;
    }

    /// <summary>
    /// 将单个文档写入指定索引（索引不存在时会自动创建）
    /// </summary>
    /// <typeparam name="T">文档类型</typeparam>
    /// <param name="indexName">索引名称（建议使用小写）</param>
    /// <param name="id">文档 ID</param>
    /// <param name="document">文档对象</param>
    /// <returns>Task</returns>
    public async Task IndexAsync<T>(string indexName, string id, T document) where T : class
    {
        var response = await _client.IndexAsync(document, indexName, id);
        if (!response.IsValidResponse)
            _logger.LogWarning("ES index failed for {Index}/{Id}: {Error}", indexName, id, response.ElasticsearchServerError?.Error);
    }

    /// <summary>
    /// 从指定索引删除文档
    /// </summary>
    /// <param name="indexName">索引名称</param>
    /// <param name="id">文档 ID</param>
    /// <returns>Task</returns>
    public async Task DeleteAsync(string indexName, string id)
    {
        var response = await _client.DeleteAsync(indexName, id);
        if (!response.IsValidResponse)
            _logger.LogWarning("ES delete failed for {Index}/{Id}: {Error}", indexName, id, response.ElasticsearchServerError?.Error);
    }

    /// <summary>
    /// 多字段模糊搜索：按 SearchQuery 中的查询条件分页返回结果；未显式指定 EntityType 时以类名作为索引名
    /// </summary>
    /// <typeparam name="T">文档类型</typeparam>
    /// <param name="query">搜索查询参数（包含关键词、页码、页大小等）</param>
    /// <returns>分页搜索结果；若请求失败则返回空结果对象</returns>
    public async Task<SearchResult<T>> SearchAsync<T>(SearchQuery query) where T : class
    {
        var indexName = query.EntityType?.ToLowerInvariant() ?? typeof(T).Name.ToLowerInvariant();
        var from = (query.PageNo - 1) * query.PageSize;

        var searchResponse = await _client.SearchAsync<T>(s => s
            .Indices(indexName)
            .From(from)
            .Size(query.PageSize)
            .Query(q => q
                .MultiMatch(mm => mm
                    .Query(query.Query)
                    .Fuzziness(new Fuzziness("AUTO"))
                )
            )
        );

        if (!searchResponse.IsValidResponse)
        {
            _logger.LogWarning("ES search failed: {Error}", searchResponse.ElasticsearchServerError?.Error);
            return new SearchResult<T> { PageNo = query.PageNo, PageSize = query.PageSize, EntityType = query.EntityType };
        }

        return new SearchResult<T>
        {
            Items = searchResponse.Documents.ToList(),
            Total = searchResponse.Total,
            PageNo = query.PageNo,
            PageSize = query.PageSize,
            EntityType = query.EntityType,
        };
    }

    /// <summary>
    /// Ping 检查 ES 集群是否可用
    /// </summary>
    /// <returns>可用时返回 true</returns>
    public async Task<bool> PingAsync()
    {
        var response = await _client.PingAsync();
        return response.IsValidResponse;
    }
}
