namespace JeeSiteNET.Core.AiTools;

/// <summary>
/// AI 工具执行结果：封装成功/失败标志、返回数据与错误信息。
/// 静态工厂方法 Ok / Fail 简化创建流程。
/// </summary>
public class AiToolResult
{
    /// <summary>
    /// 是否执行成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 失败时的错误描述；成功时通常为 null
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 工具返回的数据体（字符串，建议返回 JSON 或结构化文本供 LLM 直接消费）
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// 返回数据的内容类型（text / json / csv 等，便于 LLM 识别）
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// 创建成功结果
    /// </summary>
    /// <param name="data">返回数据（字符串）</param>
    /// <param name="contentType">内容类型标记</param>
    /// <returns>成功的 AiToolResult 实例</returns>
    public static AiToolResult Ok(string? data = null, string? contentType = "text")
        => new() { Success = true, Data = data, ContentType = contentType };

    /// <summary>
    /// 创建失败结果
    /// </summary>
    /// <param name="error">错误描述</param>
    /// <returns>失败的 AiToolResult 实例</returns>
    public static AiToolResult Fail(string error)
        => new() { Success = false, Error = error };
}
