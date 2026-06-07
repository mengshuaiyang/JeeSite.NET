namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class RoleDataScopeDto
{
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}

public class RoleDataScopeSaveDto
{
    public string RoleCode { get; set; } = string.Empty;
    public string MenuCode { get; set; } = string.Empty;
    public string RuleName { get; set; } = string.Empty;
    public string RuleType { get; set; } = string.Empty;
    public string? RuleConfig { get; set; }
}
