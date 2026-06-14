    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

// 定义class EmployeeRepository
// 定义类：EmployeeRepository
public class EmployeeRepository : IEmployeeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 EmployeeRepository
    // 构造函数：EmployeeRepository
    public EmployeeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法：Query
    public IQueryable<Employee> Query() => _db.Set<Employee>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<Employee?> GetAsync(string empCode)
        => await _db.Set<Employee>().FindAsync(empCode);

    // 方法 GetPostsAsync
    // 方法：GetPostsAsync
    public async Task<List<EmployeePost>> GetPostsAsync(string empCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmployeePost>().Where(e => e.EmpCode == empCode).AsNoTracking().ToListAsync();

    // 方法 GetOfficesAsync
    // 方法：GetOfficesAsync
    public async Task<List<EmployeeOffice>> GetOfficesAsync(string empCode)
        // 数据库操作：条件过滤
        => await _db.Set<EmployeeOffice>().Where(e => e.EmpCode == empCode).AsNoTracking().ToListAsync();

    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Employee entity)
        // 数据库操作：异步添加
        => await _db.Set<Employee>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(Employee entity)
    {
        // 调用 Update
        _db.Set<Employee>().Update(entity);
        // return 返回结果
        return Task.CompletedTask;
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string empCode)
    {
        var entity = await _db.Set<Employee>().FindAsync(empCode);
        // if 条件判断
        if (entity != null) _db.Set<Employee>().Remove(entity);
        // await 异步等待
        await DeletePostsAsync(empCode);
        // await 异步等待
        await DeleteOfficesAsync(empCode);
    }

    // 方法 AddPostAsync
    // 方法：AddPostAsync
    public async Task AddPostAsync(EmployeePost entity)
        // 数据库操作：异步添加
        => await _db.Set<EmployeePost>().AddAsync(entity);

    // 方法 DeletePostsAsync
    // 方法：DeletePostsAsync
    public async Task DeletePostsAsync(string empCode)
    {
        // 数据库操作：条件过滤
        var list = await _db.Set<EmployeePost>().Where(e => e.EmpCode == empCode).ToListAsync();
        // 集合操作：批量移除
        _db.Set<EmployeePost>().RemoveRange(list);
    }

    // 方法 AddOfficeAsync
    // 方法：AddOfficeAsync
    public async Task AddOfficeAsync(EmployeeOffice entity)
        // 数据库操作：异步添加
        => await _db.Set<EmployeeOffice>().AddAsync(entity);

    // 方法 DeleteOfficesAsync
    // 方法：DeleteOfficesAsync
    public async Task DeleteOfficesAsync(string empCode)
    {
        // 数据库操作：条件过滤
        var list = await _db.Set<EmployeeOffice>().Where(e => e.EmpCode == empCode).ToListAsync();
        // 集合操作：批量移除
        _db.Set<EmployeeOffice>().RemoveRange(list);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
