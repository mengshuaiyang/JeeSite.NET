using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Infrastructure.Repositories;

public class GuestbookRepository : IGuestbookRepository
{
    private readonly JeeSiteDbContext _db;
    public GuestbookRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Guestbook> Query() => _db.Set<Guestbook>().AsNoTracking();
    public async Task<Guestbook?> GetAsync(object id) => await _db.Set<Guestbook>().FindAsync(id);
    public async Task<List<Guestbook>> FindListAsync() => await _db.Set<Guestbook>().AsNoTracking().ToListAsync();
    public async Task AddAsync(Guestbook entity) { _db.Set<Guestbook>().Add(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Guestbook entity) { _db.Set<Guestbook>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Guestbook entity) { _db.Set<Guestbook>().Remove(entity); await _db.SaveChangesAsync(); }

    public async Task<PageResult<Guestbook>> FindPageAsync(PageRequest<Guestbook> request)
    {
        var query = _db.Set<Guestbook>().AsNoTracking()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.GbType), e => e.GbType == request.Entity!.GbType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status), e => e.Status == request.Entity!.Status)
            .OrderByDescending(e => e.CreateDate);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<Guestbook> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }
}
