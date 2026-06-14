namespace JeeSiteNET.Core.Modules;

/// <summary>
/// 模块描述符接口：提供模块的基本元信息，供 ModuleLoader 进行依赖排序与注册
/// </summary>
public interface IModuleDescriptor
{
    /// <summary>
    /// 模块编码（唯一标识，用于依赖引用）
    /// </summary>
    string Code { get; }

    /// <summary>
    /// 模块显示名称
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 模块版本号（语义化版本，如 "1.0.0"）
    /// </summary>
    string Version { get; }

    /// <summary>
    /// 依赖的其他模块编码列表（需要先于本模块注册）
    /// </summary>
    string[] Dependencies { get; }

    /// <summary>
    /// 加载排序序号（值越小越先注册）
    /// </summary>
    int SortOrder { get; }
}

/// <summary>
/// 模块描述特性：以特性形式标注在 IModuleInstaller 实现类上，
/// 运行时由 ModuleLoader 读取并登记到模块列表
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ModuleDescriptionAttribute : Attribute, IModuleDescriptor
{
    /// <summary>
    /// 模块编码（唯一标识），默认值 string.Empty
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// 模块显示名称，默认值 string.Empty
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 模块版本号，默认值 "1.0.0"
    /// </summary>
    public string Version { get; set; } = "1.0.0";

    /// <summary>
    /// 依赖模块编码列表，默认值 []
    /// </summary>
    public string[] Dependencies { get; set; } = [];

    /// <summary>
    /// 加载排序序号，默认值 100
    /// </summary>
    public int SortOrder { get; set; } = 100;
}
