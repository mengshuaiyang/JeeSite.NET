using JeeSiteNET.Modules.Sys.Domain.Entities;

namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class LangDto
{
    public string Id { get; set; } = string.Empty;
    public string ModuleCode { get; set; } = string.Empty;
    public string LangCode { get; set; } = string.Empty;
    public string LangText { get; set; } = string.Empty;
    public string LangType { get; set; } = string.Empty;
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }

    public static LangDto FromEntity(Lang e) => new()
    {
        Id = e.Id, ModuleCode = e.ModuleCode,
        LangCode = e.LangCode, LangText = e.LangText,
        LangType = e.LangType, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

public class LangSaveDto
{
    public string? Id { get; set; }
    public string ModuleCode { get; set; } = string.Empty;
    public string LangCode { get; set; } = string.Empty;
    public string LangText { get; set; } = string.Empty;
    public string LangType { get; set; } = string.Empty;
}
