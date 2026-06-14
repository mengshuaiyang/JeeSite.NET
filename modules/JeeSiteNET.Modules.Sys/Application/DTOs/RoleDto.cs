namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 角色信息 DTO。
/// </summary>
public class RoleDto
{
    /// <summary>
    /// 角色编码（主键）。
    /// </summary>
    [Core.Utils.ExcelField("角色编码")]
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 角色显示名称；带列宽 25。
    /// </summary>
    [Core.Utils.ExcelField("角色名称", ColumnWidth = 25)]
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 角色类型（admin/user/custom 等）。
    /// </summary>
    [Core.Utils.ExcelField("角色类型")]
    public string? RoleType { get; set; }

    /// <summary>
    /// 是否系统内置角色（1 是 / 0 否）。
    /// </summary>
    [Core.Utils.ExcelField("是否系统")]
    public string? IsSys { get; set; }

    /// <summary>
    /// 适用用户类型。
    /// </summary>
    [Core.Utils.ExcelField("用户类型")]
    public string? UserType { get; set; }

    /// <summary>
    /// 排序值（数字越大越靠前/靠后，由前端解释）。
    /// </summary>
    [Core.Utils.ExcelField("排序")]
    public decimal? Sort { get; set; }

    /// <summary>
    /// 角色状态。
    /// </summary>
    [Core.Utils.ExcelField("状态")]
    public string? Status { get; set; }

