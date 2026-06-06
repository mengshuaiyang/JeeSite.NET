using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Module : DataEntity
{
    public string ModuleCode { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ModuleVersion { get; set; }
    public string? MainClass { get; set; }
    public string? PackageName { get; set; }
    public decimal? ModuleSort { get; set; }
    public string? CurrentVersion { get; set; }
    public string? UpgradeInfo { get; set; }
    public string? GenBaseDir { get; set; }
    public string? GenFrontDir { get; set; }
    public string? TplCategory { get; set; }
    public string? IsEnabled { get; set; } = "1";
}
