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

// 定义class CompanyRepository
// 定义类：CompanyRepository
public class CompanyRepository : ICompanyRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 CompanyRepository
    // 构造函数：CompanyRepository
    public CompanyRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法：Query
    public IQueryable<Company> Query() => _db.Set<Company>().AsNoTracking();

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<Company?> GetAsync(string companyCode)
        => await _db.Set<Company>().FindAsync(companyCode);

    // 方法 GetOfficesAsync
    // 方法：GetOfficesAsync
    public async Task<List<CompanyOffice>> GetOfficesAsync(string companyCode)
        // 数据库操作：条件过滤
        => await _db.Set<CompanyOffice>().Where(e => e.CompanyCode == companyCode).AsNoTracking().ToListAsync();

    // 方法：AddAsync
    public async Task AddAsync(Company entity) => await _db.Set<Company>().AddAsync(entity);

    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public Task UpdateAsync(Company entity)
    {
        // 调用 Update
        _db.Set<Company>().Update(entity);
        // return 返回结果
        return Task.CompletedTask;
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(string companyCode)
    {
        var entity = await _db.Set<Company>().FindAsync(companyCode);
        // if 条件判断
        if (entity != null) _db.Set<Company>().Remove(entity);
        // await 异步等待
        await DeleteOfficesAsync(companyCode);
    }

    // 方法：AddOfficeAsync
    public async Task AddOfficeAsync(CompanyOffice entity) => await _db.Set<CompanyOffice>().AddAsync(entity);

    // 方法 DeleteOfficesAsync
    // 方法：DeleteOfficesAsync
    public async Task DeleteOfficesAsync(string companyCode)
    {
        // 数据库操作：条件过滤
        var list = await _db.Set<CompanyOffice>().Where(e => e.CompanyCode == companyCode).ToListAsync();
        // 集合操作：批量移除
        _db.Set<CompanyOffice>().RemoveRange(list);
    }

    // 方法：SaveChangesAsync
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
