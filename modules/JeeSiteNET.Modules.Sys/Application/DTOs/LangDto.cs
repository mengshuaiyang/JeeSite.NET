    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;

// 定义 JeeSiteNET.Modules.Sys.Application.DTOs 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.DTOs
namespace JeeSiteNET.Modules.Sys.Application.DTOs;

// 定义class LangDto
// 定义类：LangDto
public class LangDto
{
    // 属性 Id
    // 属性：Id
    public string Id { get; set; } = string.Empty;
    // 属性 ModuleCode
    // 属性：ModuleCode
    public string ModuleCode { get; set; } = string.Empty;
    // 属性 LangCode
    // 属性：LangCode
    public string LangCode { get; set; } = string.Empty;
    // 属性 LangText
    // 属性：LangText
    public string LangText { get; set; } = string.Empty;
    // 属性 LangType
    // 属性：LangType
    public string LangType { get; set; } = string.Empty;
    // 属性：CreateBy
    public string? CreateBy { get; set; }
    // 属性：CreateDate
    public DateTime? CreateDate { get; set; }

    // 方法 FromEntity
    // 方法：FromEntity
    public static LangDto FromEntity(Lang e) => new()
    {
        Id = e.Id, ModuleCode = e.ModuleCode,
        LangCode = e.LangCode, LangText = e.LangText,
        LangType = e.LangType, CreateBy = e.CreateBy, CreateDate = e.CreateDate
    };
}

// 定义class LangSaveDto
// 定义类：LangSaveDto
public class LangSaveDto
{
    // 属性：Id
    public string? Id { get; set; }
    // 属性 ModuleCode
    // 属性：ModuleCode
    public string ModuleCode { get; set; } = string.Empty;
    // 属性 LangCode
    // 属性：LangCode
    public string LangCode { get; set; } = string.Empty;
    // 属性 LangText
    // 属性：LangText
    public string LangText { get; set; } = string.Empty;
    // 属性 LangType
    // 属性：LangType
    public string LangType { get; set; } = string.Empty;
}
