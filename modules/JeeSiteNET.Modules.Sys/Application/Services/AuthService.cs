using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
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
    private readonly ICurrentUser _currentUser;

    public AuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IConfiguration configuration, IFusionCache cache, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
        _cache = cache;
        _currentUser = currentUser;
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

    /// <summary>通过短信/邮件验证码登录。target 为手机号或邮箱。</summary>
    public async Task<ApiResult<LoginResultDto>> LoginByCodeAsync(string target, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code))
            return ApiResult<LoginResultDto>.Fail(400, "请输入手机号/邮箱和验证码");

        var scene = "login";
        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached))
            return ApiResult<LoginResultDto>.Fail(400, "验证码已过期或不存在");
        if (!string.Equals(cached, code, StringComparison.Ordinal))
            return ApiResult<LoginResultDto>.Fail(400, "验证码错误");
        await _cache.RemoveAsync(codeKey);

        Domain.Entities.User? user = null;
        if (target.Contains('@'))
            user = await _userRepository.GetByEmailAsync(target);
        else
            user = await _userRepository.GetByPhoneAsync(target);

        if (user == null)
            return ApiResult<LoginResultDto>.Fail(400, "该手机号/邮箱未注册账号");
        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

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

    /// <summary>获取当前登录用户的完整认证信息（用户信息+权限+角色+系统编码）</summary>
    public async Task<ApiResult<object>> GetAuthInfoAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return ApiResult<object>.Fail(401, "未登录");

        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<object>.NotFound("用户不存在");

        List<string> permissions;
        if (_currentUser.IsSuperAdmin)
            permissions = new List<string> { "*" };
        else
            permissions = new List<string>(_currentUser.Permissions);

        var sysCodes = await _menuRepository.Query()
            .Where(m => m.Status == "0" && !string.IsNullOrEmpty(m.SysCode))
            .Select(m => m.SysCode!)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        return ApiResult<object>.Ok(new
        {
            userCode = user.UserCode,
            loginCode = user.LoginCode,
            userName = user.UserName,
            userType = user.UserType,
            avatar = user.Avatar,
            email = user.Email,
            phone = user.Phone,
            orgCode = user.OrgCode,
            orgName = user.OrgName,
            status = user.Status,
            loginDate = user.LoginDate,
            createDate = user.CreateDate,
            permissions = permissions,
            roleCodes = _currentUser.RoleCodes,
            sysCodes = sysCodes
        });
    }

    /// <summary>获取当前用户的菜单路由树（支持前端动态路由）</summary>
    public async Task<ApiResult<List<MenuDto>>> GetMenuRouteAsync(string? sysCode = null)
    {
        if (!_currentUser.IsAuthenticated)
            return ApiResult<List<MenuDto>>.Fail(401, "未登录");

        var query = _menuRepository.Query()
            .Where(m => m.Status == "0" && m.IsShow != "0");

        if (!string.IsNullOrEmpty(sysCode))
            query = query.Where(m => m.SysCode == sysCode);

        var all = await query.OrderBy(m => m.TreeSort).ToListAsync();

        List<Menu> filtered;
        if (_currentUser.IsSuperAdmin || _currentUser.Permissions.Count == 0)
            filtered = all;
        else
        {
            var perms = new HashSet<string>(_currentUser.Permissions);
            filtered = all.Where(m =>
                string.IsNullOrEmpty(m.Permission) || perms.Contains(m.Permission)).ToList();
        }

        var tree = BuildMenuTree(filtered, "0");
        return ApiResult<List<MenuDto>>.Ok(tree);
    }

    private static List<MenuDto> BuildMenuTree(List<Menu> list, string parentCode)
    {
        var children = list.Where(m => m.ParentCode == parentCode).OrderBy(m => m.TreeSort).ToList();
        if (children.Count == 0) return new List<MenuDto>();
        var result = new List<MenuDto>();
        foreach (var child in children)
        {
            var dto = new MenuDto
            {
                MenuCode = child.MenuCode,
                ParentCode = child.ParentCode,
                MenuName = child.MenuName,
                MenuHref = child.MenuHref,
                MenuTarget = child.MenuTarget,
                MenuIcon = child.MenuIcon,
                Permission = child.Permission,
                Weight = child.Weight,
                IsShow = child.IsShow,
                ModuleCode = child.ModuleCode,
                SysCode = child.SysCode,
                TreeSort = child.TreeSort,
                Children = BuildMenuTree(list, child.MenuCode)
            };
            result.Add(dto);
        }
        return result;
    }
}
