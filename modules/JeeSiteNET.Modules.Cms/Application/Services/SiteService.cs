using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Application.DTOs;
using JeeSiteNET.Modules.Cms.Domain.Entities;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public class SiteService
{
    private readonly ISiteRepository _siteRepository;
    public SiteService(ISiteRepository siteRepository) => _siteRepository = siteRepository;

    public async Task<List<SiteDto>> GetAllAsync()
    {
        var list = await _siteRepository.Query().Where(s => s.Status == "0").ToListAsync();
        return list.Select(SiteDto.FromEntity).ToList();
    }

    public async Task<SiteDto?> GetAsync(string siteCode)
    {
        var entity = await _siteRepository.GetAsync(siteCode);
        return entity == null ? null : SiteDto.FromEntity(entity);
    }

    public async Task<PageResult<SiteDto>> FindPageAsync(PageRequest<Site> request)
    {
        var query = _siteRepository.Query().OrderBy(e => e.SiteCode);
        var total = await query.CountAsync();
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        return new PageResult<SiteDto> { List = list.Select(SiteDto.FromEntity).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    public async Task<ApiResult> SaveAsync(SiteSaveDto dto)
    {
        var now = DateTime.Now;
        Site? entity;
        if (!string.IsNullOrEmpty(dto.SiteCode))
        {
            entity = await _siteRepository.GetAsync(dto.SiteCode);
            if (entity == null) return ApiResult.NotFound("站点不存在");
            entity.SiteName = dto.SiteName; entity.Domain = dto.Domain; entity.Logo = dto.Logo;
            entity.Keywords = dto.Keywords; entity.Description = dto.Description; entity.UpdateDate = now;
            await _siteRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Site { SiteCode = dto.SiteName, SiteName = dto.SiteName, Domain = dto.Domain,
                Logo = dto.Logo, Keywords = dto.Keywords, Description = dto.Description,
                CreateDate = now, UpdateDate = now };
            await _siteRepository.AddAsync(entity);
        }
        return ApiResult.Ok(SiteDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string siteCode)
    {
        var entity = await _siteRepository.GetAsync(siteCode);
        if (entity == null) return ApiResult.NotFound("站点不存在");
        await _siteRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }
}
