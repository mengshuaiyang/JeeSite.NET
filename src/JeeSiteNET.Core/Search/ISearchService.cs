namespace JeeSiteNET.Core.Search;

public class SearchQuery
{
    public string Query { get; set; } = string.Empty;
    public string? EntityType { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string[]? Fields { get; set; }
}

public class SearchResult<T>
{
    public List<T> Items { get; set; } = [];
    public long Total { get; set; }
    public int PageNo { get; set; }
    public int PageSize { get; set; }
    public string? EntityType { get; set; }
}

public interface ISearchService
{
    Task IndexAsync<T>(string indexName, string id, T document) where T : class;
    Task DeleteAsync(string indexName, string id);
    Task<SearchResult<T>> SearchAsync<T>(SearchQuery request) where T : class;
    Task<bool> PingAsync();
}
