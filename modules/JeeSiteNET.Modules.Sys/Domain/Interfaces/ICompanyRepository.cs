using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

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
