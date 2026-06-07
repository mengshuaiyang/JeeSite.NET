using JeeSiteNET.Core;
using JeeSiteNET.Modules.App.Application.DTOs;
using JeeSiteNET.Modules.App.Domain.Entities;
using JeeSiteNET.Modules.App.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.App.Application.Services;

public class AppService
{
    private readonly IAppCommentRepository _commentRepository;
    private readonly IAppUpgradeRepository _upgradeRepository;

    public AppService(IAppCommentRepository commentRepository, IAppUpgradeRepository upgradeRepository)
    {
        _commentRepository = commentRepository;
        _upgradeRepository = upgradeRepository;
    }

    // --- Feedback ---

    public async Task<List<AppCommentDto>> GetCommentsAsync()
    {
        var list = await _commentRepository.FindListAsync();
        return list.Select(AppCommentDto.FromEntity).ToList();
    }

    public async Task<ApiResult> SaveCommentAsync(AppCommentSaveDto dto)
    {
        var now = DateTime.Now;
        var entity = new AppComment
        {
            Id = Guid.NewGuid().ToString("N")[..20],
            Category = dto.Category, Content = dto.Content,
            Contact = dto.Contact, Status = "0",
            CreateDate = now, UpdateDate = now
        };
        await _commentRepository.AddAsync(entity);
        return ApiResult.Ok(AppCommentDto.FromEntity(entity));
    }

    public async Task<ApiResult> ReplyCommentAsync(string id, string replyContent)
    {
        var entity = await _commentRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("反馈不存在");
        entity.ReplyContent = replyContent;
        entity.ReplyDate = DateTime.Now;
        entity.Status = "2";
        await _commentRepository.UpdateAsync(entity);
        return ApiResult.Ok();
    }

    public async Task<ApiResult> DeleteCommentAsync(string id)
    {
        var entity = await _commentRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("反馈不存在");
        await _commentRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }

    // --- Upgrade ---

    public async Task<List<AppUpgradeDto>> GetUpgradesAsync()
    {
        var list = await _upgradeRepository.FindListAsync();
        return list.Select(AppUpgradeDto.FromEntity).ToList();
    }

    public async Task<AppUpgradeDto?> GetLatestUpgradeAsync(string? appCode)
    {
        var query = _upgradeRepository.Query();
        if (!string.IsNullOrEmpty(appCode))
            query = query.Where(e => e.AppCode == appCode);
        var entity = await query.OrderByDescending(e => e.UpVersion).FirstOrDefaultAsync();
        return entity == null ? null : AppUpgradeDto.FromEntity(entity);
    }

    public async Task<ApiResult> SaveUpgradeAsync(AppUpgradeSaveDto dto)
    {
        var now = DateTime.Now;
        AppUpgrade? entity;
        if (!string.IsNullOrEmpty(dto.Id))
        {
            entity = await _upgradeRepository.GetAsync(dto.Id);
            if (entity == null) return ApiResult.NotFound("升级版本不存在");
            entity.AppCode = dto.AppCode; entity.UpTitle = dto.UpTitle;
            entity.UpContent = dto.UpContent; entity.UpVersion = dto.UpVersion;
            entity.UpType = dto.UpType; entity.UpDate = dto.UpDate;
            entity.ApkUrl = dto.ApkUrl; entity.ResUrl = dto.ResUrl;
            entity.UpdateDate = now;
            await _upgradeRepository.UpdateAsync(entity);
        }
        else
        {
            entity = new AppUpgrade
            {
                Id = Guid.NewGuid().ToString("N")[..20],
                AppCode = dto.AppCode, UpTitle = dto.UpTitle,
                UpContent = dto.UpContent, UpVersion = dto.UpVersion,
                UpType = dto.UpType, UpDate = dto.UpDate,
                ApkUrl = dto.ApkUrl, ResUrl = dto.ResUrl,
                Status = "0", CreateDate = now, UpdateDate = now
            };
            await _upgradeRepository.AddAsync(entity);
        }
        return ApiResult.Ok(AppUpgradeDto.FromEntity(entity));
    }

    public async Task<ApiResult> DeleteUpgradeAsync(string id)
    {
        var entity = await _upgradeRepository.GetAsync(id);
        if (entity == null) return ApiResult.NotFound("升级版本不存在");
        await _upgradeRepository.DeleteAsync(entity);
        return ApiResult.Ok();
    }
}
