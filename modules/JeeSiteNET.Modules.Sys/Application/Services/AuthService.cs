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

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IConfiguration _configuration;
    private readonly IFusionCache _cache;

    public AuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IConfiguration configuration, IFusionCache cache)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
        _cache = cache;
    }

    public async Task<ApiResult<LoginResultDto>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrEmpty(dto.LoginCode) || string.IsNullOrEmpty(dto.Password))
            return ApiResult<LoginResultDto>.Fail(400, "登录名和密码不能为空");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null)
            return ApiResult<LoginResultDto>.Fail(400, "登录名或密码错误");

        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        if (!string.IsNullOrEmpty(dto.ValidCodeKey))
        {
            var cached = await _cache.GetOrDefaultAsync<string>($"Captcha:{dto.ValidCodeKey}");
            if (string.IsNullOrEmpty(cached))
                return ApiResult<LoginResultDto>.Fail(400, "验证码已过期，请刷新后重新输入");
            if (!string.Equals(cached, dto.ValidCode, StringComparison.OrdinalIgnoreCase))
                return ApiResult<LoginResultDto>.Fail(400, "验证码错误");
            await _cache.RemoveAsync($"Captcha:{dto.ValidCodeKey}");
        }

        var inputPwd = EncryptUtil.Md5(dto.Password);
        if (user.Password != inputPwd)
            return ApiResult<LoginResultDto>.Fail(400, "登录名或密码错误");

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
                LoginDate = user.LoginDate,
                CreateDate = user.CreateDate,
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

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
