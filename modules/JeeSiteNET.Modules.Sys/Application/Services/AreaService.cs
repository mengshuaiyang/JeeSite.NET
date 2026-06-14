    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Infrastructure.EntityFrameworkCore 命名空间
// 引入命名空间：JeeSiteNET.Infrastructure.EntityFrameworkCore
using JeeSiteNET.Infrastructure.EntityFrameworkCore;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class AreaService
// 定义类：AreaService
public class AreaService
{
    // 字段 _db
    // 字段：_db
    private readonly JeeSiteDbContext _db;

    // 方法 AreaService
    // 构造函数：AreaService
    public AreaService(JeeSiteDbContext db)
    {
        _db = db;
    }

    // 方法 TreeAsync
    // 方法：TreeAsync
    public async Task<List<AreaDto>> TreeAsync()
    {
        var list = await _db.Set<Area>()
            // 数据库操作：升序排序
            .OrderBy(a => a.TreeSorts)
            // 数据库操作：异步查询为列表
            .ToListAsync();
        // return 返回结果
        return BuildTree(list, null);
    }

    // 方法 BuildTree
    // 方法：BuildTree
    private static List<AreaDto> BuildTree(List<Area> all, string? parentCode)
    {
        // return 返回结果
        return all
            // 数据库操作：条件过滤
            .Where(a => a.ParentCode == parentCode)
            // 数据库操作：投影选择
            .Select(a =>
            {
                // 声明并初始化变量：dto
                var dto = a.ToDto();
                dto.Children = BuildTree(all, a.AreaCode);
                // return 返回结果
                return dto;
            })
            // 数据库操作：查询为列表
            .ToList();
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(Area dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // if 条件判断
        if (string.IsNullOrEmpty(dto.AreaCode))
            // return 返回结果
            return ApiResult.Fail(400, "区域编码不能为空");
        // 数据库操作：异步取首条或默认值
        var exist = await _db.Set<Area>().FirstOrDefaultAsync(a => a.AreaCode == dto.AreaCode);
        // if 条件判断
        if (exist == null)
        {
            // 创建 Area实例并赋给 area
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
            // await 异步等待
            await _db.Set<Area>().AddAsync(area);
        }
        // else 否则分支
        else
        {
            exist.AreaName = dto.AreaName;
            exist.AreaType = dto.AreaType;
            exist.ParentCode = dto.ParentCode;
            exist.UpdateDate = now;
            // 调用 Update
            _db.Set<Area>().Update(exist);
        }
        // await 异步等待
        await _db.SaveChangesAsync();
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task<ApiResult> DeleteAsync(string areaCode)
    {
        // 数据库操作：异步取首条或默认值
        var exist = await _db.Set<Area>().FirstOrDefaultAsync(a => a.AreaCode == areaCode);
        // if 条件判断
        if (exist == null) return ApiResult.NotFound("区域不存在");
        // 数据库操作：异步检查是否存在
        var hasChildren = await _db.Set<Area>().AnyAsync(a => a.ParentCode == areaCode);
        // if 条件判断
        if (hasChildren) return ApiResult.Fail(400, "存在下级区域，无法删除");
        // 集合操作：移除元素
        _db.Set<Area>().Remove(exist);
        // await 异步等待
        await _db.SaveChangesAsync();
        // return 返回结果
        return ApiResult.Ok();
    }
}
