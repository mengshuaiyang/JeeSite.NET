using JeeSiteNET.Core;
using JeeSiteNET.Core.Utils;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class TenantService
{
    private readonly ITenantRepository _tenantRepository;

    public TenantService(ITenantRepository tenantRepository) => _tenantRepository = tenantRepository;

    public async Task<TenantDto?> GetAsync(string tenantCode)
    {
        var entity = await _tenantRepository.GetAsync(tenantCode);
        return entity == null ? null : MapToDto(entity);
    }

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

    public async Task<ApiResult> DeleteAsync(string tenantCode)
    {
        var entity = await _tenantRepository.GetAsync(tenantCode);
        if (entity == null) return ApiResult.NotFound("租户不存在");
        await _tenantRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

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

public class TenantDto
{
    public string TenantCode { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? IsAvailable { get; set; }
    public string? Status { get; set; }
}

public class TenantSaveDto
{
    public string? TenantCode { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }
    public string? IsAvailable { get; set; }
}
