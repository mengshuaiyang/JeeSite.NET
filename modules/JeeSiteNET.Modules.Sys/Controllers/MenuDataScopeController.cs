using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/menu-data-scope")]
[Permission("sys:menu:data-scope")]
public class MenuDataScopeController : ControllerBase
{
    private readonly JeeSiteDbContext _db;

    public MenuDataScopeController(JeeSiteDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ApiResult<List<MenuDataScopeDtoInline>>> GetByRole(string roleCode)
    {
        var list = await _db.Set<MenuDataScope>()
            .Where(e => e.RoleCode == roleCode)
            .ToListAsync();
        return ApiResult<List<MenuDataScopeDtoInline>>.Ok(list.Select(MapToDtoInline).ToList());
    }

    [HttpPost]
    public async Task<ApiResult> Save([FromBody] MenuDataScopeSaveDtoInline dto)
    {
        var entity = new MenuDataScope
        {
            Id = IdGenerator.NewId(),
            RoleCode = dto.RoleCode,
            MenuCode = dto.MenuCode,
            RuleName = dto.RuleName,
            RuleType = dto.RuleType,
            RuleConfig = dto.RuleConfig
        };
        await _db.Set<MenuDataScope>().AddAsync(entity);
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    [HttpPut("{id}")]
    public async Task<ApiResult> Update(string id, [FromBody] MenuDataScopeSaveDtoInline dto)
    {
        var entity = await _db.Set<MenuDataScope>().FindAsync(id);
        if (entity == null) return ApiResult.NotFound("数据权限配置不存在");
        entity.RuleName = dto.RuleName;
        entity.RuleType = dto.RuleType;
        entity.RuleConfig = dto.RuleConfig;
        _db.Set<MenuDataScope>().Update(entity);
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ApiResult> Delete(string id)
    {
        var entity = await _db.Set<MenuDataScope>().FindAsync(id);
        if (entity != null)
        {
            _db.Set<MenuDataScope>().Remove(entity);
            await _db.SaveChangesAsync();
        }
        return ApiResult.Ok();
    }

    private static MenuDataScopeDtoInline MapToDtoInline(MenuDataScope e) => new()
    {
        Id = e.Id,
        RoleCode = e.RoleCode,
        MenuCode = e.MenuCode,
        RuleName = e.RuleName,
        RuleType = e.RuleType,
        RuleConfig = e.RuleConfig
    };
}

public class MenuDataScopeSaveDtoInline
{
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}

public class MenuDataScopeDtoInline
{
    public string Id { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}
