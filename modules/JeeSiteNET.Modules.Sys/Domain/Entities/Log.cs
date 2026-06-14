using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 操作日志实体，记录每次请求的行为、参数、IP、用户、异常等信息。是审计与追溯的核心数据源。
/// </summary>
public class Log : BaseEntity, ICorpEntity
{
    /// <summary>日志 ID，业务主键。</summary>
    public string LogId { get; set; } = string.Empty;
    /// <summary>日志类型：access（访问日志）、operation（操作日志）、exception（异常日志）。</summary>
    public string? LogType { get; set; }
    /// <summary>日志标题/操作名称（如：新增用户、删除订单）。</summary>
    public string? LogTitle { get; set; }
    /// <summary>请求 URI。</summary>
    public string? RequestUri { get; set; }
    /// <summary>请求方法：GET / POST / PUT / DELETE。</summary>
    public string? RequestMethod { get; set; }
    /// <summary>请求参数（JSON 或 QueryString 截断后存储）。</summary>
    public string? Params { get; set; }
    /// <summary>差异数据：记录更新前后字段变化（DiffMatchPatch 格式）。</summary>
    public string? DiffData { get; set; }
    /// <summary>关联业务对象主键（如 UserCode / OrderId）。</summary>
    public string? BizKey { get; set; }
    /// <summary>业务类型编码（标识 bizKey 所属实体类型）。</summary>
    public string? BizType { get; set; }
    /// <summary>请求执行耗时（毫秒）。</summary>
    public decimal? ExecuteTime { get; set; }
    /// <summary>操作者用户编码。</summary>
    public string? UserCode { get; set; }
    /// <summary>操作者用户名称。</summary>
    public string? UserName { get; set; }
    /// <summary>创建者姓名（冗余字段）。</summary>
    public string? CreateByName { get; set; }
    /// <summary>所属机构编码。</summary>
    public string? OrgCode { get; set; }
    /// <summary>客户端 IP 地址。</summary>
    public string? RemoteIp { get; set; }
    /// <summary>服务器地址。</summary>
    public string? ServerAddr { get; set; }
    /// <summary>浏览器 User-Agent 原文。</summary>
    public string? UserAgent { get; set; }
    /// <summary>识别出的设备名称（Android/iOS/Windows 等）。</summary>
    public string? DeviceName { get; set; }
    /// <summary>识别出的浏览器名称。</summary>
    public string? BrowserName { get; set; }
    /// <summary>是否发生异常：1=是，0=否。</summary>
    public string? IsException { get; set; }
    /// <summary>异常堆栈信息（截断后存储）。</summary>
    public string? ExceptionInfo { get; set; }

    /// <summary>公司编码（多租户/多公司隔离字段）。</summary>
    public string? CorpCode { get; set; }
    /// <summary>公司名称，冗余便于展示。</summary>
    public string? CorpName { get; set; }
}
