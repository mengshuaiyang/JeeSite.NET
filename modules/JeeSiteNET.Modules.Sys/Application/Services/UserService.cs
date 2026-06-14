using JeeSiteNET.Core;
using JeeSiteNET.Core.Security;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>用户管理服务，负责用户信息的增删改查、角色绑定以及权限收集。</summary>
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IDataScopeService _dataScopeService;

    /// <summary>依赖注入构造函数。</summary>
    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMenuRepository menuRepository,
        IUserRoleRepository userRoleRepository,
        IDataScopeService dataScopeService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _userRoleRepository = userRoleRepository;
        _dataScopeService = dataScopeService;
    }

    /// <summary>根据用户编码获取用户信息。</summary>
    /// <param name="userCode">用户编码。</param>
    /// <returns>用户 DTO，不存在时返回 null。</returns>
    public async Task<UserDto?> GetAsync(string userCode)
    {
        var user = await _userRepository.GetAsync(userCode);
        return user == null ? null : MapToDto(user);
    }

    /// <summary>根据登录名获取用户信息。</summary>
    /// <param name="loginCode">登录名。</param>
    /// <returns>用户 DTO，不存在时返回 null。</returns>
    public async Task<UserDto?> GetByLoginCodeAsync(string loginCode)
    {
        var user = await _userRepository.GetByLoginCodeAsync(loginCode);
        return user == null ? null : MapToDto(user);
    }

    /// <summary>按条件分页查询用户列表，同时应用数据权限过滤。</summary>
    /// <param name="request">分页及过滤条件（用户名、用户类型、状态）。</param>
    /// <returns>分页结果。</returns>
    public async Task<PageResult<UserDto>> FindPageAsync(PageRequest<User> request)
    {
        // ApplyDataScope 会根据当前用户角色自动追加 OrgCode 等范围过滤条件
        var query = _dataScopeService.ApplyDataScope(_userRepository.Query(), "User")
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.UserName),
                u => u.UserName!.Contains(request.Entity!.UserName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.UserType),
                u => u.UserType == request.Entity!.UserType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status),
                u => u.Status == request.Entity!.Status)
            .OrderByDescending(u => u.CreateDate);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResult<UserDto>
        {
            List = list.Select(MapToDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    /// <summary>新增或更新用户：有 UserCode 时为更新；无 UserCode 时为新增（默认密码 123456）。</summary>
    /// <param name="dto">用户信息与待绑定角色编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> SaveAsync(UserSaveDto dto)
    {
        var now = DateTime.Now;
        User? user;

        if (!string.IsNullOrEmpty(dto.UserCode))
        {
            user = await _userRepository.GetAsync(dto.UserCode);
            if (user == null) return ApiResult.NotFound("用户不存在");
            user.LoginCode = dto.LoginCode;
            user.UserName = dto.UserName;
            user.UserType = dto.UserType;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.OrgCode = dto.OrgCode;
            user.UpdateDate = now;
            await _userRepository.UpdateAsync(user);
        }
        else
        {
            // 新增用户时使用默认密码 123456（MD5），首次登录后应强制修改
            user = new User
            {
                UserCode = IdGenerator.NewId(),
                LoginCode = dto.LoginCode,
                UserName = dto.UserName,
                Password = EncryptUtil.Md5("123456"),
                UserType = dto.UserType,
                Email = dto.Email,
                Phone = dto.Phone,
                OrgCode = dto.OrgCode,
                CreateDate = now,
                UpdateDate = now
            };
            await _userRepository.AddAsync(user);
        }

        // 若调用方未传入 RoleCodes 则跳过，保留原绑定关系
        if (dto.RoleCodes != null)
        {
            await _userRoleRepository.SaveUserRolesAsync(user.UserCode, dto.RoleCodes);
        }

        return ApiResult.Ok();
    }

    /// <summary>删除用户：先清理用户-角色关系，再删除用户记录。</summary>
    /// <param name="userCode">用户编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string userCode)
    {
        var user = await _userRepository.GetAsync(userCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        await _userRoleRepository.DeleteByUserAsync(userCode);
        await _userRepository.DeleteAsync(user);
        return ApiResult.Ok();
    }

    /// <summary>获取用户所持有的全部权限编码（通过角色-菜单关系聚合）。</summary>
    /// <param name="userCode">用户编码。</param>
    /// <returns>权限编码列表。</returns>
    public async Task<List<string>> GetPermissionsAsync(string userCode)
    {
        var roleCodes = await _userRoleRepository.GetRoleCodesByUserAsync(userCode);
        return await _menuRepository.GetPermissionsByRoleCodesAsync(roleCodes);
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    public static UserDto MapToDto(User user) => new()
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
        CreateDate = user.CreateDate
    };
}
