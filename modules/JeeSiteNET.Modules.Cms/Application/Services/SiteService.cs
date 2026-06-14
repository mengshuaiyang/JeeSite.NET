    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Cms.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.DTOs
using JeeSiteNET.Modules.Cms.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Entities
using JeeSiteNET.Modules.Cms.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.Services
namespace JeeSiteNET.Modules.Cms.Application.Services;

// 定义class SiteService
// 定义类：SiteService
public class SiteService
{
    // 字段 _siteRepository
    // 字段：_siteRepository
    private readonly ISiteRepository _siteRepository;
    // 构造函数 SiteService
    // 构造函数：SiteService
    public SiteService(ISiteRepository siteRepository) => _siteRepository = siteRepository;

    // 方法 GetAllAsync
    // 方法：GetAllAsync
    public async Task<List<SiteDto>> GetAllAsync()
    {
        // 数据库操作：条件过滤
        var list = await _siteRepository.Query().Where(s => s.Status == "0").ToListAsync();
        // return 返回结果
        return list.Select(SiteDto.FromEntity).ToList();
    }

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<SiteDto?> GetAsync(string siteCode)
    {
        // 缓存：获取值
        var entity = await _siteRepository.GetAsync(siteCode);
        // return 返回结果
        return entity == null ? null : SiteDto.FromEntity(entity);
    }

    // 方法 FindPageAsync
    // 方法：FindPageAsync
    public async Task<PageResult<SiteDto>> FindPageAsync(PageRequest<Site> request)
    {
        // 数据库操作：升序排序
        var query = _siteRepository.Query().OrderBy(e => e.SiteCode);
        // 数据库操作：异步统计数量
        var total = await query.CountAsync();
        // 数据库操作：异步查询为列表
        var list = await query.Skip((request.PageNo - 1) * request.PageSize).Take(request.PageSize).ToListAsync();
        // return 返回结果
        return new PageResult<SiteDto> { List = list.Select(SiteDto.FromEntity).ToList(), Total = total, PageNo = request.PageNo, PageSize = request.PageSize };
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(SiteSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        Site? entity;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.SiteCode))
        {
            // 缓存：获取值
            entity = await _siteRepository.GetAsync(dto.SiteCode);
            // if 条件判断
            if (entity == null) return ApiResult.NotFound("站点不存在");
            entity.SiteName = dto.SiteName; entity.Domain = dto.Domain; entity.Logo = dto.Logo;
            entity.Keywords = dto.Keywords; entity.Description = dto.Description; entity.UpdateDate = now;
            // await 异步等待
            await _siteRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 创建 Site实例并赋给 entity
            entity = new Site { SiteCode = dto.SiteName, SiteName = dto.SiteName, Domain = dto.Domain,
                Logo = dto.Logo, Keywords = dto.Keywords, Description = dto.Description,
                CreateDate = now, UpdateDate = now };
            // await 异步等待
            await _siteRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(SiteDto.FromEntity(entity));
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task<ApiResult> DeleteAsync(string siteCode)
    {
        // 缓存：获取值
        var entity = await _siteRepository.GetAsync(siteCode);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("站点不存在");
        // await 异步等待
        await _siteRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }
}
