using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 角色-字段权限范围 DTO（数据列级权限）。
/// </summary>
public class RoleFieldScopeDto
{
    /// <summary>
    /// 主键标识。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码。
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（权限作用域）。
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 实体/表名称。
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// 实体显示名称。
    /// </summary>
    public string EntityLabel { get; set; } = string.Empty;

    /// <summary>
    /// 实体类（程序集限定名或命名空间+类名）。
    /// </summary>
    public string EntityClass { get; set; } = string.Empty;

    /// <summary>
    /// 字段级权限配置（JSON：{字段名: 权限}）。
    /// </summary>
    public string? FieldConfig { get; set; }
}

/// <summary>
/// 角色-字段权限范围保存请求 DTO。
/// </summary>
public class RoleFieldScopeSaveDto
{
    /// <summary>
    /// 角色编码。
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码。
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 实体/表名称。
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// 实体显示名称。
    /// </summary>
    public string EntityLabel { get; set; } = string.Empty;

    /// <summary>
    /// 实体类。
    /// </summary>
    public string EntityClass { get; set; } = string.Empty;

    /// <summary>
    /// 字段级权限配置。
    /// </summary>
    public string? FieldConfig { get; set; }
}

/// <summary>
/// 角色字段权限映射扩展方法集合。
/// </summary>
public static class RoleFieldScopeMapping
{
    /// <summary>
    /// 将 <see cref="RoleFieldScope"/> 实体转换为 <see cref="RoleFieldScopeDto"/>。
    /// </summary>
    /// <param name="e">实体。</param>
    /// <returns>字段权限 DTO。</returns>
    public static RoleFieldScopeDto ToDto(this RoleFieldScope e) => new()
    {
        Id = e.Id,
        RoleCode = e.RoleCode,
        MenuCode = e.MenuCode,
        EntityName = e.EntityName,
        EntityLabel = e.EntityLabel,
        EntityClass = e.EntityClass,
        FieldConfig = e.FieldConfig
    };

    /// <summary>
    /// 将 <see cref="RoleFieldScopeSaveDto"/> 转换为 <see cref="RoleFieldScope"/> 实体（生成主键）。
    /// </summary>
    /// <param name="dto">保存请求 DTO。</param>
    /// <returns>字段权限实体。</returns>
    public static RoleFieldScope ToEntity(this RoleFieldScopeSaveDto dto) => new()
    {
        Id = IdGenerator.NewId(),
        RoleCode = dto.RoleCode,
        MenuCode = dto.MenuCode,
        EntityName = dto.EntityName,
        EntityLabel = dto.EntityLabel,
        EntityClass = dto.EntityClass,
        FieldConfig = dto.FieldConfig
    };
}
