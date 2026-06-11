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

        // Check multi-device token
        var existingToken = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{user.UserCode}");
        if (!string.IsNullOrEmpty(existingToken))
            await _cache.RemoveAsync($"TokenBlacklist:{existingToken}");

        var token = GenerateToken(user);
        await _cache.SetAsync($"OnlineToken:{user.UserCode}", token, TimeSpan.FromHours(12));

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

    public async Task<ApiResult> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrEmpty(dto.LoginCode) || string.IsNullOrEmpty(dto.Password))
            return ApiResult.Fail(400, "登录名和密码不能为空");

        var existing = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (existing != null)
            return ApiResult.Fail(400, "该登录名已被注册");

        var now = DateTime.Now;
        var user = new Domain.Entities.User
        {
            UserCode = IdGenerator.NewId(),
            LoginCode = dto.LoginCode,
            UserName = dto.UserName ?? dto.LoginCode,
            Password = EncryptUtil.Md5(dto.Password),
            UserType = "employee",
            Email = dto.Email,
            Phone = dto.Phone,
            Status = "0",
            PwdSecurityLevel = PasswordStrengthUtil.Evaluate(dto.Password),
            PwdUpdateDate = now,
            PwdUpdateRecord = EncryptUtil.Md5(dto.Password),
            CreateDate = now
        };
        await _userRepository.AddAsync(user);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        if (string.IsNullOrEmpty(dto.LoginCode) || string.IsNullOrEmpty(dto.Email))
            return ApiResult.Fail(400, "登录名和邮箱不能为空");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null)
            return ApiResult.Fail(400, "账号不存在");

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            return ApiResult.Fail(400, "邮箱不匹配");

        var resetToken = Guid.NewGuid().ToString("N")[..20];
        await _cache.SetAsync($"ResetPwd:{resetToken}", user.UserCode, TimeSpan.FromMinutes(30));

        return ApiResult.Ok(new { resetToken, message = "测试环境直接返回 Token，正式环境通过邮件发送" });
    }

    public async Task<ApiResult> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var userCode = await _cache.GetOrDefaultAsync<string>($"ResetPwd:{dto.Token}");
        if (string.IsNullOrEmpty(userCode))
            return ApiResult.Fail(400, "重置链接已过期");

        var user = await _userRepository.GetAsync(userCode);
        if (user == null)
            return ApiResult.Fail(400, "用户不存在");

        var now = DateTime.Now;
        user.Password = EncryptUtil.Md5(dto.NewPassword);
        user.PwdSecurityLevel = PasswordStrengthUtil.Evaluate(dto.NewPassword);
        user.PwdUpdateDate = now;
        user.PwdUpdateRecord = EncryptUtil.Md5(dto.NewPassword);
        await _cache.RemoveAsync($"ResetPwd:{dto.Token}");
        return ApiResult.Ok();
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
