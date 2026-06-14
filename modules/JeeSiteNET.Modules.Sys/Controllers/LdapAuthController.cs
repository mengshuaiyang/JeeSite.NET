    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.Services
using JeeSiteNET.Modules.Sys.Application.Services;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/auth")]
[AllowAnonymous]
// 定义class LdapAuthController
// 定义类：LdapAuthController

public class LdapAuthController : ControllerBase
{
    // 字段 _ldapAuthService
    // 字段：_ldapAuthService

    private readonly LdapAuthService _ldapAuthService;

    // 构造函数 LdapAuthController
    // 构造函数：LdapAuthController

    public LdapAuthController(LdapAuthService ldapAuthService) => _ldapAuthService = ldapAuthService;

    [HttpPost("ldap-login")]
    // 方法 LdapLogin
    // 方法：LdapLogin

    public async Task<ApiResult<LoginResultDto>> LdapLogin([FromBody] LoginDto dto)
        => await _ldapAuthService.LdapLoginAsync(dto);
}
