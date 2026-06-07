using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly DbContext _db;

    public CompanyRepository(DbContext db)
    {
        _db = db;
    }

    public IQueryable<Company> Query() => _db.Set<Company>().AsNoTracking();

    public async Task<Company?> GetAsync(string companyCode)
        => await _db.Set<Company>().FindAsync(companyCode);

    public async Task<List<CompanyOffice>> GetOfficesAsync(string companyCode)
        => await _db.Set<CompanyOffice>().Where(e => e.CompanyCode == companyCode).AsNoTracking().ToListAsync();

    public async Task AddAsync(Company entity) => await _db.Set<Company>().AddAsync(entity);

    public Task UpdateAsync(Company entity)
    {
        _db.Set<Company>().Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string companyCode)
    {
        var entity = await _db.Set<Company>().FindAsync(companyCode);
        if (entity != null) _db.Set<Company>().Remove(entity);
        await DeleteOfficesAsync(companyCode);
    }

    public async Task AddOfficeAsync(CompanyOffice entity) => await _db.Set<CompanyOffice>().AddAsync(entity);

    public async Task DeleteOfficesAsync(string companyCode)
    {
        var list = await _db.Set<CompanyOffice>().Where(e => e.CompanyCode == companyCode).ToListAsync();
        _db.Set<CompanyOffice>().RemoveRange(list);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
