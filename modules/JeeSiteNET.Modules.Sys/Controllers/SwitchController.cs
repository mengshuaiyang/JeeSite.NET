using JeeSiteNET.Core;
using Microsoft.AspNetCore.Mvc;
using JeeSiteNET.Core.Security;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/switch")]
public class SwitchController : ControllerBase
{
    [Permission("sys:switch:view")]
    [HttpPost("role/{roleCode}")]
    public ApiResult SwitchRole(string roleCode)
    {
        // Update current user's active role
        return ApiResult.Ok("角色已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("post/{postCode}")]
    public ApiResult SwitchPost(string postCode)
    {
        return ApiResult.Ok("岗位已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("skin/{skinName}")]
    public ApiResult SwitchSkin(string skinName)
    {
        return ApiResult.Ok("主题已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("corp/{corpCode}")]
    public ApiResult SwitchCorp(string corpCode)
    {
        return ApiResult.Ok("企业已切换");
    }
}
