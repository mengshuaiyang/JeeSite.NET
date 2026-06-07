using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

public interface IEmployeeRepository
{
    IQueryable<Employee> Query();
    Task<Employee?> GetAsync(string empCode);
    Task<List<EmployeePost>> GetPostsAsync(string empCode);
    Task<List<EmployeeOffice>> GetOfficesAsync(string empCode);
    Task AddAsync(Employee entity);
    Task UpdateAsync(Employee entity);
    Task DeleteAsync(string empCode);
    Task AddPostAsync(EmployeePost entity);
    Task DeletePostsAsync(string empCode);
    Task AddOfficeAsync(EmployeeOffice entity);
    Task DeleteOfficesAsync(string empCode);
    Task SaveChangesAsync();
}
