using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
