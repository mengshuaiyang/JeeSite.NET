using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class AreaService
{
    private readonly JeeSiteDbContext _db;

    public AreaService(JeeSiteDbContext db)
    {
        _db = db;
    }

    public async Task<List<AreaDto>> TreeAsync()
    {
        var list = await _db.Set<Area>()
            .OrderBy(a => a.TreeSorts)
            .ToListAsync();
        return BuildTree(list, null);
    }

    private static List<AreaDto> BuildTree(List<Area> all, string? parentCode)
    {
        return all
            .Where(a => a.ParentCode == parentCode)
            .Select(a =>
            {
                var dto = a.ToDto();
                dto.Children = BuildTree(all, a.AreaCode);
                return dto;
            })
            .ToList();
    }

    public async Task<ApiResult> SaveAsync(Area dto)
    {
        var now = DateTime.Now;
        if (string.IsNullOrEmpty(dto.AreaCode))
            return ApiResult.Fail(400, "区域编码不能为空");
        var exist = await _db.Set<Area>().FirstOrDefaultAsync(a => a.AreaCode == dto.AreaCode);
        if (exist == null)
        {
            var area = new Area
            {
                AreaCode = dto.AreaCode,
                AreaName = dto.AreaName,
                AreaType = dto.AreaType,
                ParentCode = dto.ParentCode,
                TreeLeaf = "1",
                Status = dto.Status,
                CreateDate = now,
                UpdateDate = now
            };
            await _db.Set<Area>().AddAsync(area);
        }
        else
        {
            exist.AreaName = dto.AreaName;
            exist.AreaType = dto.AreaType;
            exist.ParentCode = dto.ParentCode;
            exist.UpdateDate = now;
            _db.Set<Area>().Update(exist);
        }
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteAsync(string areaCode)
    {
        var exist = await _db.Set<Area>().FirstOrDefaultAsync(a => a.AreaCode == areaCode);
        if (exist == null) return ApiResult.NotFound("区域不存在");
        var hasChildren = await _db.Set<Area>().AnyAsync(a => a.ParentCode == areaCode);
        if (hasChildren) return ApiResult.Fail(400, "存在下级区域，无法删除");
        _db.Set<Area>().Remove(exist);
        await _db.SaveChangesAsync();
        return ApiResult.Ok();
    }
}
