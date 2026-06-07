using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.DTOs;

public class GuestbookDto
{
    public string GbCode { get; set; } = string.Empty;
    public string GbType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string WorkUnit { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string? ReUserCode { get; set; }
    public DateTime? ReDate { get; set; }
    public string? ReContent { get; set; }
    public string? Status { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static GuestbookDto FromEntity(Guestbook e) => new()
    {
        GbCode = e.GbCode, GbType = e.GbType, Content = e.Content,
        Name = e.Name, Email = e.Email, Phone = e.Phone,
        WorkUnit = e.WorkUnit, Ip = e.Ip, ReUserCode = e.ReUserCode,
        ReDate = e.ReDate, ReContent = e.ReContent,
        Status = e.Status, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class GuestbookSaveDto
{
    public string GbType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string WorkUnit { get; set; } = string.Empty;
}
