using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Employee : DataEntity, ICorpEntity, IExtendEntity
{
    public string EmpCode { get; set; } = string.Empty;
    public string? EmpNo { get; set; }
    public string EmpName { get; set; } = string.Empty;
    public string? EmpNameEn { get; set; }
    public string OfficeCode { get; set; } = string.Empty;
    public string OfficeName { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

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
