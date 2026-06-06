using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto?> GetAsync(string roleCode)
    {
        var role = await _roleRepository.GetAsync(roleCode);
        return role == null ? null : MapToDto(role);
    }

    public async Task<PageResult<RoleDto>> FindPageAsync(PageRequest<Role> request)
    {
        var query = _roleRepository.Query()
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.RoleName),
                r => r.RoleName!.Contains(request.Entity!.RoleName!))
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.RoleType),
                r => r.RoleType == request.Entity!.RoleType)
            .WhereIf(!string.IsNullOrEmpty(request.Entity?.Status),
                r => r.Status == request.Entity!.Status)
            .OrderBy(r => r.RoleSort);

        var total = await query.CountAsync();
        var list = await query
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return new PageResult<RoleDto>
        {
            List = list.Select(MapToDto).ToList(),
            Total = total,
            PageNo = request.PageNo,
            PageSize = request.PageSize
        };
    }

    public async Task<List<RoleDto>> FindListAsync()
    {
        var list = await _roleRepository.FindListAsync();
        return list.Select(MapToDto).ToList();
    }

    public async Task<ApiResult> SaveAsync(RoleSaveDto dto)
    {
        var now = DateTime.Now;
        Role? role;

        if (!string.IsNullOrEmpty(dto.RoleCode))
        {
            role = await _roleRepository.GetAsync(dto.RoleCode);
            if (role == null) return ApiResult.NotFound("角色不存在");
            role.RoleName = dto.RoleName;
            role.RoleType = dto.RoleType;
            role.IsSys = dto.IsSys;
            role.UserType = dto.UserType;
            role.RoleSort = dto.Sort;
            role.UpdateDate = now;
            await _roleRepository.UpdateAsync(role);
        }
        else
        {
            role = new Role
            {
                RoleCode = IdGenerator.NewId(),
                RoleName = dto.RoleName,
                RoleType = dto.RoleType,
                IsSys = dto.IsSys ?? "0",
                UserType = dto.UserType,
                RoleSort = dto.Sort,
                CreateDate = now,
                UpdateDate = now
            };
            await _roleRepository.AddAsync(role);
        }

        return ApiResult.Ok(MapToDto(role));
    }

    public async Task<ApiResult> DeleteAsync(string roleCode)
    {
        var role = await _roleRepository.GetAsync(roleCode);
        if (role == null) return ApiResult.NotFound("角色不存在");
        await _roleRepository.DeleteAsync(role);
        return ApiResult.Ok();
    }

    private static RoleDto MapToDto(Role role) => new()
    {
        RoleCode = role.RoleCode,
        RoleName = role.RoleName,
        RoleType = role.RoleType,
        IsSys = role.IsSys,
        UserType = role.UserType,
        Sort = role.RoleSort,
        Status = role.Status,
        CreateDate = role.CreateDate
    };
}
