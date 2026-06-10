namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class RoleDto
{
    [Core.Utils.ExcelField("角色编码")]
    public string RoleCode { get; set; } = string.Empty;
    [Core.Utils.ExcelField("角色名称", ColumnWidth = 25)]
    public string RoleName { get; set; } = string.Empty;
    [Core.Utils.ExcelField("角色类型")]
    public string? RoleType { get; set; }
    [Core.Utils.ExcelField("是否系统")]
    public string? IsSys { get; set; }
    [Core.Utils.ExcelField("用户类型")]
    public string? UserType { get; set; }
    [Core.Utils.ExcelField("排序")]
    public decimal? Sort { get; set; }
    [Core.Utils.ExcelField("状态")]
    public string? Status { get; set; }
    [Core.Utils.ExcelField("创建时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? CreateDate { get; set; }
}

public class RoleSaveDto
{
    public string? RoleCode { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string? RoleType { get; set; }
    public string? IsSys { get; set; }
    public string? UserType { get; set; }
    public decimal? Sort { get; set; }
}

public class MenuDto
{
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? MenuHref { get; set; }
    public string? MenuTarget { get; set; }
    public string? MenuIcon { get; set; }
    public string? Permission { get; set; }
    public decimal? Weight { get; set; }
    public string? IsShow { get; set; }
    public string? ModuleCode { get; set; }
    public string ParentCode { get; set; } = "0";
    public string ParentCodes { get; set; } = string.Empty;
    public decimal TreeSort { get; set; } = 1000;
    public string TreeNames { get; set; } = string.Empty;
    public decimal TreeLevel { get; set; } = 0;
    public string TreeLeaf { get; set; } = "1";
    public string? Status { get; set; }
    public List<MenuDto>? Children { get; set; }
}

public class MenuSaveDto
{
    public string? MenuCode { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string? MenuHref { get; set; }
    public string? MenuTarget { get; set; }
    public string? MenuIcon { get; set; }
    public string? Permission { get; set; }
    public decimal? Weight { get; set; }
    public string? IsShow { get; set; }
    public string? ModuleCode { get; set; }
    public string ParentCode { get; set; } = "0";
    public decimal TreeSort { get; set; } = 1000;
}

public class OrganizationDto
{
    public string OrgCode { get; set; } = string.Empty;
    public string OrgName { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string? FullName { get; set; }
    public string? OrgType { get; set; }
    public string? Leader { get; set; }
    public string? Phone { get; set; }
    public string ParentCode { get; set; } = "0";
    public string ParentCodes { get; set; } = string.Empty;
    public decimal TreeSort { get; set; } = 1000;
    public string TreeNames { get; set; } = string.Empty;
    public decimal TreeLevel { get; set; } = 0;
    public string TreeLeaf { get; set; } = "1";
    public string? Status { get; set; }
    public List<OrganizationDto>? Children { get; set; }
}

public class OrganizationSaveDto
{
    public string? OrgCode { get; set; }
    public string OrgName { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string? FullName { get; set; }
    public string? OrgType { get; set; }
    public string? Leader { get; set; }
    public string? Phone { get; set; }
    public string ParentCode { get; set; } = "0";
    public decimal TreeSort { get; set; } = 1000;
}
