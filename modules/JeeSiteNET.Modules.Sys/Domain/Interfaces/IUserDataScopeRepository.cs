    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IUserDataScopeRepository
// 定义接口：IUserDataScopeRepository
public interface IUserDataScopeRepository
{
    IQueryable<UserDataScope> Query();
    Task<UserDataScope?> GetByUserAsync(string userCode);
    Task AddAsync(UserDataScope entity);
    Task UpdateAsync(UserDataScope entity);
    Task SaveChangesAsync();
}
