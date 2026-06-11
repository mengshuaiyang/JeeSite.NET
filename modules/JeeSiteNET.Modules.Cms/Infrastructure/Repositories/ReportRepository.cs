using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly JeeSiteDbContext _db;
    public ReportRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Report> Query() => _db.Set<Report>().AsNoTracking();
    public async Task<Report?> GetAsync(object id) => await _db.Set<Report>().FindAsync(id);
    public async Task<List<Report>> FindListAsync() => await _db.Set<Report>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Report entity) { _db.Set<Report>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Report entity) { _db.Set<Report>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Report entity) { _db.Set<Report>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<PageResult<Report>> FindPageAsync(PageRequest<Report> request)
    {
        var query = _db.Set<Report>().AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.ReportType), e => e.ReportType == request.Entity!.ReportType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            .OrderByDescending(e => e.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<Report> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
