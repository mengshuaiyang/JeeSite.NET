using JeeSiteNET.Core;
using JeeSiteNET.Modules.Sys.Application.DTOs;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class LangService
{
    private readonly ILangRepository _langRepository;

    public LangService(ILangRepository langRepository)
    {
        _langRepository = langRepository;
    }

    public async Task<LangDto?> GetAsync(string id)
    {
        var entity = await _langRepository.GetAsync(id);
        return entity == null ? null : LangDto.FromEntity(entity);
    }

    public async Task<List<LangDto>> GetAllAsync()
    {
        var list = await _langRepository.FindListAsync();
        return list.Select(LangDto.FromEntity).ToList();
    }

    public async Task<List<LangDto>> GetByLangTypeAsync(string langType)
    {
        var list = await _langRepository.Query().Where(e => e.LangType == langType).ToListAsync();
        return list.Select(LangDto.FromEntity).ToList();
    }

    public async Task<ApiResult> SaveAsync(LangSaveDto dto)
    {
        var now = DateTime.Now;
        Lang? entity;
        if (!string.IsNullOrEmpty(dto.Id))
        {
            entity = await _langRepository.GetAsync(dto.Id);
            if (entity == null) return ApiResult.NotFound("语言项不存在");
            entity.ModuleCode = dto.ModuleCode;
            entity.LangCode = dto.LangCode;
            entity.LangText = dto.LangText;
            entity.LangType = dto.LangType;
            entity.UpdateDate = now;
            await _langRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new Lang
            {
                ModuleCode = dto.ModuleCode, LangCode = dto.LangCode,
                LangText = dto.LangText, LangType = dto.LangType,
                CreateDate = now, UpdateDate = now
            };
            await _langRepository.AddAsync(entity);
        }
        return ApiResult.Ok(LangDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteAsync(string id)
    {
        var entity = await _langRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("语言项不存在");
        await _langRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }
}
