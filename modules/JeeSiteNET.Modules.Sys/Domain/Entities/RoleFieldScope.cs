using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class RoleFieldScope : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityLabel { get; set; } = string.Empty;
    public string EntityClass { get; set; } = string.Empty;
    public string? FieldConfig { get; set; }
}
