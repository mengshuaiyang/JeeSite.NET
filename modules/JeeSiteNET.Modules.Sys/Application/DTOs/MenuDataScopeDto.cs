namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class MenuDataScopeDto
{
    public string Id { get; set; } = string.Empty;
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}
