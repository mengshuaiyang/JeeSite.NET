namespace JeeSiteNET.Core.Search;

/// <summary>
/// 全文检索查询参数：封装关键词、实体类型、分页与返回字段
/// </summary>
public class SearchQuery
{
    /// <summary>
    /// 查询关键词（用于全文匹配）
    /// </summary>
    public string Query { get; set; } = string.Empty;

    /// <summary>
    /// 按实体类型过滤（对应索引或文档的 type 字段），null 表示不过滤
    /// </summary>
    public string? EntityType { get; set; }

    /// <summary>
    /// 当前页码（从 1 开始），默认值 1
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 每页条数，默认值 20
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 指定需要返回的字段名称（null 表示返回全部字段）
    /// </summary>
    public string[]? Fields { get; set; }
}

/// <summary>
/// 全文检索结果：包含匹配文档列表、总数与分页信息
/// </summary>
/// <typeparam name="T">文档对象类型</typeparam>
public class SearchResult<T>
{
    /// <summary>
    /// 当前页的文档列表
    /// </summary>
    public List<T> Items { get; set; } = [];

    /// <summary>
    /// 命中的总文档数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    public int PageNo { get; set; }

    /// <summary>
    /// 每页条数
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 实体类型（回显用于确认查询范围）
    /// </summary>
    public string? EntityType { get; set; }
}

/// <summary>
/// 全文检索服务接口：屏蔽具体搜索引擎（Elasticsearch、Meilisearch 等），
/// 暴露索引的增删查与健康检查能力
/// </summary>
public interface ISearchService
{
    /// <summary>
    /// 向指定索引写入/更新单条文档（若 id 已存在则为更新）
    /// </summary>
    /// <typeparam name="T">文档类型</typeparam>
    /// <param name="indexName">索引名称</param>
    /// <param name="id">文档唯一标识</param>
    /// <param name="document">文档对象</param>
    /// <returns>Task</returns>
    Task IndexAsync<T>(string indexName, string id, T document) where T : class;

    /// <summary>
    /// 从指定索引中删除单条文档
    /// </summary>
    /// <param name="indexName">索引名称</param>
    /// <param name="id">文档唯一标识</param>
    /// <returns>Task</returns>
    Task DeleteAsync(string indexName, string id);

    /// <summary>
    /// 按查询条件检索文档
    /// </summary>
    /// <typeparam name="T">文档类型</typeparam>
    /// <param name="request">查询参数（含关键词、分页、类型过滤等）</param>
    /// <returns>分页检索结果</returns>
    Task<SearchResult<T>> SearchAsync<T>(SearchQuery request) where T : class;

    /// <summary>
    /// 探测搜索引擎是否可用（用于健康检查/初始化完成判断）
    /// </summary>
    /// <returns>true 表示可联通；false 表示服务不可用</returns>
    Task<bool> PingAsync();
}
