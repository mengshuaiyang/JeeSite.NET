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

// 定义class GuestbookRepository
// 定义类：GuestbookRepository
public class GuestbookRepository : IGuestbookRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 GuestbookRepository
    // 构造函数：GuestbookRepository
    public GuestbookRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Guestbook> Query() => _db.Set<Guestbook>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Guestbook?> GetAsync(object id) => await _db.Set<Guestbook>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Guestbook>> FindListAsync() => await _db.Set<Guestbook>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Guestbook entity) { _db.Set<Guestbook>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Guestbook entity) { _db.Set<Guestbook>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Guestbook entity) { _db.Set<Guestbook>().Remove(entity); await _db.SaveChangesAsync(); }

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<Guestbook>> FindPageAsync(PageRequest<Guestbook> request)
    {
        // 数据库操作：不跟踪查询（只读）
        var query = _db.Set<Guestbook>().AsNoTracking()
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.GbType), e => e.GbType == request.Entity!.GbType)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            // 数据库操作：降序排序
            .OrderByDescending(e => e.CreateDate);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<Guestbook> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
