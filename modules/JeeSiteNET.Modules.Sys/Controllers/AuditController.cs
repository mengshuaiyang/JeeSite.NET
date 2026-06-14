using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.Services;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

/// <summary>审计与权限分析接口控制器，提供审计日志分页查询、Excel 导出、用户-菜单权限分析等接口。</summary>
[ApiController]
[Route("api/v1/sys/audit")]
[Permission("sys:audit:view")]
public class AuditController : ControllerBase

{
    private readonly AuditService _auditService;

    private readonly IUserRepository _userRepo;

    private readonly IMenuRepository _menuRepo;

    private readonly IRoleRepository _roleRepo;

    private readonly IUserRoleRepository _userRoleRepo;

    public AuditController(

        AuditService auditService,

        IUserRepository userRepo,

        IMenuRepository menuRepo,

        IRoleRepository roleRepo,

        IUserRoleRepository userRoleRepo)
    {
        _auditService = auditService;

        _userRepo = userRepo;

        _menuRepo = menuRepo;

        _roleRepo = roleRepo;

        _userRoleRepo = userRoleRepo;
    }

    /// <summary>HTTP POST - 分页查询列表，支持筛选与排序条件。</summary>
    [HttpPost("list")]
    public async Task<ApiResult<PageResult<Audit>>> List([FromBody] PageRequest<Audit> request)
    {
        var result = await _auditService.FindPageAsync(request);

        return ApiResult<PageResult<Audit>>.Ok(result);
    }

    /// <summary>HTTP GET - 以查询字符串形式获取审计日志列表。</summary>
    [HttpGet("list")]
    public async Task<ApiResult<PageResult<Audit>>> GetList([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20,

        [FromQuery] string? auditType = null, [FromQuery] string? loginCode = null)
    {
        var request = new PageRequest<Audit>

        {
            PageNo = pageNo,

            PageSize = pageSize,

            Entity = new Audit { AuditType = auditType, LoginCode = loginCode }

        };

        var result = await _auditService.FindPageAsync(request);

        return ApiResult<PageResult<Audit>>.Ok(result);
    }

    /// <summary>HTTP POST - 将审计日志导出为 Excel 文件并返回文件下载。</summary>
    [HttpPost("export")]
    public async Task<IActionResult> Export([FromBody] PageRequest<Audit> request)
    {
        request.PageSize = int.MaxValue;

        var result = await _auditService.FindPageAsync(request);

        var rows = result.List.Select(a => new AuditExportRow

        {
            AuditType = a.AuditType,

            UserCode = a.UserCode,

            LoginCode = a.LoginCode,

            UserName = a.UserName,

            Created = a.CreateDate

        }).ToList();

        var excel = new ExcelService();

        var bytes = excel.Export(rows, "审计日志");

        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "审计日志.xlsx");
    }

    /// <summary>HTTP GET - 获取用户权限分析视图（含角色与权限统计）。</summary>
    [HttpGet("user-permissions")]
    public async Task<ApiResult<List<UserPermissionRow>>> UserPermissions([FromQuery] string? loginCode = null)
    {
        var users = string.IsNullOrEmpty(loginCode)

            ? await _userRepo.Query().Where(u => u.Status == "0").ToListAsync()

            : await _userRepo.Query().Where(u => u.LoginCode!.Contains(loginCode) && u.Status == "0").ToListAsync();

        var result = new List<UserPermissionRow>();

        foreach (var user in users)
        {
            var roleCodes = await _userRoleRepo.GetRoleCodesByUserAsync(user.UserCode);

            var roles = await _roleRepo.Query().Where(r => roleCodes.Contains(r.RoleCode)).ToListAsync();

            var perms = await _menuRepo.GetPermissionsByRoleCodesAsync(roleCodes);

            result.Add(new UserPermissionRow

            {
                UserCode = user.UserCode,

                LoginCode = user.LoginCode,

                UserName = user.UserName,

                RoleCount = roles.Count,

                Roles = string.Join(", ", roles.Select(r => r.RoleName)),

                PermissionCount = perms.Count,

                Permissions = string.Join(", ", perms.Take(20))

            });
        }

        return ApiResult<List<UserPermissionRow>>.Ok(result);
    }

    /// <summary>HTTP GET - 获取指定用户在每个菜单上的权限明细。</summary>
    [HttpGet("menu-permissions/{userCode}")]
    public async Task<ApiResult<List<MenuPermissionRow>>> MenuPermissions(string userCode)
    {
        var roleCodes = await _userRoleRepo.GetRoleCodesByUserAsync(userCode);

        var allMenus = await _menuRepo.Query().Where(m => m.Status == "0").OrderBy(m => m.TreeSort).ToListAsync();

        var perms = await _menuRepo.GetPermissionsByRoleCodesAsync(roleCodes);

        var result = allMenus.Select(m => new MenuPermissionRow

        {
            MenuCode = m.MenuCode,

            MenuName = m.MenuName,

            Permission = m.Permission,

            HasPermission = string.IsNullOrEmpty(m.Permission) || perms.Contains(m.Permission)

        }).ToList();

        return ApiResult<List<MenuPermissionRow>>.Ok(result);
    }
}

/// <summary>审计日志 Excel 导出行模型。</summary>

public class AuditExportRow

{
    [ExcelField("审计类型", ColumnWidth = 15)]

    public string? AuditType { get; set; }

    [ExcelField("用户编码", ColumnWidth = 20)]

    public string? UserCode { get; set; }

    [ExcelField("登录名", ColumnWidth = 15)]

    public string? LoginCode { get; set; }

    [ExcelField("用户名", ColumnWidth = 20)]

    public string? UserName { get; set; }

    [ExcelField("创建时间", ColumnWidth = 20)]

    public DateTime? Created { get; set; }
}

/// <summary>用户权限分析视图行模型。</summary>

public class UserPermissionRow

{
    public string? UserCode { get; set; }

    public string? LoginCode { get; set; }

    public string? UserName { get; set; }

    public int RoleCount { get; set; }

    public string? Roles { get; set; }

    public int PermissionCount { get; set; }

    public string? Permissions { get; set; }
}

/// <summary>菜单权限分析视图行模型。</summary>

public class MenuPermissionRow

{
    public string? MenuCode { get; set; }

    public string? MenuName { get; set; }

    public string? Permission { get; set; }

    public bool HasPermission { get; set; }
}
