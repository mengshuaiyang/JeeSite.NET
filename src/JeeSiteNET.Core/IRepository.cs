namespace JeeSiteNET.Core;

/// <summary>
/// 通用仓储接口：提供实体的查询与持久化能力
/// </summary>
/// <typeparam name="T">实体类型</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// 返回可查询对象（用于 LINQ 组合条件）
    /// </summary>
    /// <returns>IQueryable 查询对象</returns>
    IQueryable<T> Query();

    /// <summary>
    /// 按主键获取单条记录
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>实体对象（未找到返回 null）</returns>
    Task<T?> GetAsync(object id);

    /// <summary>
    /// 获取当前实体的全部列表
    /// </summary>
    /// <returns>实体列表</returns>
    Task<List<T>> FindListAsync();

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">待新增的实体</param>
    /// <returns>Task</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">待更新的实体</param>
    /// <returns>Task</returns>
    Task UpdateAsync(T entity);

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="entity">待删除的实体</param>
    /// <returns>Task</returns>
    Task DeleteAsync(T entity);
}
