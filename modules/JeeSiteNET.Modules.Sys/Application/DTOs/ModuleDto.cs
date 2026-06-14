namespace JeeSiteNET.Modules.Sys.Application.DTOs;

/// <summary>
/// 模块/插件信息 DTO。
/// </summary>
public class ModuleDto
{
    /// <summary>
    /// 模块编码（主键）。
    /// </summary>
    public string ModuleCode { get; set; } = string.Empty;

    /// <summary>
    /// 模块名称。
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 模块版本号。
    /// </summary>
    public string? ModuleVersion { get; set; }

    /// <summary>
    /// 模块启动类/主程序集。
    /// </summary>
    public string? MainClass { get; set; }

    /// <summary>
    /// 是否启用（1 是 / 0 否）。
    /// </summary>
    public string? IsEnabled { get; set; }

    /// <summary>
    /// 状态。
    /// </summary>
    public string? Status { get; set; }
}

/// <summary>
/// 模块保存请求 DTO。
/// </summary>
public class ModuleSaveDto
{
    /// <summary>
    /// 模块编码；空表示新建。
    /// </summary>
    public string? ModuleCode { get; set; }

    /// <summary>
    /// 模块名称。
    /// </summary>
    public string ModuleName { get; set; } = string.Empty;

    /// <summary>
    /// 模块版本号。
    /// </summary>
    public string? ModuleVersion { get; set; }

    /// <summary>
    /// 主程序类/程序集。
    /// </summary>
    public string? MainClass { get; set; }

    /// <summary>
    /// 是否启用。
    /// </summary>
    public string? IsEnabled { get; set; }
}
