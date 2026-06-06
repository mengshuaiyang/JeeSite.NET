using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Menu : TreeEntity, ICorpEntity, IExtendEntity
{
    public string MenuCode { get; set; } = string.Empty;
    public string MenuName { get; set; } = string.Empty;
    public string? MenuType { get; set; }
    public string? MenuHref { get; set; }
    public string? MenuTarget { get; set; }
    public string? MenuIcon { get; set; }
    public string? MenuColor { get; set; }
    public string? MenuTitle { get; set; }
    public string? Permission { get; set; }
    public decimal? Weight { get; set; } = 0;
    public string? IsShow { get; set; } = "1";
    public string? SysCode { get; set; }
    public string? ModuleCodes { get; set; }
    public string? ModuleCode { get; set; }
    public string? Component { get; set; }
    public string? Params { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }

    public string? ExtendS1 { get; set; }
    public string? ExtendS2 { get; set; }
    public string? ExtendS3 { get; set; }
    public string? ExtendS4 { get; set; }
    public string? ExtendS5 { get; set; }
    public string? ExtendS6 { get; set; }
    public string? ExtendS7 { get; set; }
    public string? ExtendS8 { get; set; }
    public int? ExtendI1 { get; set; }
    public int? ExtendI2 { get; set; }
    public int? ExtendI3 { get; set; }
    public int? ExtendI4 { get; set; }
    public decimal? ExtendF1 { get; set; }
    public decimal? ExtendF2 { get; set; }
    public decimal? ExtendF3 { get; set; }
    public decimal? ExtendF4 { get; set; }
    public DateTime? ExtendD1 { get; set; }
    public DateTime? ExtendD2 { get; set; }
    public DateTime? ExtendD3 { get; set; }
    public DateTime? ExtendD4 { get; set; }
    public string? ExtendJson { get; set; }

    public override string GetName() => MenuName;
}
