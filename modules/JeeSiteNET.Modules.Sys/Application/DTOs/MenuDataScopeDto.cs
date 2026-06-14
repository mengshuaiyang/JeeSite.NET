namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 菜单级数据范围配置 DTO。
/// </summary>
public class MenuDataScopeDto
{
    /// <summary>
    /// 主键标识。
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 角色编码（数据范围属于哪个角色）。
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单编码（数据范围针对哪个菜单）。
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 规则名称（自定义描述）。
    /// </summary>
    public string RuleName { get; set; } = string.Empty;

    /// <summary>
    /// 规则类型（All/Self/Custom 等枚举文本）。
    /// </summary>
    public string RuleType { get; set; } = string.Empty;

    /// <summary>
    /// 规则配置（自定义 SQL 或表达式）。
    /// </summary>
    public string? RuleConfig { get; set; }
}
