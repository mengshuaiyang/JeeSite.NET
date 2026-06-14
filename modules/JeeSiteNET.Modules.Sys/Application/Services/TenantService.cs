using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>租户管理服务，负责多租户的信息维护与查询。</summary>
public class TenantService
{
    private readonly ITenantRepository _tenantRepository;

    /// <summary>依赖注入构造函数。</summary>
    public TenantService(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;

    /// <summary>根据租户编码获取租户信息。</summary>
    /// <param name="tenantCode">租户编码。</param>
    /// <returns>租户 DTO，不存在时返回 null。</returns>
    public async Task<TenantDto?> GetAsync(string tenantCode)
    {
        var entity = await _tenantRepository.GetAsync(tenantCode);
        return entity == null ? null : MapToDto(entity);
    }

    /// <summary>按条件分页查询租户列表。</summary>
    /// <param name="request">分页与过滤条件。</param>
    /// <returns>租户分页结果。</returns>
    public async Task<PageResult<TenantDto>> FindPageAsync(PageRequest<Tenant> request)
    {
        var result = await _tenantRepository.FindPageAsync(request);
        return new PageResult<TenantDto>
        {
            List = result.List.Select(MapToDto).ToList(),
            Total = result.Total,
            PageNo = result.PageNo,
            PageSize = result.PageSize
        };
    }

    /// <summary>新增或更新租户（新增时默认可用）。</summary>
    /// <param name="dto">租户保存信息。</param>
    /// <returns>保存后的租户 DTO。</returns>
    public async Task<ApiResult> SaveAsync(TenantSaveDto dto)
    {
        var now = DateTime.Now;
        Tenant? entity;

        if (!string.IsNullOrEmpty(dto.TenantCode))
        {
            entity = await _tenantRepository.GetAsync(dto.TenantCode);
            if (entity == null) return ApiResult.NotFound("租户不存在");
            entity.TenantName = dto.TenantName;
            entity.ContactName = dto.ContactName;
            entity.ContactPhone = dto.ContactPhone;
            entity.IsAvailable = dto.IsAvailable;
            entity.UpdateDate = now;
            await _tenantRepository.UpdateAsync(entity);
        }
        else
        {
            // 新增租户默认状态为可用（IsAvailable = "1"），避免新建后无法登录
            entity = new Tenant
            {
                TenantCode = IdGenerator.NewId(),
                TenantName = dto.TenantName,
                ContactName = dto.ContactName,
                ContactPhone = dto.ContactPhone,
                IsAvailable = dto.IsAvailable ?? "1",
                CreateDate = now,
                UpdateDate = now
            };
            await _tenantRepository.AddAsync(entity);
        }

        return ApiResult.Ok(MapToDto(entity));
    }

    /// <summary>删除租户。</summary>
    /// <param name="tenantCode">租户编码。</param>
    /// <returns>操作结果。</returns>
    public async Task<ApiResult> DeleteAsync(string tenantCode)
    {
        var entity = await _tenantRepository.GetAsync(tenantCode);
        if (entity == null) return ApiResult.NotFound("租户不存在");
        await _tenantRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    /// <summary>实体到 DTO 的转换映射。</summary>
    private static TenantDto MapToDto(Tenant entity) => new()
    {
        TenantCode = entity.TenantCode,
        TenantName = entity.TenantName,
        ContactName = entity.ContactName,
        ContactPhone = entity.ContactPhone,
        IsAvailable = entity.IsAvailable,
        Status = entity.Status
    };
}
