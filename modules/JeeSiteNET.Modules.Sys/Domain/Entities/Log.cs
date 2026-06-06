using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Log : BaseEntity, ICorpEntity
{
    public string LogId { get; set; } = string.Empty;
    public string? LogType { get; set; }
    public string? LogTitle { get; set; }
    public string? RequestUri { get; set; }
    public string? RequestMethod { get; set; }
    public string? Params { get; set; }
    public string? DiffData { get; set; }
    public string? BizKey { get; set; }
    public string? BizType { get; set; }
    public decimal? ExecuteTime { get; set; }
    public string? UserCode { get; set; }
    public string? UserName { get; set; }
    public string? OrgCode { get; set; }
    public string? RemoteIp { get; set; }
    public string? ServerAddr { get; set; }
    public string? UserAgent { get; set; }
    public string? DeviceName { get; set; }
    public string? BrowserName { get; set; }
    public string? IsException { get; set; }
    public string? ExceptionInfo { get; set; }

    public string? CorpCode { get; set; }
    public string? CorpName { get; set; }
}
