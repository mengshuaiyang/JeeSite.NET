using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Lang : BaseEntity
{
    public string Id { get; set; } = string.Empty;
    public string ModuleCode { get; set; } = string.Empty;
    public string LangCode { get; set; } = string.Empty;
    public string LangText { get; set; } = string.Empty;
    public string LangType { get; set; } = string.Empty;
}
