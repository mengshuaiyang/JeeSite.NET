namespace JeeSiteNET.Core.AiTools;

/// <summary>
/// AI 工具注解：标记某个类为可被 AI 工具框架识别的工具。
/// 工具注册器将读取此注解提取名称、描述与分类。
/// </summary>
/// <param name="name">工具唯一名称（英文/驼峰）</param>
/// <param name="description">面向 LLM 的工具功能描述（中文）</param>
/// <param name="category">可选分类标签，用于多工具提示时分组</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AiToolAttribute(string name, string description, string? category = null) : Attribute
{
    /// <summary>
    /// 工具名称（英文/驼峰，例如：StockQuoteTool）
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// 工具功能描述（提交给 LLM）
    /// </summary>
    public string Description { get; } = description;

    /// <summary>
    /// 工具分类标签（如：基础数据 / 技术分析 / 新闻检索）
    /// </summary>
    public string? Category { get; } = category;
}
