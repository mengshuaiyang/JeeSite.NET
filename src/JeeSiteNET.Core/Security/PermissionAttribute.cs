using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Security;

/// <summary>
/// 权限校验筛选器属性：标注在控制器类或 Action 上，
/// 在请求进入前校验当前用户是否拥有指定权限标识；未登录返回 401，无权限返回 403
/// 支持前缀匹配规则：用户权限 "xxx" 可访问要求 "xxx:*" 的资源
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    /// <summary>
    /// 要求的权限标识数组（拥有任一即可通过）
    /// </summary>
    public string[] Permissions { get; }

    /// <summary>
    /// 构造函数：传入一个或多个权限标识
    /// </summary>
    /// <param name="permissions">权限标识（如 "sys:user:add"）</param>
    public PermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    /// <summary>
    /// 授权校验入口：从 DI 获取当前用户信息，按规则判定是否可继续执行
    /// </summary>
    /// <param name="context">授权筛选上下文</param>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 从请求服务容器获取当前用户上下文实例
        var user = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();

        // 未登录：直接返回 401
        if (!user.IsAuthenticated)
        {
            context.Result = new JsonResult(ApiResult.Unauthorized()) { StatusCode = 401 };
            return;
        }

        // 超级管理员：跳过权限校验
        if (user.IsSuperAdmin)
            return;

        // 普通用户：判断用户权限列表是否包含要求的权限，或用户权限为要求权限的前缀（"xxx:"）
        // 即 user.Permissions 中的 up 与要求的 p 完全相等，或 p 以 "up:" 开头表示通配子项
        if (Permissions.Length > 0 && !Permissions.Any(p => user.Permissions.Any(up => up == p || p.StartsWith(up + ":"))))
        {
            context.Result = new JsonResult(ApiResult.Forbidden()) { StatusCode = 403 };
        }
    }
}
