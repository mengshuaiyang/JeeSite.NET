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
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.Logging 命名空间
// 引入命名空间：Microsoft.Extensions.Logging
using Microsoft.Extensions.Logging;
    // 引入 Microsoft.IdentityModel.Tokens 命名空间
// 引入命名空间：Microsoft.IdentityModel.Tokens
using Microsoft.IdentityModel.Tokens;

// 定义 JeeSiteNET.Modules.Sys.Application.Services.OAuth2 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services.OAuth2
namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

// 定义class OAuth2Service
// 定义类：OAuth2Service
public class OAuth2Service
{
    // 字段 _providers
    // 字段：_providers
    private readonly IEnumerable<IOAuth2Provider> _providers;
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
    // 字段 _logger
    // 字段：_logger
    private readonly ILogger<OAuth2Service> _logger;

    // 构造函数 OAuth2Service
    // 构造函数：OAuth2Service
    public OAuth2Service(
        IEnumerable<IOAuth2Provider> providers,
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IMenuRepository menuRepository,
        IConfiguration configuration,
        ILogger<OAuth2Service> logger)
    {
        _providers = providers;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
        _logger = logger;
    }

    // 方法 GetProvider
    // 方法：GetProvider
    public IOAuth2Provider? GetProvider(string provider)
    {
        // return 返回结果
        return _providers.FirstOrDefault(p =>
            p.Provider.Equals(provider, StringComparison.OrdinalIgnoreCase));
    }

    // 方法 LoginAsync
    // 方法：LoginAsync
    public async Task<ApiResult<LoginResultDto>> LoginAsync(string provider, string code, string redirectUri)
    {
        // 声明并初始化变量：oauthProvider
        var oauthProvider = GetProvider(provider);
        // if 条件判断
        if (oauthProvider == null)
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(400, $"不支持的 OAuth2 提供商: {provider}");

        var userInfo = await oauthProvider.HandleCallbackAsync(code, redirectUri);
        // if 条件判断
        if (userInfo == null)
            // return 返回结果
            return ApiResult<LoginResultDto>.Fail(401, "OAuth2 登录失败");

        // 声明并初始化变量：refCode
        var refCode = $"{provider}:{userInfo.ProviderUserId}";
        var user = await _userRepository.GetByLoginCodeAsync(
            // null 合并操作 ??（若为 null 则使用右侧值）
            userInfo.LoginCode ?? userInfo.ProviderUserId);

        // if 条件判断
        if (user == null)
        {
            // 创建 User实例并赋给 user
            user = new User
            {
                UserCode = IdGenerator.NewId(),
                // null 合并操作 ??（若为 null 则使用右侧值）
                LoginCode = userInfo.LoginCode ?? $"{provider}_{userInfo.ProviderUserId[..8]}",
                // null 合并操作 ??（若为 null 则使用右侧值）
                UserName = userInfo.UserName ?? userInfo.LoginCode ?? provider,
                Password = EncryptUtil.Md5(IdGenerator.NewId()),
                UserType = "employee",
                Email = userInfo.Email,
                Avatar = userInfo.Avatar,
                RefCode = refCode,
                RefName = userInfo.UserName,
                Status = "0",
            };
            // await 异步等待
            await _userRepository.AddAsync(user);
            // 日志：记录信息
            _logger.LogInformation("Auto-created user {LoginCode} from {Provider}", user.LoginCode, provider);
        }
        // else 否则分支
        else
        {
            // null 合并操作 ??（若为 null 则使用右侧值）
            user.RefCode ??= refCode;
            // null 合并操作 ??（若为 null 则使用右侧值）
            user.Avatar ??= userInfo.Avatar;
            // await 异步等待
            await _userRepository.UpdateAsync(user);
        }

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
                Permissions = permissions,
            }
        });
    }

    // 方法 GenerateToken
    // 方法：GenerateToken
    private string GenerateToken(User user)
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
        var token = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials);

        // return 返回结果
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
