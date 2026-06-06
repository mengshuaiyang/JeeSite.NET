using System.Linq.Expressions;

namespace JeeSiteNET.Core;

public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }

    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
    {
        return await Task.FromResult(query.ToList());
    }

    public static async Task<int> CountAsync<T>(this IQueryable<T> query)
    {
        return await Task.FromResult(query.Count());
    }

    public static async Task<PageResult<T>> ToPageResultAsync<T>(
        this IQueryable<T> query,
        PageRequest request)
    {
        var total = await query.CountAsync();
        var list = await Task.FromResult(
            query.Skip((request.PageNo - 1) * request.PageSize)
                 .Take(request.PageSize)
                 .ToList());
        return new PageResult<T>
        {
            List = list,
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }
}
