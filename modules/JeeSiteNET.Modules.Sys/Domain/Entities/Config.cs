using JeeSiteNET.Core;

namespace JeeSiteNET.Modules.Sys.Domain.Entities;

/// <summary>
/// 系统配置项实体，以 Key-Value 方式存储全局/系统级参数（如站点名称、上传路径、开关位）。
/// </summary>
public class Config : DataEntity
{
    /// <summary>配置项 Key，全局唯一（如 sys.upload.maxSize）。</summary>
    public string ConfigKey { get; set; } = string.Empty;
    /// <summary>配置项显示名称。</summary>
    public string ConfigName { get; set; } = string.Empty;
    /// <summary>配置项值（字符串形式，读取时自行类型转换）。</summary>
    public string? ConfigValue { get; set; }
    /// <summary>是否系统内置配置：1=是（不可删除），0=否，默认 0。</summary>
    public string? IsSys { get; set; } = "0";
}
