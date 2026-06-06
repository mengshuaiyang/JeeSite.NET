using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly JeeSiteDbContext _db;

    public OrganizationRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<Organization> Query() => _db.Set<Organization>().AsNoTracking();

    public async Task<Organization?> GetAsync(object id)
        => await _db.Set<Organization>().FindAsync(id);

    public async Task<List<Organization>> FindListAsync()
        => await _db.Set<Organization>().AsNoTracking().ToListAsync();

    public async Task AddAsync(Organization entity)
    {
        await _db.Set<Organization>().AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Organization entity)
    {
        _db.Set<Organization>().Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Organization entity)
    {
        _db.Set<Organization>().Remove(entity);
        await _db.SaveChangesAsync();
    }
}
