namespace JeeSiteNET.Modules.Sys.Application.DTOs;

public class LogDto
{
    public string LogId { get; set; } = string.Empty;
    public string? LogType { get; set; }
    public string? LogTitle { get; set; }
    public string? RequestUri { get; set; }
    public string? RequestMethod { get; set; }
    public decimal? ExecuteTime { get; set; }
    public string? UserCode { get; set; }
    public string? UserName { get; set; }
    public string? OrgCode { get; set; }
    public string? RemoteIp { get; set; }
    public DateTime? CreateDate { get; set; }
}
