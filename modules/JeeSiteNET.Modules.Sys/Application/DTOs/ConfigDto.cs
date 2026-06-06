namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class ConfigDto
{
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigName { get; set; } = string.Empty;
    public string? ConfigValue { get; set; }
    public string? IsSys { get; set; }
    public string? Status { get; set; }
}

public class ConfigSaveDto
{
    public string? ConfigKey { get; set; }
    public string ConfigName { get; set; } = string.Empty;
    public string? ConfigValue { get; set; }
    public string? IsSys { get; set; }
}
