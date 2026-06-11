using System.Text.Json;
using JeeSiteNET.Core.AiTools;

namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool("get_datetime", "获取当前日期和时间", "通用")]
public class DateTimeTool : IAiTool
{
    public string Name => "get_datetime";
    public string Description => "获取当前日期和时间信息";

    public Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default)
    {
        var format = context.GetParameter("format") ?? "full";
        var now = DateTime.Now;
        var utcNow = DateTime.UtcNow;

        var info = new Dictionary<string, object>
        {
            ["local_time"] = now.ToString("yyyy-MM-dd HH:mm:ss"),
            ["utc_time"] = utcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            ["date"] = now.ToString("yyyy-MM-dd"),
            ["time"] = now.ToString("HH:mm:ss"),
            ["weekday"] = now.DayOfWeek.ToString(),
            ["timezone"] = TimeZoneInfo.Local.DisplayName,
            ["timestamp"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        var result = format switch
        {
            "date" => now.ToString("yyyy-MM-dd"),
            "time" => now.ToString("HH:mm:ss"),
            "unix" => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
            "iso" => now.ToString("O"),
            _ => JsonSerializer.Serialize(info, new JsonSerializerOptions { WriteIndented = true })
        };

        return Task.FromResult(AiToolResult.Ok(result));
    }
}
