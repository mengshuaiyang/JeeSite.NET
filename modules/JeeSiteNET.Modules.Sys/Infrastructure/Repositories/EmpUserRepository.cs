using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class EmpUserRepository : IEmpUserRepository
{
    private readonly JeeSiteDbContext _db;
    public EmpUserRepository(JeeSiteDbContext db) => _db = db;

    public IQueryable<EmpUser> Query() => _db.Set<EmpUser>().AsNoTracking();

    public async Task<EmpUser?> GetAsync(string empCode, string userCode)
        => await _db.Set<EmpUser>().FindAsync(empCode, userCode);

    public async Task<List<EmpUser>> GetByEmpCodeAsync(string empCode)
        => await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).ToListAsync();

    public async Task<List<EmpUser>> GetByUserCodeAsync(string userCode)
        => await _db.Set<EmpUser>().Where(e => e.UserCode == userCode).ToListAsync();

    public async Task<List<string>> GetUserCodesByEmpCodeAsync(string empCode)
        => await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).Select(e => e.UserCode).ToListAsync();

    public async Task<List<string>> GetEmpCodesByUserCodeAsync(string userCode)
        => await _db.Set<EmpUser>().Where(e => e.UserCode == userCode).Select(e => e.EmpCode).ToListAsync();

    public async Task AddAsync(EmpUser entity)
    {
        _db.Set<EmpUser>().Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string empCode, string userCode)
    {
        var entity = await _db.Set<EmpUser>().FindAsync(empCode, userCode);
        if (entity != null)
        {
            _db.Set<EmpUser>().Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    public async Task DeleteByEmpCodeAsync(string empCode)
    {
        var entities = await _db.Set<EmpUser>().Where(e => e.EmpCode == empCode).ToListAsync();
        _db.Set<EmpUser>().RemoveRange(entities);
        await _db.SaveChangesAsync();
    }
}
