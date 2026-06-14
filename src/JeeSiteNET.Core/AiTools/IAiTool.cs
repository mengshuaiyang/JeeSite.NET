namespace JeeSiteNET.Core.AiTools;

/// <summary>
/// AI 工具统一接口：所有可被 AI 调用的工具必须实现此接口。
/// ExecuteAsync 接收运行上下文并返回工具结果，注册器按类型扫描与反射。
/// </summary>
public interface IAiTool
{
    /// <summary>
    /// 工具名称（唯一标识）
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 工具功能描述（提交给 LLM，帮助模型理解何时调用）
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 异步执行工具逻辑
    /// </summary>
    /// <param name="context">执行上下文：包含用户输入、参数、服务提供者等</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>执行结果（成功/失败 + 数据）</returns>
    Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default);
}
