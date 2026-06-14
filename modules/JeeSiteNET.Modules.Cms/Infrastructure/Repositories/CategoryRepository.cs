    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Cms.Infrastructure.Repositories 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Infrastructure.Repositories
namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

// 定义class CategoryRepository
// 定义类：CategoryRepository
public class CategoryRepository : ICategoryRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 CategoryRepository
    // 构造函数：CategoryRepository
    public CategoryRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<Category> Query() => _db.Set<Category>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Category?> GetAsync(object id) => await _db.Set<Category>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Category>> FindListAsync() => await _db.Set<Category>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Category entity) { _db.Set<Category>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Category entity) { _db.Set<Category>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Category entity) { _db.Set<Category>().Remove(entity); await _db.SaveChangesAsync(); }
    // 方法：FindTreeAsync
    public async Task<List<Category>> FindTreeAsync() => await _db.Set<Category>().AsNoTracking().OrderBy(e => e.TreeSort).ToListAsync();
    // 方法：FindByCodesAsync
    public async Task<List<Category>> FindByCodesAsync(List<string> categoryCodes) => await _db.Set<Category>().AsNoTracking().Where(e => categoryCodes.Contains(e.CategoryCode)).ToListAsync();
}
