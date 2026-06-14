using System.Security.Claims;

namespace JeeSiteNET.Core.AiTools;

/// <summary>
/// AI 工具执行上下文：承载工具调用时的输入、参数、当前用户、服务提供者等信息。
/// 注册器在调用 ExecuteAsync 前会注入 ServiceProvider。
/// </summary>
public class AiToolContext
{
    /// <summary>
    /// 原始用户消息文本（可能由 LLM 解析后得到参数）
    /// </summary>
    public string UserMessage { get; set; } = string.Empty;

    /// <summary>
    /// 键值对参数表：LLM 解析工具调用参数后写入
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; } = new();

    /// <summary>
    /// 当前请求用户的 ClaimsPrincipal（可为 null）
    /// </summary>
    public ClaimsPrincipal? User { get; set; }

    /// <summary>
    /// 服务提供者：由注册器在 ExecuteToolAsync 时注入，供工具按需解析依赖
    /// </summary>
    public IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// 对话 ID：支持跨调用上下文关联
    /// </summary>
    public string? ConversationId { get; set; }

    /// <summary>
    /// 按名称读取参数；不存在时返回 null
    /// </summary>
    /// <param name="key">参数名</param>
    /// <returns>参数值或 null</returns>
    public string? GetParameter(string key) =>
        Parameters.TryGetValue(key, out var val) ? val : null;
}
