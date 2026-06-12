<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# AI-Tools开发

> 自定义 AI 工具开发最佳实践：IAiTool 接口、工具元信息、参数校验、审计日志、权限控制。
>
> **适用角色**：后端开发人员、AI 应用开发者
> **阅读时间**：约 12 分钟
> **相关文档**：[20-AI智能问答](20-AI智能问答) · [21-MCP服务协议](21-MCP服务协议)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、AI Tools 框架概述](#一、ai-tools-框架概述)
- [二、AI Tool 的核心结构](#二、ai-tool-的核心结构)
- [三、工具注册与发现](#三、工具注册与发现)
- [四、核心基础设施类](#四、核心基础设施类)
  - [AiToolAttribute（标记类，定义工具元信息）](#aitoolattribute（标记类，定义工具元信息）)
  - [AiToolRegistry（工具注册中心）](#aitoolregistry（工具注册中心）)
  - [AiChatService（对话服务核心，与 LLM 交互并调用工具）](#aichatservice（对话服务核心，与-llm-交互并调用工具）)
- [五、工具开发的 10 条最佳实践](#五、工具开发的-10-条最佳实践)
- [六、一个完整的"天气查询"工具示例](#六、一个完整的天气查询工具示例)
- [七、工具测试](#七、工具测试)
- [八、工具清单（当前已内置）](#八、工具清单（当前已内置）)
- [九、新增工具的 5 步流程](#九、新增工具的-5-步流程)
- [十、常见坑位与避免方法](#十、常见坑位与避免方法)
- [十一、监控与运维](#十一、监控与运维)
- [十二、性能与成本预算建议](#十二、性能与成本预算建议)

---


## 一、AI Tools 框架概述

JeeSite.NET 的 AI Tools 框架为 LLM（大型语言模型）提供标准化的工具调用接口，遵循 JSON-RPC 2.0 over HTTP 的 MCP（Model Context Protocol）协议风格。

**核心思想**：
- **工具即服务**：每个 AI Tool 是一个独立的 Service，有清晰的输入 Schema 和输出格式
- **统一注册**：通过 `AiToolRegistry` 自动发现所有 `[AiTool]` 标记的工具
- **LLM 驱动**：LLM（DeepSeek / OpenAI / Ollama）根据工具描述和 Schema 动态选择并调用合适的工具
- **审计与限流**：每次调用自动记录审计日志，有用户级和全局速率限制

---

## 二、AI Tool 的核心结构

一个标准 AI Tool 的代码骨架：

```csharp
using JeeSiteNET.Core.Annotations; // AiToolAttribute
using JeeSiteNET.Core.Interfaces;   // IAiTool
using System.Text.Json.Serialization;

namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool(
    Name = "cms_search_article",
    Description = "根据关键词搜索 CMS 系统中的文章，返回匹配的文章列表。"
                + "适用于用户询问某个主题是否有相关文章、"
                + "或需要引用已发布文章的场景。",
    InputSchemaJson = @"
    {
        ""type"": ""object"",
        ""properties"": {
            ""keyword"": {
                ""type"": ""string"",
                ""description"": ""搜索关键词，如 系统管理、用户权限、教程""
            },
            ""category"": {
                ""type"": ""string"",
                ""description"": ""栏目代码或栏目名称，可选参数，用于缩小搜索范围""
            },
            ""limit"": {
                ""type"": ""integer"",
                ""default"": 5,
                ""description"": ""返回结果的最大数量""
            }
        },
        ""required"": [""keyword""]
    }")]
public class CmsSearchTool : IAiTool
{
    private readonly ArticleService _articleService;

    public CmsSearchTool(ArticleService articleService)
    {
        _articleService = articleService;
    }

    // 核心方法: 执行工具并返回 JSON 可序列化对象
    public async Task<object> ExecuteAsync(object input, CancellationToken ct)
    {
        var args = input as CmsSearchToolArgs
            ?? JsonSerializer.Deserialize<CmsSearchToolArgs>(input?.ToString() ?? "{}")
            ?? new CmsSearchToolArgs();

        // 1. 参数校验（必填 + 类型）
        if (string.IsNullOrWhiteSpace(args.Keyword))
            throw new ArgumentException("keyword 不能为空");
        if (args.Limit < 1 || args.Limit > 50)
            throw new ArgumentException("limit 必须在 1-50 之间");

        // 2. 调用业务服务
        var articles = await _articleService.SearchAsync(
            keyword: args.Keyword,
            categoryCode: args.Category,
            pageSize: args.Limit,
            pageIndex: 1,
            isPublished: true,
            cancellationToken: ct
        );

        // 3. 精简输出（只返回 LLM 需要的字段，节省 token）
        return new CmsSearchToolResult
        {
            Total = articles.TotalCount,
            Articles = articles.Items.Select(a => new ArticleSummary
            {
                Id = a.Id,
                Title = a.Title,
                Category = a.CategoryName,
                PublishDate = a.PublishDate?.ToString("yyyy-MM-dd"),
                Summary = Truncate(a.Content, 200) // 只返回前 200 字摘要
            }).ToList()
        };
    }

    // 工具输入参数（强类型，便于 JSON Schema 校验）
    public class CmsSearchToolArgs
    {
        [JsonPropertyName("keyword")] public string Keyword { get; set; } = "";
        [JsonPropertyName("category")] public string? Category { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; } = 5;
    }

    // 工具输出格式（供 LLM 消费的简洁格式）
    public class CmsSearchToolResult
    {
        [JsonPropertyName("total")] public int Total { get; set; }
        [JsonPropertyName("articles")] public List<ArticleSummary> Articles { get; set; } = new();
    }

    public class ArticleSummary
    {
        [JsonPropertyName("id")] public long Id { get; set; }
        [JsonPropertyName("title")] public string Title { get; set; } = "";
        [JsonPropertyName("category")] public string Category { get; set; } = "";
        [JsonPropertyName("publishDate")] public string? PublishDate { get; set; }
        [JsonPropertyName("summary")] public string Summary { get; set; } = "";
    }

    private static string Truncate(string text, int maxLen)
    {
        if (string.IsNullOrWhiteSpace(text)) return "";
        return text.Length > maxLen ? text.Substring(0, maxLen) + "..." : text;
    }
}
```

---

## 三、工具注册与发现

框架自动扫描所有引用的程序集，查找带有 `[AiTool]` 特性并实现 `IAiTool` 接口的类。

在 `Program.cs`（或 `Startup` 类）中注册：

```csharp
// 注册 AI Tools 框架（含所有模块中的工具自动发现）
builder.Services.AddAiTools(
    assembliesToScan: new[] { typeof(CmsModuleInstaller).Assembly, typeof(SysModuleInstaller).Assembly }
);
```

---

## 四、核心基础设施类

### AiToolAttribute（标记类，定义工具元信息）

```csharp
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AiToolAttribute : Attribute
{
    public string Name { get; }            // 工具名（唯一标识，建议: module_action_entity）
    public string Description { get; }     // 工具的自然语言描述（LLM 依据此描述决定何时调用）
    public string InputSchemaJson { get; } // JSON Schema，定义输入参数
    public int RateLimitPerMinute { get; set; } = 10; // 单用户每分钟限额
}
```

### AiToolRegistry（工具注册中心）

```csharp
public class AiToolRegistry
{
    // 获取所有已注册工具（供 MCP API /tools/list 返回）
    public IReadOnlyList<AiToolInfo> GetAllTools() { ... }

    // 根据工具名查找工具类型（供 MCP API /tools/call 调用）
    public AiToolInfo? FindByName(string toolName) { ... }

    // 创建工具实例（依赖注入支持，通过 IServiceProvider）
    public IAiTool? CreateInstance(string toolName, IServiceProvider sp) { ... }
}

// 工具信息（供 API 输出）
public class AiToolInfo
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public JsonDocument InputSchema { get; set; } = null!;
    public Type ImplementationType { get; set; } = null!;
}
```

### AiChatService（对话服务核心，与 LLM 交互并调用工具）

```csharp
public class AiChatService
{
    private readonly AiToolRegistry _registry;
    private readonly ILogger<AiChatService> _logger;
    private readonly ILlmProvider _llm;

    public async Task<ChatResponse> ChatAsync(
        string userMessage,
        string sessionId,
        string? userCode,
        List<string>? enabledToolNames = null,
        CancellationToken ct = default)
    {
        // 1. 从 Redis 读取会话历史（key: "ai:chat:history:{sessionId}"）
        var history = await _cache.GetAsync<List<ChatMessage>>(sessionId);

        // 2. 决定可用工具（如果指定了 enabledToolNames 则过滤）
        var availableTools = enabledToolNames != null && enabledToolNames.Any()
            ? enabledToolNames.Select(n => _registry.FindByName(n)!).Where(t => t != null).ToList()
            : _registry.GetAllTools();

        // 3. 将用户消息 + 工具列表发送给 LLM（使用 function calling / tool use）
        var llmResponse = await _llm.SendWithToolsAsync(
            userMessage,
            history,
            availableTools,
            ct
        );

        // 4. 检查 LLM 是否要求调用工具（tool_use）
        if (llmResponse.ToolCalls != null && llmResponse.ToolCalls.Count > 0)
        {
            // 遍历每一个 tool_call 并执行
            foreach (var call in llmResponse.ToolCalls)
            {
                var tool = _registry.FindByName(call.ToolName);
                if (tool == null) continue;

                // 4a. 限流检查（单用户每分钟不超过 RateLimitPerMinute）
                if (!await CheckRateLimit(userCode, tool))
                    throw new RateLimitExceededException($"工具 {tool.Name} 调用过于频繁");

                // 4b. 创建工具实例（通过 DI）
                using var scope = _serviceProvider.CreateScope();
                var instance = _registry.CreateInstance(call.ToolName, scope.ServiceProvider);
                if (instance == null) continue;

                // 4c. 执行并捕获异常（不要让异常传播到用户，记录日志即可）
                object? toolResult = null;
                try
                {
                    toolResult = await instance.ExecuteAsync(call.Input, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "AI Tool 执行失败: {Tool}", tool.Name);
                    toolResult = new { error = ex.Message };
                }

                // 4d. 将工具结果写入对话历史（供下一轮 LLM 决策参考）
                history.Add(new ChatMessage(Role.Tool,
                    JsonSerializer.Serialize(toolResult),
                    tool.Name));
            }

            // 5. 工具调用完成后，将结果再次发送给 LLM 生成最终回答
            llmResponse = await _llm.SendWithToolsAsync(userMessage, history, availableTools, ct);
        }

        // 6. 保存会话历史（保留最近 20 条，TTL 24 小时）
        history.Add(new ChatMessage(Role.User, userMessage));
        history.Add(new ChatMessage(Role.Assistant, llmResponse.Text));
        await _cache.SetAsync(sessionId, history.TakeLast(20).ToList(), TimeSpan.FromHours(24));

        // 7. 记录审计日志（谁、何时、调用了哪些工具、消耗多少 token）
        await _auditService.LogAiChatAsync(userCode, userMessage, llmResponse.Text,
            llmResponse.TokenUsage, llmResponse.ToolCalls?.Select(t => t.ToolName).ToList());

        return new ChatResponse
        {
            Answer = llmResponse.Text,
            ToolCalls = llmResponse.ToolCalls,
            TokenUsage = llmResponse.TokenUsage
        };
    }
}
```

---

## 五、工具开发的 10 条最佳实践

| # | 实践 | 说明 |
|---|------|------|
| 1 | **有意义的工具名** | 命名约定: `模块_动作_实体`，如 `cms_search_article`、`sys_get_user_info`。LLM 根据名称推断用途 |
| 2 | **清晰的自然语言描述** | `Description` 字段是 LLM 选择工具的最关键依据。请用 2-3 句话明确描述工具用途、输入、输出和适用场景 |
| 3 | **完整的 JSON Schema** | `InputSchemaJson` 必须完整描述每个字段的类型、含义、是否必填、默认值、枚举值等 |
| 4 | **强类型输入 / 输出** | 使用 `class ToolArgs` / `class ToolResult` 而非动态对象，便于序列化 / 校验 / 测试 |
| 5 | **Token 经济** | 工具输出只返回 LLM 需要的最小信息。如: 搜索文章不要返回全文，只返回标题+200字摘要 |
| 6 | **参数严格校验** | 在 `ExecuteAsync` 开头检查所有必填字段、类型范围、枚举值。抛出有意义的 `ArgumentException` |
| 7 | **幂等 + 可重入** | 工具应可以被多次调用而不产生副作用（避免"创建用户"、"发送消息"这类不可回滚的操作） |
| 8 | **异步 + 可取消** | 支持 `CancellationToken`，在长耗时操作（如 Elasticsearch 查询、数据库查询）中检查取消标记 |
| 9 | **异常不直接暴露** | 捕获异常并记录日志，返回 `{ error: "错误描述" }` 格式，让 LLM 自行处理而不是抛给用户 |
| 10 | **每个工具配一个单元测试** | 验证: (a) 空输入 → 异常；(b) 合法输入 → 预期输出；(c) 依赖服务可用 → Mock 后验证调用路径 |

---

## 六、一个完整的"天气查询"工具示例

这是一个调用外部 API 的工具，展示如何处理外部依赖、错误重试和限流：

```csharp
using JeeSiteNET.Core.Annotations;
using JeeSiteNET.Core.Interfaces;

namespace JeeSiteNET.Modules.Cms.Application.AiTools;

[AiTool(
    Name = "cms_get_weather",
    Description = "查询指定城市当前的天气信息，包括温度、天气现象、湿度和风速。"
                + "适用于用户问天气、出门建议、气候相关的查询。"
                + "输入参数: city（城市中文名或拼音，必填），unit（温度单位，可选，C/F，默认 C）",
    InputSchemaJson = @"{
        ""type"": ""object"",
        ""properties"": {
            ""city"": {
                ""type"": ""string"",
                ""description"": ""城市名，如 北京、上海、Guangzhou""
            },
            ""unit"": {
                ""type"": ""string"",
                ""enum"": [""C"", ""F""],
                ""default"": ""C"",
                ""description"": ""温度单位，C=摄氏度，F=华氏度""
            }
        },
        ""required"": [""city""]
    }",
    RateLimitPerMinute = 30  // 免费天气 API 通常有 QPS 限制
)]
public class WeatherTool : IAiTool
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<WeatherTool> _logger;

    // 使用 IHttpClientFactory 管理 HTTP 客户端（自动连接池复用）
    public WeatherTool(IHttpClientFactory httpClientFactory, ILogger<WeatherTool> logger)
    {
        _httpClient = httpClientFactory.CreateClient("WeatherAPI");
        _httpClient.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
        _httpClient.Timeout = TimeSpan.FromSeconds(5);
        _logger = logger;
    }

    public async Task<object> ExecuteAsync(object input, CancellationToken ct)
    {
        var args = JsonSerializer.Deserialize<WeatherToolArgs>(input?.ToString() ?? "{}")
            ?? new WeatherToolArgs();

        if (string.IsNullOrWhiteSpace(args.City))
            throw new ArgumentException("city 参数不能为空");

        try
        {
            // 1. 将城市名映射为经纬度（简化示例，实际可对接地理编码 API）
            var (lat, lon) = CityToLatLon(args.City);

            // 2. 调用天气 API（含重试策略）
            var url = $"forecast?latitude={lat}&longitude={lon}&current=temperature_2m,weather_code,wind_speed_10m&timezone=auto";
            var response = await RetryAsync(async () =>
                await _httpClient.GetFromJsonAsync<WeatherApiResponse>(url, ct),
                maxRetry: 3, initialDelayMs: 200, ct
            );

            // 3. 格式转换
            var temp = args.Unit == "F"
                ? Math.Round(response.Current.Temperature2m * 9 / 5 + 32, 1)
                : Math.Round(response.Current.Temperature2m, 1);

            return new
            {
                city = args.City,
                temperature = temp,
                unit = args.Unit,
                weather = WeatherCodeToText(response.Current.WeatherCode),
                windSpeedKmh = Math.Round(response.Current.WindSpeed10m, 1),
                updatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "天气 API 调用失败，城市: {City}", args.City);
            return new { error = $"暂时无法查询 {args.City} 的天气信息，请稍后再试" };
        }
        catch (TaskCanceledException)
        {
            return new { error = "天气查询超时，请稍后再试" };
        }
    }

    // -- 工具方法 --
    private static (double lat, double lon) CityToLatLon(string city) => city switch
    {
        "北京" or "Beijing" => (39.9042, 116.4074),
        "上海" or "Shanghai" => (31.2304, 121.4737),
        "广州" or "Guangzhou" => (23.1291, 113.2644),
        _ => (31.2304, 121.4737) // 默认上海
    };

    private static string WeatherCodeToText(int code) => code switch
    {
        0 => "晴天",
        1 or 2 or 3 => "多云",
        45 or 48 => "有雾",
        51 or 53 or 55 => "小雨",
        61 or 63 or 65 => "中雨",
        71 or 73 or 75 => "小雪",
        80 or 81 or 82 => "阵雨",
        95 or 96 or 99 => "雷阵雨",
        _ => "其他天气"
    };

    private static async Task<T> RetryAsync<T>(Func<Task<T>> func, int maxRetry, int initialDelayMs, CancellationToken ct)
    {
        for (int i = 0; i < maxRetry; i++)
        {
            try { return await func(); }
            catch when (i < maxRetry - 1) { await Task.Delay(initialDelayMs * (i + 1), ct); }
        }
        throw new InvalidOperationException("重试次数已用尽");
    }

    // -- 输入参数类 --
    public class WeatherToolArgs { public string City { get; set; } = ""; public string Unit { get; set; } = "C"; }

    // -- 外部 API 响应模型 --
    public class WeatherApiResponse
    {
        public CurrentWeather Current { get; set; } = new();
        public class CurrentWeather
        {
            [JsonPropertyName("temperature_2m")] public double Temperature2m { get; set; }
            [JsonPropertyName("weather_code")] public int WeatherCode { get; set; }
            [JsonPropertyName("wind_speed_10m")] public double WindSpeed10m { get; set; }
        }
    }
}
```

---

## 七、工具测试

每个 AI Tool 都应配备对应的单元测试（使用 xUnit + NSubstitute）：

```csharp
using Xunit;
using FluentAssertions;
using NSubstitute;

namespace JeeSiteNET.Tests.Modules.Cms;

public class CmsSearchToolTests
{
    [Fact]
    public async Task ExecuteAsync_ValidKeyword_ReturnsResults()
    {
        // Arrange
        var mockArticleService = Substitute.For<ArticleService>(null!, null!, null!);
        mockArticleService.SearchAsync("用户管理", Arg.Any<string>(), 5, 1, true, Arg.Any<CancellationToken>())
            .Returns(new PagedResult<Article>
            {
                TotalCount = 2,
                Items = new List<Article> {
                    new() { Id = 1, Title = "用户管理手册", Content = "..." },
                    new() { Id = 2, Title = "权限设置指南", Content = "..." }
                }
            });
        var tool = new CmsSearchTool(mockArticleService);

        // Act
        var result = await tool.ExecuteAsync(
            new { keyword = "用户管理", limit = 5 },
            CancellationToken.None
        );

        // Assert
        var json = JsonSerializer.Serialize(result);
        json.Should().Contain("用户管理手册");
        json.Should().Contain("total");
    }

    [Fact]
    public async Task ExecuteAsync_EmptyKeyword_ThrowsArgumentException()
    {
        var mock = Substitute.For<ArticleService>(null!, null!, null!);
        var tool = new CmsSearchTool(mock);
        Func<Task> act = async () => await tool.ExecuteAsync(new { keyword = "" }, CancellationToken.None);
        await act.Should().ThrowAsync<ArgumentException>();
    }
}
```

---

## 八、工具清单（当前已内置）

| 工具名 | 所属模块 | 功能 | 调用速率 |
|--------|---------|------|---------|
| `cms_search_article` | CMS | 按关键词搜索文章 | 60 次 / 分钟 |
| `cms_get_weather` | CMS | 查询指定城市天气 | 30 次 / 分钟 |
| `cms_datetime` | CMS | 获取当前日期时间和时区信息 | 不限 |
| `sys_get_user_info` | Sys | 获取当前登录用户信息 | 30 次 / 分钟 |
| `sys_get_dict_data` | Sys | 获取字典数据枚举值 | 60 次 / 分钟 |
| `sys_get_cache_stats` | Sys | 获取缓存命中率与统计信息 | 10 次 / 分钟（管理员专用） |

---

## 九、新增工具的 5 步流程

```
步骤 1: 在对应模块的 Application/AiTools/ 目录下创建类（如 MyNewTool.cs）
        └── 继承 IAiTool，标注 [AiTool] 特性，定义 Name / Description / InputSchemaJson

步骤 2: 实现 ExecuteAsync(object input, CancellationToken ct)
        └── 参数校验 → 调用业务服务 → 返回 JSON 可序列化对象

步骤 3: 添加单元测试（MyNewToolTests.cs）
        └── 至少测试: 合法输入 / 空输入 / 异常场景

步骤 4: 在对应模块的 ModuleInstaller.cs 中注册
        └── services.AddTransient<IAiTool, MyNewTool>();

步骤 5: 验证注册成功（开发环境自测）
        └── GET /api/v1/sys/mcp/tools 应包含新工具
        └── 使用 Swagger → /api/v1/cms/ai-chat 测试调用
```

---

## 十、常见坑位与避免方法

| 问题 | 现象 | 解决办法 |
|------|------|---------|
| 工具未被 LLM 选中 | 用户提问明明相关，但 LLM 总是直接回答不调用工具 | 检查 Description 是否清晰描述场景和适用条件；检查 JSON Schema 是否完整描述参数 |
| Token 超限 | 调用工具后对话总 token 超过模型上限（如 4K / 8K / 128K） | 每个工具输出只返回必要字段，列表截断为 Top N；压缩 JSON 输出 |
| 参数解析失败 | 工具抛出 `JsonException`，LLM 构造的参数 JSON 不合法 | 增加容错: `JsonSerializer.Deserialize` 带 `JsonSerializerDefaults.Web`，在 try-catch 中降级处理 |
| 循环调用工具 | LLM 反复调用同一工具，每次返回略有不同，永不结束 | 设置最大工具调用次数（如每轮对话最多调用 5 次工具） |
| 敏感信息泄露 | 工具返回的数据包含手机号 / 邮箱等 PII | 在 `ExecuteAsync` 返回前做脱敏处理；对敏感工具添加权限校验 |
| 工具调用超时 | 外部 API 慢或数据库慢查询 | 设置合理的 Timeout（HttpClient 5s）；异步取消（CancellationToken）；记录错误日志并告知用户稍后重试 |
| 测试环境不稳定 | 单元测试依赖真实网络 / 数据库 | 使用 NSubstitute / Moq Mock 掉外部依赖，只测工具自身逻辑 |

---

## 十一、监控与运维

- **审计日志**：每次 AI 对话（含工具调用）都记录到 `sys_audit_log` 表，含 userCode / 工具名 / 响应耗时 / token 数
- **Token 消耗统计**：按日 / 周 / 月统计 Token 消耗量和估算费用
- **命中率监控**：各工具被调用次数 vs 返回有效结果比例，低命中工具需优化 Description
- **错误率监控**：工具执行失败率 > 5% 触发告警
- **速率限制**：用户级（每分钟 N 次）+ 全局级（每日 M 次）双层限流

---

## 十二、性能与成本预算建议

| 项目 | 月用量（1000 DAU） | 月费用估算（人民币） |
|------|-----------------|-------------------|
| 对话消息 | ~30,000 条 | ~¥150-500（取决于模型） |
| 工具调用 | ~15,000 次 | ¥0（内部服务成本忽略） |
| 外部 API | ~5,000 次 | ~¥50-200（天气 / 地图等付费 API） |
| 向量索引存储 | ~1 GB | ~¥50（云存储） |
| **总计** | | **¥250-850 / 月** |

**建议**：
- 开发 / 测试环境使用便宜模型（如 deepseek-chat v3），生产环境按需切换
- 通过缓存重复问题（相同问题 24 小时内直接返回上次答案）可降低 30-50% Token 消耗
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---

## 💡 快速参考

| 项目 | 关键信息 |
|------|---------|
| **文档** | AI-Tools开发 |
| **最后更新** | 2026-06-13 |
| **相关文档** | [20-AI智能问答](20-AI智能问答) · [21-MCP服务协议](21-MCP服务协议) |

---

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>