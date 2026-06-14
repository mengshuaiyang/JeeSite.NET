using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 角色实体，代表系统中的权限集合体。用户通过分配角色获得菜单/数据/业务范围内的访问权限。
/// </summary>
public class Role : DataEntity, ICorpEntity, IExtendEntity
{
    /// <summary>角色编码，业务主键。</summary>
    public string RoleCode { get; set; } = string.Empty;
    /// <summary>角色名称（如：系统管理员、普通用户）。</summary>
    public string RoleName { get; set; } = string.Empty;
    /// <summary>视图编码，用于权限分类/多视图展示。</summary>
    public string? ViewCode { get; set; }
    /// <summary>角色类型：assignment（分配）/ function（功能）/ data（数据）。</summary>
    public string? RoleType { get; set; }
    /// <summary>角色排序（数值越小越靠前）。</summary>
    public decimal? RoleSort { get; set; }
    /// <summary>是否系统内置角色：1=是（不可删除），0=否（可自定义），默认 0。</summary>
    public string? IsSys { get; set; } = "0";
    /// <summary>是否显示：1=显示，0=隐藏，默认 1。</summary>
    public string? IsShow { get; set; } = "1";
    /// <summary>适用用户类型（与 User.UserType 匹配）。</summary>
    public string? UserType { get; set; }
    /// <summary>角色默认桌面地址（登录后跳转首页）。</summary>
    public string? DesktopUrl { get; set; }
    /// <summary>数据范围控制：1=全部、2=所在公司、3=所在部门、4=本人、8=自定义。</summary>
    public string? DataScope { get; set; }
    /// <summary>业务范围编码列表（逗号分隔），用于业务权限控制。</summary>
    public string? BizScope { get; set; }
    /// <summary>允许访问的子系统/系统编码列表（逗号分隔）。</summary>
    public string? SysCodes { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }

    /// <summary>扩展字符串字段 1。</summary>
    public string? ExtendS1 { get; set; }
    /// <summary>扩展字符串字段 2。</summary>
    public string? ExtendS2 { get; set; }
    /// <summary>扩展字符串字段 3。</summary>
    public string? ExtendS3 { get; set; }
    /// <summary>扩展字符串字段 4。</summary>
    public string? ExtendS4 { get; set; }
    /// <summary>扩展字符串字段 5。</summary>
    public string? ExtendS5 { get; set; }
    /// <summary>扩展字符串字段 6。</summary>
    public string? ExtendS6 { get; set; }
    /// <summary>扩展字符串字段 7。</summary>
    public string? ExtendS7 { get; set; }
    /// <summary>扩展字符串字段 8。</summary>
    public string? ExtendS8 { get; set; }
    /// <summary>扩展整型字段 1。</summary>
    public int? ExtendI1 { get; set; }
    /// <summary>扩展整型字段 2。</summary>
    public int? ExtendI2 { get; set; }
    /// <summary>扩展整型字段 3。</summary>
    public int? ExtendI3 { get; set; }
    /// <summary>扩展整型字段 4。</summary>
    public int? ExtendI4 { get; set; }
    /// <summary>扩展十进制字段 1。</summary>
    public decimal? ExtendF1 { get; set; }
    /// <summary>扩展十进制字段 2。</summary>
    public decimal? ExtendF2 { get; set; }
    /// <summary>扩展十进制字段 3。</summary>
    public decimal? ExtendF3 { get; set; }
    /// <summary>扩展十进制字段 4。</summary>
    public decimal? ExtendF4 { get; set; }
    /// <summary>扩展日期字段 1。</summary>
    public DateTime? ExtendD1 { get; set; }
    /// <summary>扩展日期字段 2。</summary>
    public DateTime? ExtendD2 { get; set; }
    /// <summary>扩展日期字段 3。</summary>
    public DateTime? ExtendD3 { get; set; }
    /// <summary>扩展日期字段 4。</summary>
    public DateTime? ExtendD4 { get; set; }
    /// <summary>扩展 JSON 字段，用于存储自定义结构化数据。</summary>
    public string? ExtendJson { get; set; }
}
