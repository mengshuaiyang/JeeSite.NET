using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ZiggyCreatures.Caching.Fusion;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class LdapAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IConfiguration _configuration;

    public LdapAuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
    }

    public async Task<ApiResult<LoginResultDto>> LdapLoginAsync(LoginDto dto)
    {
        var ldapSection = _configuration.GetSection("Ldap");
        var ldapUrl = ldapSection["Url"] ?? "";
        var bindDn = ldapSection["BindDn"] ?? "";
        var bindPassword = ldapSection["BindPassword"] ?? "";
        var searchBase = ldapSection["SearchBase"] ?? "";
        var filterTemplate = ldapSection["Filter"] ?? "(uid={0})";

        if (string.IsNullOrEmpty(ldapUrl))
            return ApiResult<LoginResultDto>.Fail(400, "LDAP 未配置");

        var filter = string.Format(filterTemplate, dto.LoginCode);
        var authenticated = LdapAuthUtil.Authenticate(ldapUrl, bindDn, bindPassword, searchBase, filter, dto.Password);

        if (!authenticated)
            return ApiResult<LoginResultDto>.Fail(400, "LDAP 认证失败");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null)
            return ApiResult<LoginResultDto>.Fail(400, "本地用户不存在");

        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        var token = GenerateToken(user);
        List<string> permissions;
        if (user.UserCode == "admin")
            permissions = ["*"];
        else
        {
            var roleCodes = await _userRoleRepository.GetRoleCodesByUserAsync(user.UserCode);
            permissions = await _menuRepository.GetPermissionsByRoleCodesAsync(roleCodes);
        }

        return ApiResult<LoginResultDto>.Ok(new LoginResultDto
        {
            Token = token,
            Expires = DateTime.UtcNow.AddHours(12),
            User = new UserDto
            {
                UserCode = user.UserCode,
                LoginCode = user.LoginCode,
                UserName = user.UserName,
                UserType = user.UserType,
                Avatar = user.Avatar,
                Email = user.Email,
                Phone = user.Phone,
                OrgCode = user.OrgCode,
                OrgName = user.OrgName,
                Status = user.Status,
                Permissions = permissions
            }
        });
    }

    private string GenerateToken(Domain.Entities.User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"] ?? "JeeSiteNET_Default_SuperSecret_Key_2024!";
        var issuer = jwtSection["Issuer"] ?? "JeeSiteNET";
        var audience = jwtSection["Audience"] ?? "JeeSiteNET.Client";
        var expiryHours = int.Parse(jwtSection["ExpiryHours"] ?? "12");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserCode),
            new Claim(ClaimTypes.Name, user.LoginCode),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim("UserType", user.UserType),
        };

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(expiryHours), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
