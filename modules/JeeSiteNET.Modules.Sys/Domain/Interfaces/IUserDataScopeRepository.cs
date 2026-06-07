using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IUserDataScopeRepository
{
    IQueryable<UserDataScope> Query();
    Task<UserDataScope?> GetByUserAsync(string userCode);
    Task AddAsync(UserDataScope entity);
    Task UpdateAsync(UserDataScope entity);
    Task SaveChangesAsync();
}
