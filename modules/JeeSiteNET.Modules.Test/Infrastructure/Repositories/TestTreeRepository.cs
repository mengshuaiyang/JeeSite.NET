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

// 定义class TestTreeRepository
// 定义类：TestTreeRepository
public class TestTreeRepository : ITestTreeRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 TestTreeRepository
    // 构造函数：TestTreeRepository
    public TestTreeRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<TestTree> Query() => _db.Set<TestTree>().AsNoTracking();
    // 方法：GetAsync
    public async Task<TestTree?> GetAsync(object id) => await _db.Set<TestTree>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<TestTree>> FindListAsync() => await _db.Set<TestTree>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(TestTree entity) { _db.Set<TestTree>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(TestTree entity) { _db.Set<TestTree>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(TestTree entity) { _db.Set<TestTree>().Remove(entity); await _db.SaveChangesAsync(); }
}
