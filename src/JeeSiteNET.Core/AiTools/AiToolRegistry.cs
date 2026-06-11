using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.AiTools;

public class AiToolRegistry
{
    private readonly ConcurrentDictionary<string, IAiTool> _tools = new();
    private readonly IServiceProvider _serviceProvider;
    private bool _scanned;

    public AiToolRegistry(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void RegisterTool(IAiTool tool)
    {
        _tools[tool.Name] = tool;
    }

    public IAiTool? GetTool(string name)
    {
        EnsureScanned();
        return _tools.TryGetValue(name, out var tool) ? tool : null;
    }

    public IEnumerable<IAiTool> GetAllTools()
    {
        EnsureScanned();
        return _tools.Values;
    }

    public IEnumerable<(string Name, string Description, string? Category)> GetToolDefinitions()
    {
        EnsureScanned();
        foreach (var tool in _tools.Values)
        {
            var attr = tool.GetType().GetCustomAttribute<AiToolAttribute>();
            yield return (tool.Name, tool.Description, attr?.Category);
        }
    }

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

    public async Task<AiToolResult> ExecuteToolAsync(string toolName, AiToolContext context, CancellationToken ct = default)
    {
        var tool = GetTool(toolName);
        if (tool == null)
            return AiToolResult.Fail($"工具 '{toolName}' 不存在");

        context.ServiceProvider = _serviceProvider;
        return await tool.ExecuteAsync(context, ct);
    }

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
