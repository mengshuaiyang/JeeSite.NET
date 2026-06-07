using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class VisitLogRepository : IVisitLogRepository
{
    private readonly JeeSiteDbContext _db;
    public VisitLogRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<VisitLog> Query() => _db.Set<VisitLog>().AsNoTracking();
    public async Task<VisitLog?> GetAsync(object id) => await _db.Set<VisitLog>().FindAsync(id);
    public async Task<List<VisitLog>> FindListAsync() => await _db.Set<VisitLog>().AsNoTracking().ToListAsync();
    public async Task AddAsync(VisitLog entity) { _db.Set<VisitLog>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(VisitLog entity) { _db.Set<VisitLog>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(VisitLog entity) { _db.Set<VisitLog>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<PageResult<VisitLog>> FindPageAsync(PageRequest<VisitLog> request)
    {
        var query = _db.Set<VisitLog>().AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.VisitDate), e => e.VisitDate == request.Entity!.VisitDate)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.CategoryCode), e => e.CategoryCode == request.Entity!.CategoryCode)
            .OrderByDescending(e => e.VisitTime);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<VisitLog> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
