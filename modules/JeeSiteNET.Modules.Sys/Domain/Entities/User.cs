using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 用户实体，代表系统中可登录的用户账号。包含登录凭证、身份信息、安全等级、密码策略、登录日志等核心字段。
/// 支持公司隔离（ICorpEntity）与扩展字段（IExtendEntity）。
/// </summary>
public class User : DataEntity, ICorpEntity, IExtendEntity
{
    /// <summary>用户编码，业务主键，全局唯一。</summary>
    public string UserCode { get; set; } = string.Empty;
    /// <summary>登录账号，用户输入的登录名，支持英文/数字。</summary>
    public string LoginCode { get; set; } = string.Empty;
    /// <summary>用户真实姓名或昵称，用于界面展示。</summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>登录密码（加密存储），切勿明文保存。</summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>用户类型：employee（员工）、manager（管理）等，默认 employee。</summary>
    public string UserType { get; set; } = "employee";
    /// <summary>头像 URL 或 Base64。</summary>
    public string? Avatar { get; set; }
    /// <summary>电子邮箱，用于找回密码、通知。</summary>
    public string? Email { get; set; }
    /// <summary>手机号，用于短信验证码、登录通知。</summary>
    public string? Phone { get; set; }
    /// <summary>所属机构编码（引用 Organization.OrgCode）。</summary>
    public string? OrgCode { get; set; }
    /// <summary>所属机构名称，冗余存储便于列表展示。</summary>
    public string? OrgName { get; set; }
    /// <summary>性别：男 / 女 / 未知。</summary>
    public string? Sex { get; set; }
    /// <summary>个性签名。</summary>
    public string? Sign { get; set; }
    /// <summary>微信 OpenID，用于微信扫码/公众号绑定登录。</summary>
    public string? WxOpenid { get; set; }
    /// <summary>移动端设备 IMEI，用于设备绑定校验。</summary>
    public string? MobileImei { get; set; }
    /// <summary>引用编码（关联第三方员工/客户ID）。</summary>
    public string? RefCode { get; set; }
    /// <summary>引用名称（第三方员工/客户名称）。</summary>
    public string? RefName { get; set; }
    /// <summary>管理员类型：空=普通用户、1=公司管理员、2=集团管理员、9=超级管理员。</summary>
    public string? MgrType { get; set; }
    /// <summary>密码安全等级：weak/normal/strong/very_strong。</summary>
    public string? PwdSecurityLevel { get; set; }
    /// <summary>最近一次成功登录时间。</summary>
    public DateTime? LoginDate { get; set; }
    /// <summary>最近一次登录 IP 地址。</summary>
    public string? LoginIp { get; set; }
    /// <summary>累计登录次数。</summary>
    public decimal? LoginCount { get; set; }
    /// <summary>最近一次密码修改时间。</summary>
    public DateTime? PwdUpdateDate { get; set; }
    /// <summary>历史密码记录（哈希链），用于禁止复用旧密码。</summary>
    public string? PwdUpdateRecord { get; set; }
    /// <summary>密保问题。</summary>
    public string? PwdQuestion { get; set; }
    /// <summary>密保问题答案（加密存储）。</summary>
    public string? PwdQuestionAnswer { get; set; }
    /// <summary>账号冻结日期，超过当前时间即禁止登录。</summary>
    public DateTime? FreezeDate { get; set; }
    /// <summary>冻结原因描述。</summary>
    public string? FreezeCause { get; set; }
    /// <summary>用户权重，用于排序或优先级计算。</summary>
    public decimal? UserWeight { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }

    /// <summary>扩展字符串字段 1。</summary>
    public string? ExtendS1 { get; set; }
    /// <summary>扩展字符串字段 2。</summary>
    public string? ExtendS2 { get; set; }
    /// <summary>扩展字符串字段 3。</summary>
    public string? ExtendS3 { get; set; }
    /// <summary>扩展字符串字段 4。</summary>
    public string? ExtendS4 { get; set; }
    /// <summary>扩展字符串字段 5。</summary>
    public string? ExtendS5 { get; set; }
    /// <summary>扩展字符串字段 6。</summary>
    public string? ExtendS6 { get; set; }
    /// <summary>扩展字符串字段 7。</summary>
    public string? ExtendS7 { get; set; }
    /// <summary>扩展字符串字段 8。</summary>
    public string? ExtendS8 { get; set; }
    /// <summary>扩展整型字段 1。</summary>
    public int? ExtendI1 { get; set; }
    /// <summary>扩展整型字段 2。</summary>
    public int? ExtendI2 { get; set; }
    /// <summary>扩展整型字段 3。</summary>
    public int? ExtendI3 { get; set; }
    /// <summary>扩展整型字段 4。</summary>
    public int? ExtendI4 { get; set; }
    /// <summary>扩展十进制字段 1。</summary>
    public decimal? ExtendF1 { get; set; }
    /// <summary>扩展十进制字段 2。</summary>
    public decimal? ExtendF2 { get; set; }
    /// <summary>扩展十进制字段 3。</summary>
    public decimal? ExtendF3 { get; set; }
    /// <summary>扩展十进制字段 4。</summary>
    public decimal? ExtendF4 { get; set; }
    /// <summary>扩展日期字段 1。</summary>
    public DateTime? ExtendD1 { get; set; }
    /// <summary>扩展日期字段 2。</summary>
    public DateTime? ExtendD2 { get; set; }
    /// <summary>扩展日期字段 3。</summary>
    public DateTime? ExtendD3 { get; set; }
    /// <summary>扩展日期字段 4。</summary>
    public DateTime? ExtendD4 { get; set; }
    /// <summary>扩展 JSON 字段，用于存储自定义结构化数据。</summary>
    public string? ExtendJson { get; set; }
}
