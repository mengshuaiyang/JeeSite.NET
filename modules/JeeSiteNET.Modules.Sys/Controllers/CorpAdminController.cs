    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/corp-admin")]
[Permission("sys:corpAdmin:view")]
// 定义class CorpAdminController
// 定义类：CorpAdminController

public class CorpAdminController : ControllerBase
{
    // 字段 _userRepo
    // 字段：_userRepo

    private readonly IUserRepository _userRepo;
    // 字段 _roleRepo
    // 字段：_roleRepo

    private readonly IRoleRepository _roleRepo;
    // 字段 _orgRepo
    // 字段：_orgRepo

    private readonly IOrganizationRepository _orgRepo;
    // 字段 _currentUser
    // 字段：_currentUser

    private readonly ICurrentUser _currentUser;

    // 构造函数 CorpAdminController
    // 构造函数：CorpAdminController

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
    // 方法 GetUsers
    // 方法：GetUsers

    public async Task<ApiResult<List<User>>> GetUsers()
    {
        // 调用 Query
        var list = await _userRepo.Query()
            // 数据库操作：条件过滤
            .Where(u => u.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            // 数据库操作：升序排序
            .OrderBy(u => u.CreateDate)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<User>>.Ok(list);
    }

    [HttpGet("roles")]
    // 方法 GetRoles
    // 方法：GetRoles

    public async Task<ApiResult<List<Role>>> GetRoles()
    {
        // 调用 Query
        var list = await _roleRepo.Query()
            // 数据库操作：条件过滤
            .Where(r => r.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            // 数据库操作：升序排序
            .OrderBy(r => r.RoleSort)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<Role>>.Ok(list);
    }

    [HttpGet("orgs")]
    // 方法 GetOrgs
    // 方法：GetOrgs

    public async Task<ApiResult<List<Organization>>> GetOrgs()
    {
        // 调用 Query
        var list = await _orgRepo.Query()
            // 数据库操作：条件过滤
            .Where(o => o.CorpCode == _currentUser.OrgCode || _currentUser.IsSuperAdmin)
            // 数据库操作：升序排序
            .OrderBy(o => o.TreeSort)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<Organization>>.Ok(list);
    }
}
