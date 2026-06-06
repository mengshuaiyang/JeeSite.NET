using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly JeeSiteDbContext _db;

    public UserRoleRepository(JeeSiteDbContext db) => _db = db;

    public async Task<List<string>> GetRoleCodesByUserAsync(string userCode)
        => await _db.Set<UserRole>()
            .Where(ur => ur.UserCode == userCode)
            .Select(ur => ur.RoleCode)
            .ToListAsync();

    public async Task SaveUserRolesAsync(string userCode, List<string> roleCodes)
    {
        var existing = await _db.Set<UserRole>()
            .Where(ur => ur.UserCode == userCode)
            .ToListAsync();

        _db.Set<UserRole>().RemoveRange(existing);

        foreach (var roleCode in roleCodes)
        {
            _db.Set<UserRole>().Add(new UserRole
            {
                UserCode = userCode,
                RoleCode = roleCode
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task DeleteByUserAsync(string userCode)
    {
        var existing = await _db.Set<UserRole>()
            .Where(ur => ur.UserCode == userCode)
            .ToListAsync();

        _db.Set<UserRole>().RemoveRange(existing);
        await _db.SaveChangesAsync();
    }
}
