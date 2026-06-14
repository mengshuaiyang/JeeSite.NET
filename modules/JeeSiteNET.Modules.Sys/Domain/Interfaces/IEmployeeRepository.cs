    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
namespace JeeSiteNET.Modules.Sys.Domain.Interfaces;

// 定义接口 IEmployeeRepository
// 定义接口：IEmployeeRepository
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
