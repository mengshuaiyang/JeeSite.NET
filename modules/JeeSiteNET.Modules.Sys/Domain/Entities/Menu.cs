using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Menu : TreeEntity
{
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? MenuHref { get; set; }
    public string? MenuTarget { get; set; }
    public string? MenuIcon { get; set; }
    public string? Permission { get; set; }
    public decimal? Weight { get; set; } = 0;
    public string? IsShow { get; set; } = "1";
    public string? ModuleCode { get; set; }

    public override string GetName() => MenuName;
}
