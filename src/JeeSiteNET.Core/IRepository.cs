namespace JeeSiteNET.Core;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T?> GetAsync(object id);
    Task<List<T>> FindListAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
