using JeeSiteNET.Core.Utils;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class UserDto
{
    [ExcelField("用户编码")]
    public string UserCode { get; set; } = string.Empty;
    [ExcelField("登录名")]
    public string LoginCode { get; set; } = string.Empty;
    [ExcelField("用户姓名")]
    public string UserName { get; set; } = string.Empty;
    [ExcelField("用户类型")]
    public string UserType { get; set; } = "employee";
    [ExcelField("头像", IsExport = false)]
    public string? Avatar { get; set; }
    [ExcelField("邮箱")]
    public string? Email { get; set; }
    [ExcelField("手机号")]
    public string? Phone { get; set; }
    [ExcelField("机构编码")]
    public string? OrgCode { get; set; }
    [ExcelField("机构名称")]
    public string? OrgName { get; set; }
    [ExcelField("状态")]
    public string? Status { get; set; }
    [ExcelField("最后登录时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? LoginDate { get; set; }
    [ExcelField("创建时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? CreateDate { get; set; }

    [Core.Utils.ExcelField("权限", IsExport = false)]
    public List<string>? Permissions { get; set; }
}

public class UserSaveDto
{
    public string? UserCode { get; set; }
    public string LoginCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string UserType { get; set; } = "employee";
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OrgCode { get; set; }
    public List<string>? RoleCodes { get; set; }
}
