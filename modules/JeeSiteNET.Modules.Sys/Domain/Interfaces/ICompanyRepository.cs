    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 ICompanyRepository
// 定义接口：ICompanyRepository
public interface ICompanyRepository
{
    IQueryable<Company> Query();
    Task<Company?> GetAsync(string companyCode);
    Task<List<CompanyOffice>> GetOfficesAsync(string companyCode);
    Task AddAsync(Company entity);
    Task UpdateAsync(Company entity);
    Task DeleteAsync(string companyCode);
    Task AddOfficeAsync(CompanyOffice entity);
    Task DeleteOfficesAsync(string companyCode);
    Task SaveChangesAsync();
}
