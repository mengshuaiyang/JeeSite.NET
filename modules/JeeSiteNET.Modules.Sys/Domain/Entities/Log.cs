using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

public class Log : BaseEntity
{
    public string LogId { get; set; } = string.Empty;
    public string? LogType { get; set; }
    public string? LogTitle { get; set; }
    public string? RequestUri { get; set; }
    public string? RequestMethod { get; set; }
    public string? Params { get; set; }
    public string? DiffData { get; set; }
    public decimal? ExecuteTime { get; set; }
    public string? UserCode { get; set; }
    public string? UserName { get; set; }
    public string? OrgCode { get; set; }
    public string? RemoteIp { get; set; }
    public string? UserAgent { get; set; }
}
