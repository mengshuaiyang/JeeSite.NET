using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class User : DataEntity, ICorpEntity, IExtendEntity
{
    public string UserCode { get; set; } = string.Empty;
    public string LoginCode { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserType { get; set; } = "employee";
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OrgCode { get; set; }
    public string? OrgName { get; set; }
    public string? Sex { get; set; }
    public string? Sign { get; set; }
    public string? WxOpenid { get; set; }
    public string? MobileImei { get; set; }
    public string? RefCode { get; set; }
    public string? RefName { get; set; }
    public string? MgrType { get; set; }
    public string? PwdSecurityLevel { get; set; }
    public DateTime? LoginDate { get; set; }
    public string? LoginIp { get; set; }
    public decimal? LoginCount { get; set; }
    public DateTime? PwdUpdateDate { get; set; }
    public string? PwdUpdateRecord { get; set; }
    public string? PwdQuestion { get; set; }
    public string? PwdQuestionAnswer { get; set; }
    public DateTime? FreezeDate { get; set; }
    public string? FreezeCause { get; set; }
    public decimal? UserWeight { get; set; }

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
