using JeeSiteNET.Core;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly JeeSiteDbContext _db;

    public TenantRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Tenant> Query() => _db.Set<Tenant>();

    public async Task<Tenant?> GetAsync(object id)
        => await _db.Set<Tenant>().FirstOrDefaultAsync(e => e.TenantCode == id.ToString());

    public async Task<List<Tenant>> FindListAsync()
        => await _db.Set<Tenant>().ToListAsync();

    public async Task<PageResult<Tenant>> FindPageAsync(PageRequest<Tenant> request)
    {
        var query = _db.Set<Tenant>().AsQueryable();
        var total = await query.CountAsync();
        var list = await query.OrderBy(e => e.TenantCode)
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize).ToListAsync();
        return new PageResult<Tenant> { List = list, Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    public async Task AddAsync(Tenant entity) { await _db.Set<Tenant>().AddAsync(entity); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(Tenant entity) { _db.Set<Tenant>().Update(entity); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Tenant entity) { _db.Set<Tenant>().Remove(entity); await _db.SaveChangesAsync(); }
}
