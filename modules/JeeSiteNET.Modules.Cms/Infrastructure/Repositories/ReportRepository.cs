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

// 定义class ReportRepository
// 定义类：ReportRepository
public class ReportRepository : IReportRepository
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;
    // 构造函数 ReportRepository
    // 构造函数：ReportRepository
    public ReportRepository(JeeSiteDbContext db) => _db = db;

    // 方法：Query
    public IQueryable<Report> Query() => _db.Set<Report>().AsNoTracking();
    // 方法：GetAsync
    public async Task<Report?> GetAsync(object id) => await _db.Set<Report>().FindAsync(id);
    // 方法：FindListAsync
    public async Task<List<Report>> FindListAsync() => await _db.Set<Report>().AsNoTracking().ToListAsync();
    // 方法 AddAsync
    // 方法：AddAsync
    public async Task AddAsync(Report entity) { _db.Set<Report>().Add(entity); await _db.SaveChangesAsync(); }
    // 方法 UpdateAsync
    // 方法：UpdateAsync
    public async Task UpdateAsync(Report entity) { _db.Set<Report>().Update(entity); await _db.SaveChangesAsync(); }
    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task DeleteAsync(Report entity) { _db.Set<Report>().Remove(entity); await _db.SaveChangesAsync(); }

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<Report>> FindPageAsync(PageRequest<Report> request)
    {
        // 数据库操作：不跟踪查询（只读）
        var query = _db.Set<Report>().AsNoTracking()
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ReportType), e => e.ReportType == request.Entity!.ReportType)
            // 调用 IsNullOrEmpty
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            // 数据库操作：降序排序
            .OrderByDescending(e => e.CreateDate);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<Report> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
