using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/corp-admin")]
[Permission("sys:corpAdmin:view")]
public class CorpAdminController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IOrganizationRepository _orgRepo;
    private readonly ICurrentUser _currentUser;

    public CorpAdminController(
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        IOrganizationRepository orgRepo,
        ICurrentUser currentUser)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _orgRepo = orgRepo;
        _currentUser = currentUser;
    }

    [HttpGet("users")]
    public async Task<ApiResult<List<User>>> GetUsers()
    {
        var list = await _userRepo.Query()
            .Where(u => u.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            .OrderBy(u => u.CreateDate)
            .ToListAsync();
        return ApiResult<List<User>>.Ok(list);
    }

    [HttpGet("roles")]
    public async Task<ApiResult<List<Role>>> GetRoles()
    {
        var list = await _roleRepo.Query()
            .Where(r => r.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            .OrderBy(r => r.RoleSort)
            .ToListAsync();
        return ApiResult<List<Role>>.Ok(list);
    }

    [HttpGet("orgs")]
    public async Task<ApiResult<List<Organization>>> GetOrgs()
    {
        var list = await _orgRepo.Query()
            .Where(o => o.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            .OrderBy(o => o.TreeSort)
            .ToListAsync();
        return ApiResult<List<Organization>>.Ok(list);
    }
}
