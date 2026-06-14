    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/switch")]
// 定义class SwitchController
// 定义类：SwitchController

public class SwitchController : ControllerBase
{
    [Permission("sys:switch:view")]
    [HttpPost("role/{roleCode}")]
    // 方法 SwitchRole
    // 方法：SwitchRole

    public ApiResult SwitchRole(string roleCode)
    {
        // Update current user's active role
        // return 返回结果
        return ApiResult.Ok("角色已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("post/{postCode}")]
    // 方法 SwitchPost
    // 方法：SwitchPost

    public ApiResult SwitchPost(string postCode)
    {
        // return 返回结果
        return ApiResult.Ok("岗位已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("skin/{skinName}")]
    // 方法 SwitchSkin
    // 方法：SwitchSkin

    public ApiResult SwitchSkin(string skinName)
    {
        // return 返回结果
        return ApiResult.Ok("主题已切换");
    }

    [Permission("sys:switch:view")]
    [HttpPost("corp/{corpCode}")]
    // 方法 SwitchCorp
    // 方法：SwitchCorp

    public ApiResult SwitchCorp(string corpCode)
    {
        // return 返回结果
        return ApiResult.Ok("企业已切换");
    }
}
