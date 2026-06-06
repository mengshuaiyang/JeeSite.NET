namespace JeeSiteNET.Core;

public interface ICrudService<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    Task<TDto?> GetAsync(object id);
    Task<List<TDto>> FindListAsync(TEntity? entity = null);
    Task<PageResult<TDto>> FindPageAsync(PageRequest<TEntity> request);
    Task SaveAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}
