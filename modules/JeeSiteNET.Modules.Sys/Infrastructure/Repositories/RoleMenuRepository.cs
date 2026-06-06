using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Infrastructure.Repositories;

public class RoleMenuRepository : IRoleMenuRepository
{
    private readonly JeeSiteDbContext _db;
    public RoleMenuRepository(JeeSiteDbContext db) => _db = db;

    public async Task<List<string>> GetMenuCodesByRoleAsync(string roleCode)
        => await _db.Set<RoleMenu>().Where(rm => rm.RoleCode == roleCode).Select(rm => rm.MenuCode).ToListAsync();

    public async Task SaveRoleMenusAsync(string roleCode, List<string> menuCodes)
    {
        var existing = await _db.Set<RoleMenu>().Where(rm => rm.RoleCode == roleCode).ToListAsync();
        _db.Set<RoleMenu>().RemoveRange(existing);
        foreach (var menuCode in menuCodes)
            _db.Set<RoleMenu>().Add(new RoleMenu { RoleCode = roleCode, MenuCode = menuCode });
        await _db.SaveChangesAsync();
    }

    public async Task<List<string>> GetMenuCodesByRolesAsync(List<string> roleCodes)
        => await _db.Set<RoleMenu>().Where(rm => roleCodes.Contains(rm.RoleCode)).Select(rm => rm.MenuCode).ToListAsync();
}
