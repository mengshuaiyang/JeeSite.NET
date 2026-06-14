using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>角色管理服务，负责角色的增删改查及列表查询。</summary>
public class RoleService
{
    private readonly IRoleRepository _roleRepository;

    /// <summary>依赖注入构造函数。</summary>
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    /// <summary>根据角色编码获取角色信息。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <returns>角色 DTO，不存在时返回 null。</returns>
    public async Task<RoleDto?> GetAsync(string roleCode)
    {
        var role = await _roleRepository.GetAsync(roleCode);
        return role == null ? null : MapToDto(role);
    }

    /// <summary>按条件分页查询角色列表，按 RoleSort 升序排序。</summary>
    /// <param name="request">分页及过滤条件（角色名、类型、状态）。</param>
    /// <returns>角色分页结果。</returns>
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

    /// <summary>获取全部可用角色列表，用于用户授权下拉。</summary>
    /// <returns>角色 DTO 列表。</returns>
    public async Task<List<RoleDto>> FindListAsync()
    {
        var list = await _roleRepository.FindListAsync();
        return list.Select(MapToDto).ToList();
    }

    /// <summary>新增或更新角色：有 RoleCode 时为更新；无 RoleCode 时为新增（IsSys 未传默认普通角色）。</summary>
    /// <param name="dto">角色保存信息。</param>
    /// <returns>保存后的角色 DTO。</returns>
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
            // 新增角色默认非系统角色（IsSys = "0"），避免误将普通角色标记为系统内置
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

    /// <summary>删除角色。</summary>
    /// <param name="roleCode">角色编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string roleCode)
    {
        var role = await _roleRepository.GetAsync(roleCode);
        if (role == null) return ApiResult.NotFound("角色不存在");
        await _roleRepository.DeleteAsync(role);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
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
