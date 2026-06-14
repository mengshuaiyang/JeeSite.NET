using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.AiTools;

/// <summary>
/// AI 工具注册器：负责扫描程序集、创建工具实例、按名称索引与执行调度。
/// 注册器使用 ConcurrentDictionary 作为存储，确保线程安全；
/// 首次调用 GetTool / GetAllTools 时懒扫描 AppDomain 中所有已加载程序集。
/// </summary>
public class AiToolRegistry
{
    /// <summary>
    /// 名称 -> 工具实例的线程安全字典
    /// </summary>
    private readonly ConcurrentDictionary<string, IAiTool> _tools = new();

    /// <summary>
    /// 依赖注入容器，用于 ActivatorUtilities 创建工具实例
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 是否已完成首次程序集扫描（双检锁）
    /// </summary>
    private bool _scanned;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider">应用的 IServiceProvider，用于解析工具的依赖项</param>
    public AiToolRegistry(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 手动注册单个工具实例（覆盖同名工具）
    /// </summary>
    /// <param name="tool">工具实例</param>
    public void RegisterTool(IAiTool tool)
    {
        _tools[tool.Name] = tool;
    }

    /// <summary>
    /// 按名称获取工具实例；尚未扫描时触发懒扫描
    /// </summary>
    /// <param name="name">工具名称（与 IAiTool.Name 完全匹配）</param>
    /// <returns>工具实例或 null</returns>
    public IAiTool? GetTool(string name)
    {
        EnsureScanned();
        return _tools.TryGetValue(name, out var tool) ? tool : null;
    }

    /// <summary>
    /// 获取所有已注册工具的枚举
    /// </summary>
    /// <returns>全部 IAiTool 集合</returns>
    public IEnumerable<IAiTool> GetAllTools()
    {
        EnsureScanned();
        return _tools.Values;
    }

    /// <summary>
    /// 获取工具元信息列表（名称 + 描述 + 分类），用于拼装 LLM 提示
    /// </summary>
    /// <returns>元信息元组集合</returns>
    public IEnumerable<(string Name, string Description, string? Category)> GetToolDefinitions()
    {
        EnsureScanned();
        foreach (var tool in _tools.Values)
        {
            var attr = tool.GetType().GetCustomAttribute<AiToolAttribute>();
            yield return (tool.Name, tool.Description, attr?.Category);
        }
    }

    /// <summary>
    /// 以提示友好的字符串形式返回工具清单（可直接放入 Prompt）
    /// </summary>
    /// <returns>工具清单字符串，每行一个工具</returns>
    public string GetToolListForPrompt()
    {
        EnsureScanned();
        var lines = new List<string>();
        foreach (var tool in _tools.Values)
        {
            var attr = tool.GetType().GetCustomAttribute<AiToolAttribute>();
            var desc = attr?.Description ?? tool.Description;
            var cat = attr?.Category;
            var prefix = cat != null ? $"[{cat}] " : "";
            lines.Add($"- {prefix}{tool.Name}: {desc}");
        }
        return string.Join("\n", lines);
    }

    /// <summary>
    /// 按名称异步调用工具：从字典获取实例、注入 IServiceProvider 并执行。
    /// </summary>
    /// <param name="toolName">工具名称</param>
    /// <param name="context">执行上下文（若未提供 ServiceProvider，将由注册器注入）</param>
    /// <param name="ct">取消令牌</param>
    /// <returns>工具执行结果；工具不存在时返回失败结果</returns>
    public async Task<AiToolResult> ExecuteToolAsync(string toolName, AiToolContext context, CancellationToken ct = default)
    {
        var tool = GetTool(toolName);
        if (tool == null)
            return AiToolResult.Fail($"工具 '{toolName}' 不存在");

        context.ServiceProvider = _serviceProvider;
        return await tool.ExecuteAsync(context, ct);
    }

    /// <summary>
    /// 扫描指定程序集中所有标记 AiToolAttribute 且实现 IAiTool 的类并实例化注册
    /// </summary>
    /// <param name="assembly">要扫描的程序集</param>
    public void ScanAndRegister(Assembly assembly)
    {
        var toolTypes = assembly.GetTypes()
            .Where(t => !t.IsAbstract && typeof(IAiTool).IsAssignableFrom(t) && t.GetCustomAttribute<AiToolAttribute>() != null);

        foreach (var type in toolTypes)
        {
            var tool = ActivatorUtilities.CreateInstance(_serviceProvider, type) as IAiTool;
            if (tool != null)
                RegisterTool(tool);
        }
    }

    /// <summary>
    /// 确保已完成首次扫描：懒加载 + 双检锁，避免并发重复扫描
    /// </summary>
    private void EnsureScanned()
    {
        if (_scanned) return;
        lock (_tools)
        {
            if (_scanned) return;
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                ScanAndRegister(asm);
            _scanned = true;
        }
    }
}
