    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
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
[Route("api/v1/sys/menu-data-scope")]
[Permission("sys:menu:data-scope")]
// 定义class MenuDataScopeController
// 定义类：MenuDataScopeController

public class MenuDataScopeController : ControllerBase
{
    // 字段 _db
    // 字段：_db

    private readonly JeeSiteDbContext _db;

    // 方法 MenuDataScopeController
    // 构造函数：MenuDataScopeController

    public MenuDataScopeController(JeeSiteDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    // 方法 GetByRole
    // 方法：GetByRole

    public async Task<ApiResult<List<MenuDataScopeDtoInline>>> GetByRole(string roleCode)
    {
        var list = await _db.Set<MenuDataScope>()
            // 数据库操作：条件过滤
            .Where(e => e.RoleCode == roleCode)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return ApiResult<List<MenuDataScopeDtoInline>>.Ok(list.Select(MapToDtoInline).ToList());
    }

    [HttpPost]
    // 方法 Save
    // 方法：Save

    public async Task<ApiResult> Save([FromBody] MenuDataScopeSaveDtoInline dto)
    {
        // 创建 MenuDataScope实例并赋给 entity
        var entity = new MenuDataScope
        {
            Id = IdGenerator.NewId(),
            RoleCode = dto.RoleCode,
            MenuCode = dto.MenuCode,
            RuleName = dto.RuleName,
            RuleType = dto.RuleType,
            RuleConfig = dto.RuleConfig
        };
        // await 异步等待
        await _db.Set<MenuDataScope>().AddAsync(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpPut("{id}")]
    // 方法 Update
    // 方法：Update

    public async Task<ApiResult> Update(string id, [FromBody] MenuDataScopeSaveDtoInline dto)
    {
        var entity = await _db.Set<MenuDataScope>().FindAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("数据权限配置不存在");
        entity.RuleName = dto.RuleName;
        entity.RuleType = dto.RuleType;
        entity.RuleConfig = dto.RuleConfig;
        // 调用 Update
        _db.Set<MenuDataScope>().Update(entity);
        // await 异步等待
        await _db.SaveChangesAsync();
        // return 返回结果
        return ApiResult.Ok();
    }

    [HttpDelete("{id}")]
    // 方法 Delete
    // 方法：Delete

    public async Task<ApiResult> Delete(string id)
    {
        var entity = await _db.Set<MenuDataScope>().FindAsync(id);
        // if 条件判断
        if (entity != null)
        {
            // 集合操作：移除元素
            _db.Set<MenuDataScope>().Remove(entity);
            // await 异步等待
            await _db.SaveChangesAsync();
        }
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 MapToDtoInline
    // 方法：MapToDtoInline

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

// 定义class MenuDataScopeSaveDtoInline
// 定义类：MenuDataScopeSaveDtoInline

public class MenuDataScopeSaveDtoInline
{
    // 属性 RoleCode
    // 属性：RoleCode

    public string RoleCode { get; set; } = string.Empty;
    // 属性 MenuCode
    // 属性：MenuCode

    public string MenuCode { get; set; } = string.Empty;
    // 属性 RuleName
    // 属性：RuleName

    public string RuleName { get; set; } = string.Empty;
    // 属性 RuleType
    // 属性：RuleType

    public string RuleType { get; set; } = string.Empty;
    // 属性：RuleConfig

    public string? RuleConfig { get; set; }
}

// 定义class MenuDataScopeDtoInline
// 定义类：MenuDataScopeDtoInline

public class MenuDataScopeDtoInline
{
    // 属性 Id
    // 属性：Id

    public string Id { get; set; } = string.Empty;
    // 属性 RoleCode
    // 属性：RoleCode

    public string RoleCode { get; set; } = string.Empty;
    // 属性 MenuCode
    // 属性：MenuCode

    public string MenuCode { get; set; } = string.Empty;
    // 属性 RuleName
    // 属性：RuleName

    public string RuleName { get; set; } = string.Empty;
    // 属性 RuleType
    // 属性：RuleType

    public string RuleType { get; set; } = string.Empty;
    // 属性：RuleConfig

    public string? RuleConfig { get; set; }
}
