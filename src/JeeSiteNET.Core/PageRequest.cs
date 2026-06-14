namespace JeeSiteNET.Core;

/// <summary>
/// 分页查询请求基类：定义页码、页大小、排序字段与排序方式
/// </summary>
public class PageRequest
{
    /// <summary>
    /// 当前页码（从 1 开始），默认值 1
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 每页记录数，默认值 20
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 排序字段名（如 "CreateDate"）
    /// </summary>
    public string? SortField { get; set; }

    /// <summary>
    /// 排序方向（asc=升序，desc=降序）
    /// </summary>
    public string? SortOrder { get; set; }
}

/// <summary>
/// 带实体筛选条件的分页查询请求
/// </summary>
/// <typeparam name="T">查询条件实体的类型</typeparam>
public class PageRequest<T> : PageRequest
{
    /// <summary>
    /// 查询条件实体
    /// </summary>
    public T? Entity { get; set; }
}

/// <summary>
/// 分页查询结果
/// </summary>
/// <typeparam name="T">结果列表项的类型</typeparam>
public class PageResult<T>
{
    /// <summary>
    /// 当前页数据列表
    /// </summary>
    public List<T> List { get; set; } = [];

    /// <summary>
    /// 符合条件的总记录数
    /// </summary>
    public long Total { get; set; }

    /// <summary>
    /// 当前页码（从 1 开始）
    /// </summary>
    public int PageNo { get; set; } = 1;

    /// <summary>
    /// 每页记录数
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 创建一个空的分页结果（Total=0，List=空）
    /// </summary>
    /// <param name="pageNo">页码</param>
    /// <param name="pageSize">每页记录数</param>
    /// <returns>空分页结果</returns>
    public static PageResult<T> Empty(int pageNo = 1, int pageSize = 20)
        => new() { PageNo = pageNo, PageSize = pageSize, Total = 0, List = [] };
}
