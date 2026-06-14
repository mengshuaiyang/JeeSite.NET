    // 引入 System.IdentityModel.Tokens.Jwt 命名空间
// 引入命名空间：System.IdentityModel.Tokens.Jwt
using System.IdentityModel.Tokens.Jwt;
    // 引入 System.Security.Claims 命名空间
// 引入命名空间：System.Security.Claims
using System.Security.Claims;
    // 引入 System.Text 命名空间
// 引入命名空间：System.Text
using System.Text;
    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.IdentityModel.Tokens 命名空间
// 引入命名空间：Microsoft.IdentityModel.Tokens
using Microsoft.IdentityModel.Tokens;
    // 引入 ZiggyCreatures.Caching.Fusion 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion
using ZiggyCreatures.Caching.Fusion;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class LdapAuthService
// 定义类：LdapAuthService
public class LdapAuthService
{
    // 字段 _userRepository
    // 字段：_userRepository
    private readonly IUserRepository _userRepository;
    // 字段 _userRoleRepository
    // 字段：_userRoleRepository
    private readonly IUserRoleRepository _userRoleRepository;
    // 字段 _menuRepository
    // 字段：_menuRepository
    private readonly IMenuRepository _menuRepository;
    // 字段 _configuration
    // 字段：_configuration
    private readonly IConfiguration _configuration;

    // 方法 LdapAuthService
    // 构造函数：LdapAuthService
    public LdapAuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
    }

    // 方法 LdapLoginAsync
    // 方法：LdapLoginAsync
    public async Task<ApiResult<LoginResultDto>> LdapLoginAsync(LoginDto dto)
    {
        // 声明并初始化变量：ldapSection
        var ldapSection = _configuration.GetSection("Ldap");
        // 声明并初始化变量：ldapUrl
        var ldapUrl = ldapSection["Url"] ?? "";
        // 声明并初始化变量：bindDn
        var bindDn = ldapSection["BindDn"] ?? "";
        // 声明并初始化变量：bindPassword
        var bindPassword = ldapSection["BindPassword"] ?? "";
        // 声明并初始化变量：searchBase
        var searchBase = ldapSection["SearchBase"] ?? "";
        // 声明并初始化变量：filterTemplate
        var filterTemplate = ldapSection["Filter"] ?? "(uid={0})";

        // if 条件判断
        if (string.IsNullOrEmpty(ldapUrl))
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "LDAP 未配置");

        // 声明并初始化变量：filter
        var filter = string.Format(filterTemplate, dto.LoginCode);
        // 声明并初始化变量：authenticated
        var authenticated = LdapAuthUtil.Authenticate(ldapUrl, bindDn, bindPassword, searchBase, filter, dto.Password);

        // if 条件判断
        if (!authenticated)
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "LDAP 认证失败");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        // if 条件判断
        if (user == null)
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "本地用户不存在");

        // if 条件判断
        if (user.Status == "1")
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        // 声明并初始化变量：token
        var token = GenerateToken(user);
        List<string> permissions;
        // if 条件判断
        if (user.LoginCode == "admin")
            permissions = ["*"];
        // else 否则分支
        else
        {
            var roleCodes = await _userRoleRepository.GetRoleCodesByUserAsync(user.UserCode);
            permissions = await _menuRepository.GetPermissionsByRoleCodesAsync(roleCodes);
        }

        // return 返回结果
        return ApiResult<LoginResultDto>.Ok(new LoginResultDto
        {
            Token = token,
            Expires = DateTime.UtcNow.AddHours(12),
            // 创建 UserDto实例并赋给 User
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

    // 方法 GenerateToken
    // 方法：GenerateToken
    private string GenerateToken(Domain.Entities.User user)
    {
        // 声明并初始化变量：jwtSection
        var jwtSection = _configuration.GetSection("Jwt");
        // 声明并初始化变量：secret
        var secret = jwtSection["Secret"] ?? "JeeSiteNET_Default_SuperSecret_Key_2024!";
        // 声明并初始化变量：issuer
        var issuer = jwtSection["Issuer"] ?? "JeeSiteNET";
        // 声明并初始化变量：audience
        var audience = jwtSection["Audience"] ?? "JeeSiteNET.Client";
        // 调用 Parse
        var expiryHours = int.Parse(jwtSection["ExpiryHours"] ?? "12");

        // 创建 SymmetricSecurityKey实例并赋给 key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        // 创建 SigningCredentials实例并赋给 credentials
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserCode),
            new Claim(ClaimTypes.Name, user.LoginCode),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim("UserType", user.UserType),
        };

        // 创建 JwtSecurityToken实例并赋给 token
        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(expiryHours), signingCredentials: credentials);
        // return 返回结果
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
