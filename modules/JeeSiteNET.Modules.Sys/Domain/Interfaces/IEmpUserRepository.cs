    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IEmpUserRepository
// 定义接口：IEmpUserRepository
public interface IEmpUserRepository
{
    IQueryable<EmpUser> Query();
    Task<EmpUser?> GetAsync(string empCode, string userCode);
    Task<List<EmpUser>> GetByEmpCodeAsync(string empCode);
    Task<List<EmpUser>> GetByUserCodeAsync(string userCode);
    Task<List<string>> GetUserCodesByEmpCodeAsync(string empCode);
    Task<List<string>> GetEmpCodesByUserCodeAsync(string userCode);
    Task AddAsync(EmpUser entity);
    Task DeleteAsync(string empCode, string userCode);
    Task DeleteByEmpCodeAsync(string empCode);
}
