using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using JeeSiteNET.Core.Search;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Infrastructure.Search;

public class ElasticsearchService : ISearchService
{
    private readonly ElasticsearchClient _client;
    private readonly ILogger<ElasticsearchService> _logger;

    public ElasticsearchService(ElasticsearchClient client, ILogger<ElasticsearchService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task IndexAsync<T>(string indexName, string id, T document) where T : class
    {
        var response = await _client.IndexAsync(document, indexName, id);
        if (!response.IsValidResponse)
            _logger.LogWarning("ES index failed for {Index}/{Id}: {Error}", indexName, id, response.ElasticsearchServerError?.Error);
    }

    public async Task DeleteAsync(string indexName, string id)
    {
        var response = await _client.DeleteAsync(indexName, id);
        if (!response.IsValidResponse)
            _logger.LogWarning("ES delete failed for {Index}/{Id}: {Error}", indexName, id, response.ElasticsearchServerError?.Error);
    }

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

    public async Task<bool> PingAsync()
    {
        var response = await _client.PingAsync();
        return response.IsValidResponse;
    }
}
