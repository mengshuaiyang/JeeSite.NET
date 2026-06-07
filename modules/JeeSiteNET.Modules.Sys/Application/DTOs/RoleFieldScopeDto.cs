using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class RoleFieldScopeDto
{
    public string Id { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityLabel { get; set; } = string.Empty;
    public string EntityClass { get; set; } = string.Empty;
    public string? FieldConfig { get; set; }
}

public class RoleFieldScopeSaveDto
{
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityLabel { get; set; } = string.Empty;
    public string EntityClass { get; set; } = string.Empty;
    public string? FieldConfig { get; set; }
}

public static class RoleFieldScopeMapping
{
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
