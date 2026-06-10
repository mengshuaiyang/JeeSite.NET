using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace JeeSiteNET.Modules.Sys.Application.Services.OAuth2;

public class OAuth2Service
{
    private readonly IEnumerable<IOAuth2Provider> _providers;
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OAuth2Service> _logger;

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

    public IOAuth2Provider? GetProvider(string provider)
    {
        return _providers.FirstOrDefault(p =>
            p.Provider.Equals(provider, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<ApiResult<LoginResultDto>> LoginAsync(string provider, string code, string redirectUri)
    {
        var oauthProvider = GetProvider(provider);
        if (oauthProvider == null)
            return ApiResult<LoginResultDto>.Fail(400, $"不支持的 OAuth2 提供商: {provider}");

        var userInfo = await oauthProvider.HandleCallbackAsync(code, redirectUri);
        if (userInfo == null)
            return ApiResult<LoginResultDto>.Fail(401, "OAuth2 登录失败");

        var refCode = $"{provider}:{userInfo.ProviderUserId}";
        var user = await _userRepository.GetByLoginCodeAsync(
            userInfo.LoginCode ?? userInfo.ProviderUserId);

        if (user == null)
        {
            user = new User
            {
                UserCode = IdGenerator.NewId(),
                LoginCode = userInfo.LoginCode ?? $"{provider}_{userInfo.ProviderUserId[..8]}",
                UserName = userInfo.UserName ?? userInfo.LoginCode ?? provider,
                Password = EncryptUtil.Md5(IdGenerator.NewId()),
                UserType = "employee",
                Email = userInfo.Email,
                Avatar = userInfo.Avatar,
                RefCode = refCode,
                RefName = userInfo.UserName,
                Status = "0",
            };
            await _userRepository.AddAsync(user);
            _logger.LogInformation("Auto-created user {LoginCode} from {Provider}", user.LoginCode, provider);
        }
        else
        {
            user.RefCode ??= refCode;
            user.Avatar ??= userInfo.Avatar;
            await _userRepository.UpdateAsync(user);
        }

        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        var token = GenerateToken(user);
        List<string> permissions;
        if (user.LoginCode == "admin")
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
                Permissions = permissions,
            }
        });
    }

    private string GenerateToken(User user)
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

        var token = new JwtSecurityToken(issuer, audience, claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
