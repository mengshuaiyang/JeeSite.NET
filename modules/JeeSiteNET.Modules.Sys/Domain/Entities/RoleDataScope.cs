using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 角色-菜单-数据权限规则实体，描述角色在指定菜单下的数据行级过滤规则（自定义 SQL 条件/表达式）。
/// </summary>
public class RoleDataScope : DataEntity
{
    /// <summary>角色编码（引用 Role.RoleCode）。</summary>
    public string RoleCode { get; set; } = string.Empty;
    /// <summary>菜单编码（引用 Menu.MenuCode），限定规则生效的功能入口。</summary>
    public string MenuCode { get; set; } = string.Empty;
    /// <summary>规则名称。</summary>
    public string RuleName { get; set; } = string.Empty;
    /// <summary>规则类型：sql（SQL 片段）、expression（表达式）、dept（部门过滤）、creator（创建者过滤）。</summary>
    public string RuleType { get; set; } = string.Empty;
    /// <summary>规则配置内容（SQL 片段、表达式体、部门 ID 列表等 JSON）。</summary>
    public string? RuleConfig { get; set; }
}
