using System.Linq.Expressions;

namespace JeeSiteNET.Core;

/// <summary>
/// IQueryable 扩展方法集合：条件筛选、列表查询、分页查询的统一封装
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// 条件性筛选：当 condition 为 true 时才应用指定的过滤条件
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <param name="condition">是否应用筛选的判断条件</param>
    /// <param name="predicate">筛选表达式</param>
    /// <returns>应用筛选后的查询对象（或原始对象）</returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        // condition=true 时应用过滤，否则返回原 query，避免生成多余的 SQL 条件
        return condition ? query.Where(predicate) : query;
    }

    /// <summary>
    /// 异步将查询对象转为列表（同步转异步的轻量封装）
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <returns>实体列表</returns>
    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
    {
        // 将同步枚举卸载到线程池，真正让出调用线程（区别于 Task.FromResult 的伪异步）
        return await Task.Run(() => query.ToList());
    }

    /// <summary>
    /// 异步统计查询结果数量（同步转异步的轻量封装）
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <returns>记录总数</returns>
    public static async Task<int> CountAsync<T>(this IQueryable<T> query)
    {
        // 将同步计数卸载到线程池，真正让出调用线程（区别于 Task.FromResult 的伪异步）
        return await Task.Run(() => query.Count());
    }

    /// <summary>
    /// 异步将查询对象转为分页结果（含总条数、当前页列表等）
    /// </summary>
    /// <typeparam name="T">查询实体类型</typeparam>
    /// <param name="query">原始查询对象</param>
    /// <param name="request">分页参数（含页码和页大小）</param>
    /// <returns>分页结果对象</returns>
    public static async Task<PageResult<T>> ToPageResultAsync<T>(
        this IQueryable<T> query,
        PageRequest request)
    {
        // 先统计总数，再 Skip/Take 取当前页，避免重复查询
        var total = await query.CountAsync();
        var list = await Task.Run(() =>
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
