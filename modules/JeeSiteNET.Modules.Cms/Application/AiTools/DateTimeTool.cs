    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 JeeSiteNET.Core.AiTools 命名空间
// 引入命名空间：JeeSiteNET.Core.AiTools
using JeeSiteNET.Core.AiTools;

// 定义 JeeSiteNET.Modules.Cms.Application.AiTools 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms.Application.AiTools
namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool("get_datetime", "获取当前日期和时间", "通用")]
// 定义class DateTimeTool
// 定义类：DateTimeTool
public class DateTimeTool : IAiTool
{
    public string Name => "get_datetime";
    public string Description => "获取当前日期和时间信息";

    // 方法 ExecuteAsync
    // 方法：ExecuteAsync
    public Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default)
    {
        // 声明并初始化变量：format
        var format = context.GetParameter("format") ?? "full";
        // 声明并初始化变量：now
        var now = DateTime.Now;
        // 声明并初始化变量：utcNow
        var utcNow = DateTime.UtcNow;

        // 创建 Dictionary实例并赋给 info
        var info = new Dictionary<string, object>
        {
            // 调用 ToString
            ["local_time"] = now.ToString("yyyy-MM-dd HH:mm:ss"),
            // 调用 ToString
            ["utc_time"] = utcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            // 调用 ToString
            ["date"] = now.ToString("yyyy-MM-dd"),
            // 调用 ToString
            ["time"] = now.ToString("HH:mm:ss"),
            // 调用 ToString
            ["weekday"] = now.DayOfWeek.ToString(),
            ["timezone"] = TimeZoneInfo.Local.DisplayName,
            ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        // 声明并初始化变量：result
        var result = format switch
        {
            // 调用 ToString
            "date" => now.ToString("yyyy-MM-dd"),
            // 调用 ToString
            "time" => now.ToString("HH:mm:ss"),
            // 调用 ToString
            "unix" => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            // 调用 ToString
            "iso" => now.ToString("O"),
            // JSON 序列化
            _ => JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true })
        };

        // return 返回结果
        return Task.FromResult(AiToolResult.Ok(result));
    }
}
