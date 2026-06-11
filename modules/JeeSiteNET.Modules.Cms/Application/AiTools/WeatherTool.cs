using JeeSiteNET.Core.AiTools;

namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool("get_weather", "获取天气信息", "通用")]
public class WeatherTool : IAiTool
{
    public string Name => "get_weather";
    public string Description => "获取指定城市的当前天气信息（模拟数据）";

    public Task<AiToolResult> ExecuteAsync(AiToolContext context, CancellationToken cancellationToken = default)
    {
        var city = context.GetParameter("city") ?? "北京";
        var weatherData = new Dictionary<string, (string temp, string condition)>
        {
            ["北京"] = ("22°C", "晴"),
            ["上海"] = ("25°C", "多云"),
            ["广州"] = ("30°C", "阵雨"),
            ["深圳"] = ("29°C", "阴"),
            ["杭州"] = ("24°C", "小雨"),
            ["成都"] = ("26°C", "晴"),
            ["武汉"] = ("28°C", "多云"),
        };

        if (weatherData.TryGetValue(city, out var weather))
            return Task.FromResult(AiToolResult.Ok($"当前 {city} 天气: {weather.temp}, {weather.condition}"));

        return Task.FromResult(AiToolResult.Ok($"未找到 {city} 的天气数据"));
    }
}
