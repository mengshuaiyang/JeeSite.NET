using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 菜单实体，树形结构（TreeEntity），代表系统功能菜单。支持目录/菜单/按钮三级，
/// 并承载前端路由、权限标识、图标等配置。
/// </summary>
public class Menu : TreeEntity, ICorpEntity, IExtendEntity
{
    /// <summary>菜单编码，业务主键。</summary>
    public string MenuCode { get; set; } = string.Empty;
    /// <summary>菜单名称（左侧导航显示文字）。</summary>
    public string MenuName { get; set; } = string.Empty;
    /// <summary>菜单类型：1=目录、2=菜单、3=按钮。</summary>
    public string? MenuType { get; set; }
    /// <summary>菜单链接地址（路由或外部 URL）。</summary>
    public string? MenuHref { get; set; }
    /// <summary>菜单目标：iframe / menuItem / blank / dialog / _self。</summary>
    public string? MenuTarget { get; set; }
    /// <summary>菜单图标（Ant Design / FontAwesome 图标标识）。</summary>
    public string? MenuIcon { get; set; }
    /// <summary>菜单字体颜色（CSS color）。</summary>
    public string? MenuColor { get; set; }
    /// <summary>菜单鼠标悬停标题（title 属性）。</summary>
    public string? MenuTitle { get; set; }
    /// <summary>权限标识字符串（Shiro/HasPermission 判断依据，如 sys:user:add）。</summary>
    public string? Permission { get; set; }
    /// <summary>菜单权重，同级内排序（数字越小越靠前）。</summary>
    public decimal? Weight { get; set; } = 0;
    /// <summary>是否显示：1=显示，0=隐藏。</summary>
    public string? IsShow { get; set; } = "1";
    /// <summary>所属子系统编码。</summary>
    public string? SysCode { get; set; }
    /// <summary>所属模块编码列表（逗号分隔）。</summary>
    public string? ModuleCodes { get; set; }
    /// <summary>所属模块编码（单个）。</summary>
    public string? ModuleCode { get; set; }
    /// <summary>前端组件路径（Vue Router component）。</summary>
    public string? Component { get; set; }
    /// <summary>路由参数 JSON。</summary>
    public string? Params { get; set; }

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

    /// <summary>获取菜单名称（用于 TreeEntity 展示）。</summary>
    public override string GetName() => MenuName;
}
