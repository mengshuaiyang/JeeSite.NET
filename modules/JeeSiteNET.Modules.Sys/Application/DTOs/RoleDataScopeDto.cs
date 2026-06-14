namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 角色-数据范围配置（菜单级）DTO。
/// </summary>
public class RoleDataScopeDto
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
    /// 规则名称。
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 规则类型（All/Self/Custom 等枚举文本）。
    /// </summary>
    public string RuleType { get; set; } = string.Empty;

    /// <summary>
    /// 规则自定义配置（SQL/表达式）。
    /// </summary>
    public string? RuleConfig { get; set; }
}

/// <summary>
/// 角色-数据范围配置保存请求 DTO。
/// </summary>
public class RoleDataScopeSaveDto
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
    /// 规则名称。
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 规则类型。
    /// </summary>
    public string RuleType { get; set; } = string.Empty;

    /// <summary>
    /// 规则配置。
    /// </summary>
    public string? RuleConfig { get; set; }
}
