using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Role : DataEntity, ICorpEntity, IExtendEntity
{
    public string RoleCode { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? ViewCode { get; set; }
    public string? RoleType { get; set; }
    public decimal? RoleSort { get; set; }
    public string? IsSys { get; set; } = "0";
    public string? IsShow { get; set; } = "1";
    public string? UserType { get; set; }
    public string? DesktopUrl { get; set; }
    public string? DataScope { get; set; }
    public string? BizScope { get; set; }
    public string? SysCodes { get; set; }

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
}
