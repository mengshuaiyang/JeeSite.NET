using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Config : DataEntity
{
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigName { get; set; } = string.Empty;
    public string? ConfigValue { get; set; }
    public string? IsSys { get; set; } = "0";
}
