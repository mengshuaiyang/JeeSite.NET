using JeeSiteNET.Core.Utils;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 用户信息 DTO（用于列表/详情/登录响应）。
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户编码（主键，业务唯一）。
    /// </summary>
    [ExcelField("用户编码")]
    public string UserCode { get; set; } = string.Empty;

    /// <summary>
    /// 登录账号。
    /// </summary>
    [ExcelField("登录名")]
    public string LoginCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户姓名/昵称。
    /// </summary>
    [ExcelField("用户姓名")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型（employee/admin/customer 等）。
    /// </summary>
    [ExcelField("用户类型")]
    public string UserType { get; set; } = "employee";

    /// <summary>
    /// 头像 URL；不导出 Excel。
    /// </summary>
    [ExcelField("头像", IsExport = false)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 邮箱地址。
    /// </summary>
    [ExcelField("邮箱")]
    public string? Email { get; set; }

    /// <summary>
    /// 手机号码。
    /// </summary>
    [ExcelField("手机号")]
    public string? Phone { get; set; }

    /// <summary>
    /// 所属机构编码。
    /// </summary>
    [ExcelField("机构编码")]
    public string? OrgCode { get; set; }

    /// <summary>
    /// 所属机构名称。
    /// </summary>
    [ExcelField("机构名称")]
    public string? OrgName { get; set; }

    /// <summary>
    /// 账号状态（0 正常 / 1 禁用）。
    /// </summary>
    [ExcelField("状态")]
    public string? Status { get; set; }

    /// <summary>
    /// 最后一次登录时间；带格式化列宽。
    /// </summary>
    [ExcelField("最后登录时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? LoginDate { get; set; }

    /// <summary>
    /// 记录创建时间；带格式化列宽。
    /// </summary>
    [ExcelField("创建时间", DataFormat = "yyyy-MM-dd HH:mm:ss", ColumnWidth = 25)]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// 用户持有的权限标识列表；不导出 Excel。
    /// </summary>
    [Core.Utils.ExcelField("权限", IsExport = false)]
    public List<string>? Permissions { get; set; }
}

/// <summary>
/// 用户保存请求 DTO（新建/更新共用）。
/// </summary>
public class UserSaveDto
{
    /// <summary>
    /// 用户编码；为空时表示新建，由服务端自动生成。
    /// </summary>
    public string? UserCode { get; set; }

    /// <summary>
    /// 登录账号。
    /// </summary>
    public string LoginCode { get; set; } = string.Empty;

    /// <summary>
    /// 用户姓名。
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 用户类型。
    /// </summary>
    public string UserType { get; set; } = "employee";

    /// <summary>
    /// 邮箱。
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 手机号。
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 机构编码。
    /// </summary>
    public string? OrgCode { get; set; }

    /// <summary>
    /// 绑定的角色编码列表。
    /// </summary>
    public List<string>? RoleCodes { get; set; }
}
