using JeeSiteNET.Modules.App.Domain.Entities;

namespace JeeSiteNET.Modules.App.Application.DTOs;

public class AppCommentDto
{
    public string Id { get; set; } = string.Empty;
    public string? Category { get; set; }
    public string? Content { get; set; }
    public string? Contact { get; set; }
    public string? CreateByName { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTime? ReplyDate { get; set; }
    public string? ReplyContent { get; set; }
    public string? ReplyUserCode { get; set; }
    public string? ReplyUserName { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static AppCommentDto FromEntity(AppComment e) => new()
    {
        Id = e.Id, Category = e.Category, Content = e.Content,
        Contact = e.Contact, CreateByName = e.CreateByName,
        DeviceInfo = e.DeviceInfo, ReplyDate = e.ReplyDate,
        ReplyContent = e.ReplyContent, ReplyUserCode = e.ReplyUserCode,
        ReplyUserName = e.ReplyUserName, Status = e.Status,
        CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class AppCommentSaveDto
{
    public string? Category { get; set; }
    public string? Content { get; set; }
    public string? Contact { get; set; }
}

public class AppUpgradeDto
{
    public string Id { get; set; } = string.Empty;
    public string? AppCode { get; set; }
    public string? UpTitle { get; set; }
    public string? UpContent { get; set; }
    public int? UpVersion { get; set; }
    public string? UpType { get; set; }
    public DateTime? UpDate { get; set; }
    public string? ApkUrl { get; set; }
    public string? ResUrl { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static AppUpgradeDto FromEntity(AppUpgrade e) => new()
    {
        Id = e.Id, AppCode = e.AppCode, UpTitle = e.UpTitle,
        UpContent = e.UpContent, UpVersion = e.UpVersion,
        UpType = e.UpType, UpDate = e.UpDate,
        ApkUrl = e.ApkUrl, ResUrl = e.ResUrl,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class AppUpgradeSaveDto
{
    public string? Id { get; set; }
    public string? AppCode { get; set; }
    public string? UpTitle { get; set; }
    public string? UpContent { get; set; }
    public int? UpVersion { get; set; }
    public string? UpType { get; set; }
    public DateTime? UpDate { get; set; }
    public string? ApkUrl { get; set; }
    public string? ResUrl { get; set; }
}