    /// <summary>
    /// 创建时间；带时间格式化与列宽。
    /// </summary>
    [Core.Utils.ExcelField("创建时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? CreateDate { get; set; }
}

/// <summary>
/// 角色保存请求 DTO。
/// </summary>
public class RoleSaveDto
{
    /// <summary>
    /// 角色编码；为空表示新建。
    /// </summary>
    public string? RoleCode { get; set; }

    /// <summary>
    /// 角色名称。
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 角色类型。
    /// </summary>
    public string? RoleType { get; set; }

    /// <summary>
    /// 是否系统内置。
    /// </summary>
    public string? IsSys { get; set; }

    /// <summary>
    /// 适用用户类型。
    /// </summary>
    public string? UserType { get; set; }

    /// <summary>
    /// 排序值。
    /// </summary>
    public decimal? Sort { get; set; }
}

/// <summary>
/// 菜单信息 DTO（含树形父子结构）。
/// </summary>
public class MenuDto
{
    /// <summary>
    /// 菜单编码。
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单名称。
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 前端路由路径。
    /// </summary>
    public string? MenuHref { get; set; }

    /// <summary>
    /// 打开方式（当前窗口/新窗口）。
    /// </summary>
    public string? MenuTarget { get; set; }

    /// <summary>
    /// 图标标识。
    /// </summary>
    public string? MenuIcon { get; set; }

    /// <summary>
    /// 权限标识（用于权限校验）。
    /// </summary>
    public string? Permission { get; set; }

    /// <summary>
    /// 权重（用于同层排序）。
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 是否显示（1 显示 / 0 隐藏）。
    /// </summary>
    public string? IsShow { get; set; }

    /// <summary>
    /// 所属系统编码。
    /// </summary>
    public string? SysCode { get; set; }

    /// <summary>
    /// 所属模块编码。
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// 父级菜单编码；根节点默认为 "0"。
    /// </summary>
    public string ParentCode { get; set; } = "0";

    /// <summary>
    /// 父级菜单编码的完整路径（逗号分隔）。
    /// </summary>
    public string ParentCodes { get; set; } = string.Empty;

    /// <summary>
    /// 同级排序（数字）。
    /// </summary>
    public decimal TreeSort { get; set; } = 1000;

    /// <summary>
    /// 节点名称完整路径（逗号分隔）。
    /// </summary>
    public string TreeNames { get; set; } = string.Empty;

    /// <summary>
    /// 节点层级（0 为根）。
    /// </summary>
    public decimal TreeLevel { get; set; } = 0;

    /// <summary>
    /// 是否叶子节点（1 是 / 0 否）。
    /// </summary>
    public string TreeLeaf { get; set; } = "1";

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 子菜单集合（树形渲染用）。
    /// </summary>
    public List<MenuDto>? Children { get; set; }
}

/// <summary>
/// 菜单保存请求 DTO。
/// </summary>
public class MenuSaveDto
{
    /// <summary>
    /// 菜单编码；空表示新建。
    /// </summary>
    public string? MenuCode { get; set; }

    /// <summary>
    /// 菜单名称。
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 路由路径。
    /// </summary>
    public string? MenuHref { get; set; }

    /// <summary>
    /// 打开方式。
    /// </summary>
    public string? MenuTarget { get; set; }

    /// <summary>
    /// 图标。
    /// </summary>
    public string? MenuIcon { get; set; }

    /// <summary>
    /// 权限标识。
    /// </summary>
    public string? Permission { get; set; }

    /// <summary>
    /// 权重。
    /// </summary>
    public decimal? Weight { get; set; }

    /// <summary>
    /// 是否显示。
    /// </summary>
    public string? IsShow { get; set; }

    /// <summary>
    /// 所属模块编码。
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// 父级菜单编码。
    /// </summary>
    public string ParentCode { get; set; } = "0";

    /// <summary>
    /// 同级排序。
    /// </summary>
    public decimal TreeSort { get; set; } = 1000;
}

/// <summary>
/// 机构信息 DTO（树形结构）。
/// </summary>
public class OrganizationDto
{
    /// <summary>
    /// 机构编码。
    /// </summary>
    public string OrgCode { get; set; } = string.Empty;

    /// <summary>
    /// 机构名称。
    /// </summary>
    public string OrgName { get; set; } = string.Empty;

    /// <summary>
    /// 视图编码（用于多租户或多视图划分）。
    /// </summary>
    public string? ViewCode { get; set; }

    /// <summary>
    /// 机构完整名称。
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 机构类型（company/dept/office 等）。
    /// </summary>
    public string? OrgType { get; set; }

    /// <summary>
    /// 负责人。
    /// </summary>
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话。
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 父级机构编码。
    /// </summary>
    public string ParentCode { get; set; } = "0";

    /// <summary>
    /// 父级机构完整路径。
    /// </summary>
    public string ParentCodes { get; set; } = string.Empty;

    /// <summary>
    /// 同级排序。
    /// </summary>
    public decimal TreeSort { get; set; } = 1000;

    /// <summary>
    /// 名称完整路径。
    /// </summary>
    public string TreeNames { get; set; } = string.Empty;

    /// <summary>
    /// 层级。
    /// </summary>
    public decimal TreeLevel { get; set; } = 0;

    /// <summary>
    /// 是否叶子节点。
    /// </summary>
    public string TreeLeaf { get; set; } = "1";

    /// <summary>
    /// 状态。
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 子机构集合。
    /// </summary>
    public List<OrganizationDto>? Children { get; set; }
}

/// <summary>
/// 机构保存请求 DTO。
/// </summary>
public class OrganizationSaveDto
{
    /// <summary>
    /// 机构编码；空表示新建。
    /// </summary>
    public string? OrgCode { get; set; }

    /// <summary>
    /// 机构名称。
    /// </summary>
    public string OrgName { get; set; } = string.Empty;

    /// <summary>
    /// 视图编码。
    /// </summary>
    public string? ViewCode { get; set; }

    /// <summary>
    /// 机构完整名称。
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    /// 机构类型。
    /// </summary>
    public string? OrgType { get; set; }

    /// <summary>
    /// 负责人。
    /// </summary>
    public string? Leader { get; set; }

    /// <summary>
    /// 联系电话。
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 父级机构编码。
    /// </summary>
    public string ParentCode { get; set; } = "0";

    /// <summary>
    /// 同级排序。
    /// </summary>
    public decimal TreeSort { get; set; } = 1000;
}
