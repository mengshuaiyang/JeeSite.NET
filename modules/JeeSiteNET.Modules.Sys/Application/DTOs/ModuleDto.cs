namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class ModuleDto
{
    public string ModuleCode { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string? ModuleVersion { get; set; }
    public string? MainClass { get; set; }
    public string? IsEnabled { get; set; }
    public string? Status { get; set; }
}

public class ModuleSaveDto
{
    public string? ModuleCode { get; set; }
    public string ModuleName { get; set; } = string.Empty;
    public string? ModuleVersion { get; set; }
    public string? MainClass { get; set; }
    public string? IsEnabled { get; set; }
}
