using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class MenuDataScope : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}
