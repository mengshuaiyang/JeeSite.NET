using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class CasAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IConfiguration _configuration;
    private readonly IEnumerable<ICasCreateUser> _casCreateUsers;

    public CasAuthService(
        IUserRepository userRepository,
        IUserRoleRepository userRoleRepository,
        IMenuRepository menuRepository,
        IConfiguration configuration,
        IEnumerable<ICasCreateUser> casCreateUsers)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
        _casCreateUsers = casCreateUsers;
    }

    public async Task<ApiResult<LoginResultDto>> CasLoginAsync(string ticket, string serviceUrl)
    {
        var section = _configuration.GetSection("Cas");
        var casServerUrl = section["ServerUrl"] ?? "";
        if (string.IsNullOrEmpty(casServerUrl))
            return ApiResult<LoginResultDto>.Fail(400, "CAS 未配置");

        var result = await CasAuthUtil.ValidateTicketAsync(casServerUrl, ticket, serviceUrl);
        if (result == null || !result.IsSuccess || string.IsNullOrEmpty(result.Username))
        {
            var msg = result?.FailureMessage ?? "CAS 票据验证失败";
            return ApiResult<LoginResultDto>.Fail(401, msg);
        }

        var loginCode = result.Attributes.GetValueOrDefault("loginCode") ?? result.Username;
        var user = await _userRepository.GetByLoginCodeAsync(loginCode);

        if (user == null)
        {
            var isAllowCreate = section["AllowCreateUser"] != "false";
            if (!isAllowCreate)
                return ApiResult<LoginResultDto>.Fail(400, $"用户 [{loginCode}] 在本系统中不存在，请联系管理员");

            var userType = result.Attributes.GetValueOrDefault("userType") ?? "employee";
            user = await CreateUserFromCasAsync(result, loginCode, userType);
            if (user == null)
                return ApiResult<LoginResultDto>.Fail(400, $"用户 [{loginCode}] 自动创建失败");
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
                Permissions = permissions
            }
        });
    }

    private async Task<User?> CreateUserFromCasAsync(CasValidateResult result, string loginCode, string userType)
    {
        var attrs = result.Attributes;

        var userCode = attrs.GetValueOrDefault("userCode") ?? IdGenerator.NewId();
        var userName = attrs.GetValueOrDefault("userName") ?? loginCode;

        var user = new User
        {
            UserCode = userCode,
            LoginCode = loginCode,
            UserName = WebUtility.UrlDecode(userName),
            Password = EncryptUtil.Md5("123456"),
            UserType = userType,
            Email = attrs.GetValueOrDefault("email") != null ? WebUtility.UrlDecode(attrs["email"]) : null,
            Phone = attrs.GetValueOrDefault("phone") ?? attrs.GetValueOrDefault("mobile"),
            RefCode = attrs.GetValueOrDefault("refCode"),
            RefName = attrs.GetValueOrDefault("refName"),
            MgrType = attrs.GetValueOrDefault("mgrType"),
            CorpCode = attrs.GetValueOrDefault("corpCode"),
            CorpName = attrs.GetValueOrDefault("corpName"),
            Status = "0"
        };

        if (userType == "employee")
        {
            await _userRepository.AddAsync(user);
            return user;
        }

        foreach (var createUser in _casCreateUsers)
        {
            var newUserCode = createUser.CreateUser(userType, attrs);
            if (newUserCode != null)
            {
                return await _userRepository.GetByLoginCodeAsync(loginCode);
            }
        }

        return null;
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

        var token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.UtcNow.AddHours(expiryHours), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
