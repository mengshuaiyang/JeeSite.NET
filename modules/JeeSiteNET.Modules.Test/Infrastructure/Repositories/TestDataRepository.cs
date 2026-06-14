    // 引入 JeeSiteNET.Modules.Test.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Entities
using JeeSiteNET.Modules.Test.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Test.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Test.Domain.Interfaces
using JeeSiteNET.Modules.Test.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Test.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Test.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Test.Infrastructure.Repositories;

// 定义class TestDataRepository
// 定义类：TestDataRepository
public class TestDataRepository : ITestDataRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 TestDataRepository
    // 构造函数：TestDataRepository
    public TestDataRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<TestData> Query() => _db.Set<TestData>().AsNoTracking();
    // 方法：GetAsync
    public async Task<TestData?> GetAsync(object id) => await _db.Set<TestData>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<TestData>> FindListAsync() => await _db.Set<TestData>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(TestData entity) { _db.Set<TestData>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(TestData entity) { _db.Set<TestData>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(TestData entity) { _db.Set<TestData>().Remove(entity); await _db.SaveChangesAsync(); }
}
