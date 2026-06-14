    // 引入 JeeSiteNET.Core 命名空间
// 引入命名空间：JeeSiteNET.Core
using JeeSiteNET.Core;
    // 引入 JeeSiteNET.Modules.App.Application.DTOs 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Application.DTOs
using JeeSiteNET.Modules.App.Application.DTOs;
    // 引入 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Entities
using JeeSiteNET.Modules.App.Domain.Entities;
    // 引入 JeeSiteNET.Modules.App.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Interfaces
using JeeSiteNET.Modules.App.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.App.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Application.Services
namespace JeeSiteNET.Modules.App.Application.Services;

// 定义class AppService
// 定义类：AppService
public class AppService
{
    // 字段 _commentRepository
    // 字段：_commentRepository
    private readonly IAppCommentRepository _commentRepository;
    // 字段 _upgradeRepository
    // 字段：_upgradeRepository
    private readonly IAppUpgradeRepository _upgradeRepository;

    // 方法 AppService
    // 构造函数：AppService
    public AppService(IAppCommentRepository commentRepository, IAppUpgradeRepository upgradeRepository)
    {
        _commentRepository = commentRepository;
        _upgradeRepository = upgradeRepository;
    }

    // --- Feedback ---

    // 方法 GetCommentsAsync
    // 方法：GetCommentsAsync
    public async Task<List<AppCommentDto>> GetCommentsAsync()
    {
        var list = await _commentRepository.FindListAsync();
        // return 返回结果
        return list.Select(AppCommentDto.FromEntity).ToList();
    }

    // 方法 SaveCommentAsync
    // 方法：SaveCommentAsync
    public async Task<ApiResult> SaveCommentAsync(AppCommentSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // 创建 AppComment实例并赋给 entity
        var entity = new AppComment
        {
            // 调用 ToString
            Id = Guid.NewGuid().ToString("N")[..20],
            Category = dto.Category, Content = dto.Content,
            Contact = dto.Contact, Status = "0",
            CreateDate = now, UpdateDate = now
        };
        // await 异步等待
        await _commentRepository.AddAsync(entity);
        // return 返回结果
        return ApiResult.Ok(AppCommentDto.FromEntity(entity));
    }

    // 方法 ReplyCommentAsync
    // 方法：ReplyCommentAsync
    public async Task<ApiResult> ReplyCommentAsync(string id, string replyContent)
    {
        // 缓存：获取值
        var entity = await _commentRepository.GetAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("反馈不存在");
        entity.ReplyContent = replyContent;
        entity.ReplyDate = DateTime.Now;
        entity.Status = "2";
        // await 异步等待
        await _commentRepository.UpdateAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // 方法 DeleteCommentAsync
    // 方法：DeleteCommentAsync
    public async Task<ApiResult> DeleteCommentAsync(string id)
    {
        // 缓存：获取值
        var entity = await _commentRepository.GetAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("反馈不存在");
        // await 异步等待
        await _commentRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }

    // --- Upgrade ---

    // 方法 GetUpgradesAsync
    // 方法：GetUpgradesAsync
    public async Task<List<AppUpgradeDto>> GetUpgradesAsync()
    {
        var list = await _upgradeRepository.FindListAsync();
        // return 返回结果
        return list.Select(AppUpgradeDto.FromEntity).ToList();
    }

    // 方法 GetLatestUpgradeAsync
    // 方法：GetLatestUpgradeAsync
    public async Task<AppUpgradeDto?> GetLatestUpgradeAsync(string? appCode)
    {
        // 调用 Query
        var query = _upgradeRepository.Query();
        // if 条件判断
        if (!string.IsNullOrEmpty(appCode))
            // 数据库操作：条件过滤
            query = query.Where(e => e.AppCode == appCode);
        // 数据库操作：降序排序
        var entity = await query.OrderByDescending(e => e.UpVersion).FirstOrDefaultAsync();
        // return 返回结果
        return entity == null ? null : AppUpgradeDto.FromEntity(entity);
    }

    // 方法 SaveUpgradeAsync
    // 方法：SaveUpgradeAsync
    public async Task<ApiResult> SaveUpgradeAsync(AppUpgradeSaveDto dto)
    {
        // 声明并初始化变量：now
        var now = DateTime.Now;
        AppUpgrade? entity;
        // if 条件判断
        if (!string.IsNullOrEmpty(dto.Id))
        {
            // 缓存：获取值
            entity = await _upgradeRepository.GetAsync(dto.Id);
            // if 条件判断
            if (entity == null) return ApiResult.NotFound("升级版本不存在");
            entity.AppCode = dto.AppCode; entity.UpTitle = dto.UpTitle;
            entity.UpContent = dto.UpContent; entity.UpVersion = dto.UpVersion;
            entity.UpType = dto.UpType; entity.UpDate = dto.UpDate;
            entity.ApkUrl = dto.ApkUrl; entity.ResUrl = dto.ResUrl;
            entity.UpdateDate = now;
            // await 异步等待
            await _upgradeRepository.UpdateAsync(entity);
        }
        // else 否则分支
        else
        {
            // 创建 AppUpgrade实例并赋给 entity
            entity = new AppUpgrade
            {
                // 调用 ToString
                Id = Guid.NewGuid().ToString("N")[..20],
                AppCode = dto.AppCode, UpTitle = dto.UpTitle,
                UpContent = dto.UpContent, UpVersion = dto.UpVersion,
                UpType = dto.UpType, UpDate = dto.UpDate,
                ApkUrl = dto.ApkUrl, ResUrl = dto.ResUrl,
                Status = "0", CreateDate = now, UpdateDate = now
            };
            // await 异步等待
            await _upgradeRepository.AddAsync(entity);
        }
        // return 返回结果
        return ApiResult.Ok(AppUpgradeDto.FromEntity(entity));
    }

    // 方法 DeleteUpgradeAsync
    // 方法：DeleteUpgradeAsync
    public async Task<ApiResult> DeleteUpgradeAsync(string id)
    {
        // 缓存：获取值
        var entity = await _upgradeRepository.GetAsync(id);
        // if 条件判断
        if (entity == null) return ApiResult.NotFound("升级版本不存在");
        // await 异步等待
        await _upgradeRepository.DeleteAsync(entity);
        // return 返回结果
        return ApiResult.Ok();
    }
}
