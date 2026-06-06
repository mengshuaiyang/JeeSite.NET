using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Module : DataEntity
{
    public string ModuleCode { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string? ModuleVersion { get; set; }
    public string? MainClass { get; set; }
    public string? IsEnabled { get; set; } = "1";
}
