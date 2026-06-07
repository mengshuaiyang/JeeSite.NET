using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class MsgTemplate : DataEntity
{
    public string Id { get; set; } = string.Empty;
    public string? ModuleCode { get; set; }
    public string TplKey { get; set; } = string.Empty;
    public string TplName { get; set; } = string.Empty;
    public string TplType { get; set; } = string.Empty;
    public string TplContent { get; set; } = string.Empty;
}
