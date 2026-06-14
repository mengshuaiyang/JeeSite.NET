namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 参数配置 DTO。
/// </summary>
public class ConfigDto
{
    /// <summary>
    /// 配置键。
    /// </summary>
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    /// 配置名称。
    /// </summary>
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    /// 配置值。
    /// </summary>
    public string? ConfigValue { get; set; }

    /// <summary>
    /// 是否系统级配置（1 是 / 0 否）。
    /// </summary>
    public string? IsSys { get; set; }

    /// <summary>
    /// 状态（0 正常 / 1 禁用）。
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 参数配置保存请求 DTO。
/// </summary>
public class ConfigSaveDto
{
    /// <summary>
    /// 配置键；空表示新建。
    /// </summary>
    public string? ConfigKey { get; set; }

    /// <summary>
    /// 配置名称。
    /// </summary>
    public string ConfigName { get; set; } = string.Empty;

    /// <summary>
    /// 配置值。
    /// </summary>
    public string? ConfigValue { get; set; }

    /// <summary>
    /// 是否系统级配置。
    /// </summary>
    public string? IsSys { get; set; }
}
