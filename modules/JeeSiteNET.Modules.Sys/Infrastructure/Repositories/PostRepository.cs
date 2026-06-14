    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
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

// 定义class PostRepository
// 定义类：PostRepository
public class PostRepository : IPostRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 PostRepository
    // 构造函数：PostRepository
    public PostRepository(JeeSiteDbContext db) => _db = db;
    // 方法：Query
    public IQueryable<Post> Query() => _db.Set<Post>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Post?> GetAsync(object id) => await _db.Set<Post>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Post>> FindListAsync() => await _db.Set<Post>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Post entity) { await _db.Set<Post>().AddAsync(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Post entity) { _db.Set<Post>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Post entity) { _db.Set<Post>().Remove(entity); await _db.SaveChangesAsync(); }
}
