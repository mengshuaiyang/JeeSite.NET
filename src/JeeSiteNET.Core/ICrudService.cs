namespace JeeSiteNET.Core;

/// <summary>
/// 通用 CRUD 应用服务接口：实体与 DTO 的双向转换与持久化
/// </summary>
/// <typeparam name="TEntity">领域实体类型</typeparam>
/// <typeparam name="TDto">数据传输对象类型</typeparam>
public interface ICrudService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    /// <summary>
    /// 按主键获取单条记录（DTO 形式）
    /// </summary>
    /// <param name="id">主键值</param>
    /// <returns>DTO 对象（未找到返回 null）</returns>
    Task<TDto?> GetAsync(object id);

    /// <summary>
    /// 按实体条件查询列表
    /// </summary>
    /// <param name="entity">查询条件实体（null 表示无条件）</param>
    /// <returns>DTO 列表</returns>
    Task<List<TDto>> FindListAsync(TEntity? entity = null);

    /// <summary>
    /// 按实体条件分页查询
    /// </summary>
    /// <param name="request">分页参数（含筛选实体、页码、页大小）</param>
    /// <returns>分页结果</returns>
    Task<PageResult<TDto>> FindPageAsync(PageRequest<TEntity> request);

    /// <summary>
    /// 保存实体（新增或更新，由实现类判定）
    /// </summary>
    /// <param name="entity">待保存的实体</param>
    /// <returns>Task</returns>
    Task SaveAsync(TEntity entity);

    /// <summary>
    /// 删除实体（逻辑或物理删除由实现类判定）
    /// </summary>
    /// <param name="entity">待删除的实体</param>
    /// <returns>Task</returns>
    Task DeleteAsync(TEntity entity);
}
