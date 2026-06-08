using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JeeSiteNET.Modules.Sys.Controllers;

[ApiController]
[Route("api/v1/sys/auth")]
[AllowAnonymous]
public class LdapAuthController : ControllerBase
{
    private readonly LdapAuthService _ldapAuthService;

    public LdapAuthController(LdapAuthService ldapAuthService) => _ldapAuthService = ldapAuthService;

    [HttpPost("ldap-login")]
    public async Task<ApiResult<LoginResultDto>> LdapLogin([FromBody] LoginDto dto)
        => await _ldapAuthService.LdapLoginAsync(dto);
}
