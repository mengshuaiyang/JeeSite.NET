using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class PermissionAttribute : Attribute, IAuthorizationFilter
{
    public string[] Permissions { get; }

    public PermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.RequestServices.GetRequiredService<ICurrentUser>();
        if (!user.IsAuthenticated)
        {
            context.Result = new JsonResult(ApiResult.Unauthorized()) { StatusCode = 401 };
            return;
        }

        if (user.IsSuperAdmin)
            return;

        if (Permissions.Length > 0 && !Permissions.Any(p => user.Permissions.Any(up => up == p || p.StartsWith(up + ":"))))
        {
            context.Result = new JsonResult(ApiResult.Forbidden()) { StatusCode = 403 };
        }
    }
}
