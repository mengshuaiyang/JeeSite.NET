namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 操作日志/访问日志 DTO。
/// </summary>
public class LogDto
{
    /// <summary>
    /// 日志标识（主键）。
    /// </summary>
    public string LogId { get; set; } = string.Empty;

    /// <summary>
    /// 日志类型（access/error/audit 等）。
    /// </summary>
    public string? LogType { get; set; }

    /// <summary>
    /// 日志标题（通常为请求方法+路径）。
    /// </summary>
    public string? LogTitle { get; set; }

    /// <summary>
    /// 请求 URI。
    /// </summary>
    public string? RequestUri { get; set; }

    /// <summary>
    /// HTTP 请求方法（GET/POST/PUT/DELETE 等）。
    /// </summary>
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求执行耗时（毫秒）。
    /// </summary>
    public decimal? ExecuteTime { get; set; }

    /// <summary>
    /// 操作用户编码。
    /// </summary>
    public string? UserCode { get; set; }

    /// <summary>
    /// 操作用户姓名。
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 记录创建者姓名。
    /// </summary>
    public string? CreateByName { get; set; }

    /// <summary>
    /// 用户所属机构编码。
    /// </summary>
    public string? OrgCode { get; set; }

    /// <summary>
    /// 客户端远程 IP。
    /// </summary>
    public string? RemoteIp { get; set; }

    /// <summary>
    /// 记录创建时间。
    /// </summary>
    public DateTime? CreateDate { get; set; }
}
