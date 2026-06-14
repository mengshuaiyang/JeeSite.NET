    // 引入 JeeSiteNET.Modules.App.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.App.Domain.Entities
using JeeSiteNET.Modules.App.Domain.Entities;

// 定义 JeeSiteNET.Modules.App.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.App.Application.DTOs
namespace JeeSiteNET.Modules.App.Application.DTOs;

// 定义class AppCommentDto
// 定义类：AppCommentDto
public class AppCommentDto
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性：Category
    public string? Category { get; set; }
    // 属性：Content
    public string? Content { get; set; }
    // 属性：Contact
    public string? Contact { get; set; }
    // 属性：CreateByName
    public string? CreateByName { get; set; }
    // 属性：DeviceInfo
    public string? DeviceInfo { get; set; }
    // 属性：ReplyDate
    public DateTime? ReplyDate { get; set; }
    // 属性：ReplyContent
    public string? ReplyContent { get; set; }
    // 属性：ReplyUserCode
    public string? ReplyUserCode { get; set; }
    // 属性：ReplyUserName
    public string? ReplyUserName { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：CreateBy
    public string? CreateBy { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
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

// 定义class AppCommentSaveDto
// 定义类：AppCommentSaveDto
public class AppCommentSaveDto
{
    // 属性：Category
    public string? Category { get; set; }
    // 属性：Content
    public string? Content { get; set; }
    // 属性：Contact
    public string? Contact { get; set; }
}

// 定义class AppUpgradeDto
// 定义类：AppUpgradeDto
public class AppUpgradeDto
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性：AppCode
    public string? AppCode { get; set; }
    // 属性：UpTitle
    public string? UpTitle { get; set; }
    // 属性：UpContent
    public string? UpContent { get; set; }
    // 属性：UpVersion
    public int? UpVersion { get; set; }
    // 属性：UpType
    public string? UpType { get; set; }
    // 属性：UpDate
    public DateTime? UpDate { get; set; }
    // 属性：ApkUrl
    public string? ApkUrl { get; set; }
    // 属性：ResUrl
    public string? ResUrl { get; set; }
    // 属性：Status
    public string? Status { get; set; }
    // 属性：CreateBy
    public string? CreateBy { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static AppUpgradeDto FromEntity(AppUpgrade e) => new()
    {
        Id = e.Id, AppCode = e.AppCode, UpTitle = e.UpTitle,
        UpContent = e.UpContent, UpVersion = e.UpVersion,
        UpType = e.UpType, UpDate = e.UpDate,
        ApkUrl = e.ApkUrl, ResUrl = e.ResUrl,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

// 定义class AppUpgradeSaveDto
// 定义类：AppUpgradeSaveDto
public class AppUpgradeSaveDto
{
    // 属性：Id
    public string? Id { get; set; }
    // 属性：AppCode
    public string? AppCode { get; set; }
    // 属性：UpTitle
    public string? UpTitle { get; set; }
    // 属性：UpContent
    public string? UpContent { get; set; }
    // 属性：UpVersion
    public int? UpVersion { get; set; }
    // 属性：UpType
    public string? UpType { get; set; }
    // 属性：UpDate
    public DateTime? UpDate { get; set; }
    // 属性：ApkUrl
    public string? ApkUrl { get; set; }
    // 属性：ResUrl
    public string? ResUrl { get; set; }
}
