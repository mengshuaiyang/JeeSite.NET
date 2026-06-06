using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRoleRepository _userRoleRepository;

    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IMenuRepository menuRepository,
        IUserRoleRepository userRoleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _menuRepository = menuRepository;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<UserDto?> GetAsync(string userCode)
    {
        var user = await _userRepository.GetAsync(userCode);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetByLoginCodeAsync(string loginCode)
    {
        var user = await _userRepository.GetByLoginCodeAsync(loginCode);
        return user == null ? null : MapToDto(user);
    }

    public async Task<PageResult<UserDto>> FindPageAsync(PageRequest<User> request)
    {
        var query = _userRepository.Query()
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

        if (dto.RoleCodes != null)
        {
            await _userRoleRepository.SaveUserRolesAsync(user.UserCode, dto.RoleCodes);
        }

        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteAsync(string userCode)
    {
        var user = await _userRepository.GetAsync(userCode);
        if (user == null) return ApiResult.NotFound("用户不存在");
        await _userRoleRepository.DeleteByUserAsync(userCode);
        await _userRepository.DeleteAsync(user);
        return ApiResult.Ok();
    }

    public async Task<List<string>> GetPermissionsAsync(string userCode)
    {
        var roleCodes = await _userRoleRepository.GetRoleCodesByUserAsync(userCode);
        return await _menuRepository.GetPermissionsByRoleCodesAsync(roleCodes);
    }

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
