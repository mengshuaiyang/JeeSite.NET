using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class UserDataScopeRepository : IUserDataScopeRepository
{
    private readonly JeeSiteDbContext _db;

    public UserDataScopeRepository(JeeSiteDbContext db)
    {
        _db = db;
    }

    public IQueryable<UserDataScope> Query() => _db.Set<UserDataScope>().AsNoTracking();

    public async Task<UserDataScope?> GetByUserAsync(string userCode)
    {
        return await _db.Set<UserDataScope>()
            .FirstOrDefaultAsync(e => e.UserCode == userCode);
    }

    public async Task AddAsync(UserDataScope entity)
    {
        await _db.Set<UserDataScope>().AddAsync(entity);
    }

    public async Task UpdateAsync(UserDataScope entity)
    {
        _db.Set<UserDataScope>().Update(entity);
    }

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}
