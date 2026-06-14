namespace JeeSiteNET.Core.Security;

/// <summary>
/// 当前用户上下文接口：提供当前登录用户的身份、组织、角色与权限信息
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// 用户编码（唯一标识）
    /// </summary>
    string UserCode { get; }

    /// <summary>
    /// 用户登录名/显示名
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// 用户类型（如 "employee" / "member" 等业务自定义）
    /// </summary>
    string? UserType { get; }

    /// <summary>
    /// 所属机构/部门编码
    /// </summary>
    string? OrgCode { get; }

    /// <summary>
    /// 所属机构/部门名称
    /// </summary>
    string? OrgName { get; }

    /// <summary>
    /// 拥有的角色编码列表
    /// </summary>
    List<string> RoleCodes { get; }

    /// <summary>
    /// 拥有的权限标识列表（与 PermissionAttribute 中的 Permissions 对比）
    /// </summary>
    List<string> Permissions { get; }

    /// <summary>
    /// 是否超级管理员（跳过权限与数据范围校验）
    /// </summary>
    bool IsSuperAdmin { get; }

    /// <summary>
    /// 是否已认证（登录有效）
    /// </summary>
    bool IsAuthenticated { get; }
}
