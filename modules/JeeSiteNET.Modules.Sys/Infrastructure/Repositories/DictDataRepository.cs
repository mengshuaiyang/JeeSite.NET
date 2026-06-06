using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class DictDataRepository : IDictDataRepository
{
    private readonly JeeSiteDbContext _db;

    public DictDataRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<DictData> Query() => _db.Set<DictData>().AsNoTracking();

    public async Task<DictData?> GetAsync(object id)
        => await _db.Set<DictData>().FindAsync(id);

    public async Task<List<DictData>> FindListAsync()
        => await _db.Set<DictData>().AsNoTracking().ToListAsync();

    public async Task AddAsync(DictData entity)
    {
        await _db.Set<DictData>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(DictData entity)
    {
        _db.Set<DictData>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(DictData entity)
    {
        _db.Set<DictData>().Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<List<DictData>> GetByTypeAsync(string dictType)
        => await _db.Set<DictData>().AsNoTracking()
            .Where(d => d.DictType == dictType && d.Status == "0")
            .OrderBy(d => d.Sort)
            .ToListAsync();
}
