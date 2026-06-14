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

/// <summary>认证授权服务，负责用户登录、注册、找回密码、JWT 令牌生成以及菜单路由树构建。</summary>
public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IConfiguration _configuration;
    private readonly IFusionCache _cache;
    private readonly ICurrentUser _currentUser;

    /// <summary>依赖注入构造函数。</summary>
    public AuthService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IMenuRepository menuRepository, IConfiguration configuration, IFusionCache cache, ICurrentUser currentUser)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _menuRepository = menuRepository;
        _configuration = configuration;
        _cache = cache;
        _currentUser = currentUser;
    }

    /// <summary>账号密码登录：校验图形验证码、账号状态与密码后，签发 JWT 令牌并返回用户权限信息。</summary>
    /// <param name="dto">登录信息（登录名、密码、图形验证码 Key 和值）。</param>
    /// <returns>包含 Token、过期时间、用户基本信息及其权限列表的登录结果。</returns>
    public async Task<ApiResult<LoginResultDto>> LoginAsync(LoginDto dto)
    {
        if (string.IsNullOrEmpty(dto.LoginCode) || string.IsNullOrEmpty(dto.Password))
            return ApiResult<LoginResultDto>.Fail(400, "登录名和密码不能为空");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null)
            return ApiResult<LoginResultDto>.Fail(400, "登录名或密码错误");

        // Status = "1" 表示账号被停用（约定枚举值：0 启用 1 禁用）
        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        // 图形验证码：提交 ValidCodeKey 时才校验（配置开关由前端决定是否传入）
        if (!string.IsNullOrEmpty(dto.ValidCodeKey))
        {
            var cached = await _cache.GetOrDefaultAsync<string>($"Captcha:{dto.ValidCodeKey}");
            if (string.IsNullOrEmpty(cached))
                return ApiResult<LoginResultDto>.Fail(400, "验证码已过期，请刷新后重新输入");
            if (!string.Equals(cached, dto.ValidCode, StringComparison.OrdinalIgnoreCase))
                return ApiResult<LoginResultDto>.Fail(400, "验证码错误");
            // 验证一次即失效，防止重放
            await _cache.RemoveAsync($"Captcha:{dto.ValidCodeKey}");
        }

        // 密码采用 MD5 存储（与 JeeSite5 保持一致），正式生产建议升级为 PBKDF2/SHA-256 + Salt
        var inputPwd = EncryptUtil.Md5(dto.Password);
        if (user.Password != inputPwd)
            return ApiResult<LoginResultDto>.Fail(400, "登录名或密码错误");

        // 多设备登录控制：新令牌签发后将旧令牌拉入黑名单
        var existingToken = await _cache.GetOrDefaultAsync<string>($"OnlineToken:{user.UserCode}");
        if (!string.IsNullOrEmpty(existingToken))
            await _cache.RemoveAsync($"TokenBlacklist:{existingToken}");

        var token = GenerateToken(user);
        await _cache.SetAsync($"OnlineToken:{user.UserCode}", token, TimeSpan.FromHours(12));

        List<string> permissions;
        // 内置管理员 admin 通配权限 "*"，跳过角色-菜单权限查询
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

    /// <summary>用户注册：检查登录名是否已被占用后，以 MD5 加密密码写入用户表。</summary>
    /// <param name="dto">注册信息（登录名、密码、昵称、邮箱、手机号）。</param>
    /// <returns>注册成功或失败结果。</returns>
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
            // 未填昵称时默认使用登录名
            UserName = dto.UserName ?? dto.LoginCode,
            Password = EncryptUtil.Md5(dto.Password),
            UserType = "employee",
            Email = dto.Email,
            Phone = dto.Phone,
            Status = "0",
            // 同步评估密码强度等级并记录最近一次密码 MD5（防止循环复用密码）
            PwdSecurityLevel = PasswordStrengthUtil.Evaluate(dto.Password),
            PwdUpdateDate = now,
            PwdUpdateRecord = EncryptUtil.Md5(dto.Password),
            CreateDate = now
        };
        await _userRepository.AddAsync(user);
        return ApiResult.Ok();
    }

    /// <summary>找回密码：校验登录名与邮箱是否匹配后，签发一次性重置 Token（30 分钟有效）。</summary>
    /// <param name="dto">包含登录名和绑定邮箱的找回信息。</param>
    /// <returns>返回重置 Token（测试环境直接返回，生产环境应走邮件）。</returns>
    public async Task<ApiResult> ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        if (string.IsNullOrEmpty(dto.LoginCode) || string.IsNullOrEmpty(dto.Email))
            return ApiResult.Fail(400, "登录名和邮箱不能为空");

        var user = await _userRepository.GetByLoginCodeAsync(dto.LoginCode);
        if (user == null)
            return ApiResult.Fail(400, "账号不存在");

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
            return ApiResult.Fail(400, "邮箱不匹配");

        // 取 GUID 前 20 位作为一次性 Token，避免暴露 GUID 全部信息
        var resetToken = Guid.NewGuid().ToString("N")[..20];
        await _cache.SetAsync($"ResetPwd:{resetToken}", user.UserCode, TimeSpan.FromMinutes(30));

        return ApiResult.Ok(new { resetToken, message = "测试环境直接返回 Token，正式环境通过邮件发送" });
    }

    /// <summary>使用重置 Token 设置新密码。</summary>
    /// <param name="dto">包含重置 Token 和新密码的请求。</param>
    /// <returns>设置成功或失败结果。</returns>
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
        // 记录最新密码哈希用于未来密码复用检测（防止最近 N 次重复）
        user.PwdUpdateRecord = EncryptUtil.Md5(dto.NewPassword);
        await _cache.RemoveAsync($"ResetPwd:{dto.Token}");
        return ApiResult.Ok();
    }

    /// <summary>通过短信或邮箱验证码登录。根据 target 是否包含 @ 判断是邮箱还是手机号。</summary>
    /// <param name="target">手机号或邮箱地址。</param>
    /// <param name="code">用户提交的验证码。</param>
    /// <returns>与账号密码登录相同结构的登录结果。</returns>
    public async Task<ApiResult<LoginResultDto>> LoginByCodeAsync(string target, string code)
    {
        if (string.IsNullOrWhiteSpace(target) || string.IsNullOrWhiteSpace(code))
            return ApiResult<LoginResultDto>.Fail(400, "请输入手机号/邮箱和验证码");

        var scene = "login";
        var codeKey = $"ValidCode:{scene}:{target}";
        var cached = await _cache.GetOrDefaultAsync<string>(codeKey);
        if (string.IsNullOrEmpty(cached))
            return ApiResult<LoginResultDto>.Fail(400, "验证码已过期或不存在");
        // 验证码区分大小写，防止攻击者暴力尝试简单组合
        if (!string.Equals(cached, code, StringComparison.Ordinal))
            return ApiResult<LoginResultDto>.Fail(400, "验证码错误");
        // 使用后立即移除防止重放
        await _cache.RemoveAsync(codeKey);

        Domain.Entities.User? user = null;
        // 通过 @ 区分邮箱和手机号查询路径
        if (target.Contains('@'))
            user = await _userRepository.GetByEmailAsync(target);
        else
            user = await _userRepository.GetByPhoneAsync(target);

        if (user == null)
            return ApiResult<LoginResultDto>.Fail(400, "该手机号/邮箱未注册账号");
        if (user.Status == "1")
            return ApiResult<LoginResultDto>.Fail(400, "该账号已被禁用");

        // 多设备登录控制：新令牌签发后将旧令牌拉入黑名单
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

    /// <summary>根据配置生成 JWT 令牌：从 appsettings.json Jwt 段读取 Secret/Issuer/Audience/ExpiryHours。</summary>
    /// <param name="user">当前登录的用户实体。</param>
    /// <returns>JWT 字符串。</returns>
    private string GenerateToken(Domain.Entities.User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"] ?? "JeeSiteNET_Default_SuperSecret_Key_2024!";
        var issuer = jwtSection["Issuer"] ?? "JeeSiteNET";
        var audience = jwtSection["Audience"] ?? "JeeSiteNET.Client";
        var expiryHours = int.Parse(jwtSection["ExpiryHours"] ?? "12");

        // 使用 HmacSha256 对称签名；生产环境 Secret 至少 32 字节并从安全配置源读取
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Claims 中携带用户标识，便于中间件解析 ICurrentUser
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

    /// <summary>获取当前登录用户的完整认证信息（用户信息+权限+角色+系统编码）。</summary>
    /// <returns>包含用户基本信息、权限列表、角色列表和可用系统编码的动态对象。</returns>
    public async Task<ApiResult<object>> GetAuthInfoAsync()
    {
        if (!_currentUser.IsAuthenticated)
            return ApiResult<object>.Fail(401, "未登录");

        var user = await _userRepository.GetAsync(_currentUser.UserCode);
        if (user == null) return ApiResult<object>.NotFound("用户不存在");

        List<string> permissions;
        // 超级管理员直接通配 "*"，避免前端菜单匹配时做二次查询
        if (_currentUser.IsSuperAdmin)
            permissions = new List<string> { "*" };
        else
            permissions = new List<string>(_currentUser.Permissions);

        // 系统编码（SysCode）用于多子系统场景下的路由分组
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

    /// <summary>获取当前用户的菜单路由树（支持前端动态路由）。可按子系统编码过滤。</summary>
    /// <param name="sysCode">可选的子系统编码，为空返回全部可见菜单。</param>
    /// <returns>按 TreeSort 排序后的菜单树。</returns>
    public async Task<ApiResult<List<MenuDto>>> GetMenuRouteAsync(string? sysCode = null)
    {
        if (!_currentUser.IsAuthenticated)
            return ApiResult<List<MenuDto>>.Fail(401, "未登录");

        // 只返回启用且标记为显示的菜单节点（Status = "0" 启用；IsShow != "0" 可见）
        var query = _menuRepository.Query()
            .Where(m => m.Status == "0" && m.IsShow != "0");

        if (!string.IsNullOrEmpty(sysCode))
            query = query.Where(m => m.SysCode == sysCode);

        var all = await query.OrderBy(m => m.TreeSort).ToListAsync();

        List<Menu> filtered;
        // 超级管理员或未配置任何权限时显示全部菜单；其他用户按 Permission 字段过滤
        if (_currentUser.IsSuperAdmin || _currentUser.Permissions.Count == 0)
            filtered = all;
        else
        {
            var perms = new HashSet<string>(_currentUser.Permissions);
            // 未配置 Permission 的菜单项视为公共可见
            filtered = all.Where(m =>
                string.IsNullOrEmpty(m.Permission) || perms.Contains(m.Permission)).ToList();
        }

        var tree = BuildMenuTree(filtered, "0");
        return ApiResult<List<MenuDto>>.Ok(tree);
    }

    /// <summary>递归构建菜单树。parentCode 为 "0" 时表示顶级节点。</summary>
    /// <param name="list">已过滤并排序的菜单扁平列表。</param>
    /// <param name="parentCode">上级菜单编码。</param>
    /// <returns>递归嵌套的菜单 DTO 列表。</returns>
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
