namespace JeeSiteNET.Core;

public class PageRequest
{
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortField { get; set; }
    public string? SortOrder { get; set; }
}

public class PageRequest<T> : PageRequest
{
    public T? Entity { get; set; }
}

public class PageResult<T>
{
    public List<T> List { get; set; } = [];
    public long Total { get; set; }
    public int PageNo { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public static PageResult<T> Empty(int pageNo = 1, int pageSize = 20)
        => new() { PageNo = pageNo, PageSize = pageSize, Total = 0, List = [] };
}
