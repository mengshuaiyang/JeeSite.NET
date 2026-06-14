    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
using JeeSiteNET.Modules.Sys.Application.DTOs;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class LangService
// 定义类：LangService
public class LangService
{
    // 字段 _langRepository
    // 字段：_langRepository
    private readonly ILangRepository _langRepository;

    // 方法 LangService
    // 构造函数：LangService
    public LangService(ILangRepository langRepository)
    {
        _langRepository = langRepository;
    }

    // 方法 GetAsync
    // 方法：GetAsync
    public async Task<LangDto?> GetAsync(string id)
    {
        // 缓存：获取值
        var entity = await _langRepository.GetAsync(id);
        // return 返回结果
        return entity == null ? null : LangDto.FromEntity(entity);
    }

    // 方法 GetAllAsync
    // 方法：GetAllAsync
    public async Task<List<LangDto>> GetAllAsync()
    {
        var list = await _langRepository.FindListAsync();
        // return 返回结果
        return list.Select(LangDto.FromEntity).ToList();
    }

    // 方法 GetByLangTypeAsync
    // 方法：GetByLangTypeAsync
    public async Task<List<LangDto>> GetByLangTypeAsync(string langType)
    {
        // 数据库操作：条件过滤
        var list = await _langRepository.Query().Where(e => e.LangType == langType).ToListAsync();
        // return 返回结果
        return list.Select(LangDto.FromEntity).ToList();
    }

    // 方法 SaveAsync
    // 方法：SaveAsync
    public async Task<ApiResult> SaveAsync(LangSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        Lang? entity;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.Id))
        {
            // 缓存：获取值
            entity = await _langRepository.GetAsync(dto.Id);
            // if 条件判断
            if (entity == null) return ApiResult.NotFound("语言项不存在");
            entity.ModuleCode = dto.ModuleCode;
            entity.LangCode = dto.LangCode;
            entity.LangText = dto.LangText;
            entity.LangType = dto.LangType;
            entity.UpdateDate = now;
            // await 异步等待
            await _langRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 创建 Lang实例并赋给 entity
            entity = new Lang
            {
                ModuleCode = dto.ModuleCode, LangCode = dto.LangCode,
                LangText = dto.LangText, LangType = dto.LangType,
                CreateDate = now, UpdateDate = now
            };
            // await 异步等待
            await _langRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(LangDto.FromEntity(entity));
    }

    // 方法 DeleteAsync
    // 方法：DeleteAsync
    public async Task<ApiResult> DeleteAsync(string id)
    {
        // 缓存：获取值
        var entity = await _langRepository.GetAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("语言项不存在");
        // await 异步等待
        await _langRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }
}
