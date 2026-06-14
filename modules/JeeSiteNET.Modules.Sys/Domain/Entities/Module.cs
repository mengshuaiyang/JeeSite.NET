using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 业务模块实体，代表代码生成器、功能插件或独立业务模块。承载模块版本、生成目录、模板分类等配置。
/// </summary>
public class Module : DataEntity
{
    /// <summary>模块编码，业务主键（如 sys / cms / office）。</summary>
    public string ModuleCode { get; set; } = string.Empty;
    /// <summary>模块名称。</summary>
    public string ModuleName { get; set; } = string.Empty;
    /// <summary>模块描述。</summary>
    public string? Description { get; set; }
    /// <summary>模块版本号（如 1.0.0）。</summary>
    public string? ModuleVersion { get; set; }
    /// <summary>主入口类/主类名（插件模式使用）。</summary>
    public string? MainClass { get; set; }
    /// <summary>程序包名（NuGet / npm package 名称）。</summary>
    public string? PackageName { get; set; }
    /// <summary>模块排序（数值越小越靠前）。</summary>
    public decimal? ModuleSort { get; set; }
    /// <summary>当前部署版本。</summary>
    public string? CurrentVersion { get; set; }
    /// <summary>升级说明/变更日志。</summary>
    public string? UpgradeInfo { get; set; }
    /// <summary>后端代码生成基础目录。</summary>
    public string? GenBaseDir { get; set; }
    /// <summary>前端代码生成基础目录。</summary>
    public string? GenFrontDir { get; set; }
    /// <summary>代码生成模板分类：crud / tree / subTable 等。</summary>
    public string? TplCategory { get; set; }
    /// <summary>是否启用：1=启用，0=停用，默认 1。</summary>
    public string? IsEnabled { get; set; } = "1";
}
