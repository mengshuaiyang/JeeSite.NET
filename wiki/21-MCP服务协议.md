<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---

# 21 MCP服务协议

> JeeSite.NET 作为 MCP Server 暴露内部业务能力供 LLM 客户端调用，统一 JSON-RPC 2.0 协议。
>
> **适用角色**：架构师、AI 应用开发者
> **阅读时间**：约 10 分钟
> **相关文档**：[20-AI智能问答](20-AI智能问答) · [26-AI-Tools开发](26-AI-Tools开发)
> 最后更新: 2026-06-13

---

## 📋 目录

- [一、MCP 简介](#一、mcp-简介)
- [二、协议约定](#二、协议约定)
  - [端点与鉴权](#端点与鉴权)
  - [JSON-RPC 2.0 请求格式](#json-rpc-20-请求格式)
  - [JSON-RPC 2.0 响应格式（成功）](#json-rpc-20-响应格式（成功）)
  - [JSON-RPC 2.0 响应格式（失败）](#json-rpc-20-响应格式（失败）)
- [三、核心方法](#三、核心方法)
  - [3.1 tools/list — 获取所有工具](#31-tools-list-—-获取所有工具)
  - [3.2 tools/call — 调用指定工具](#32-tools-call-—-调用指定工具)
  - [3.3 session/list / session/create](#33-session-list-session-create)
  - [3.4 user/profile](#34-user-profile)
  - [3.5 system/health](#35-system-health)
- [四、MCP 工具映射表](#四、mcp-工具映射表)
- [五、核心服务：McpService](#五、核心服务：mcpservice)
  - [主要方法](#主要方法)
  - [审计记录](#审计记录)
- [六、控制器端点](#六、控制器端点)
  - [McpController](#mcpcontroller)
  - [典型调用流程](#典型调用流程)
  - [调试点](#调试点)
- [七、自定义工具扩展步骤](#七、自定义工具扩展步骤)
  - [步骤 1：实现 IAiTool 接口并添加注解](#步骤-1：实现-iaitool-接口并添加注解)
  - [步骤 2：注册到 DI](#步骤-2：注册到-di)
  - [步骤 3：自动发现](#步骤-3：自动发现)
  - [验证](#验证)
- [八、安全注意事项](#八、安全注意事项)
  - [8.1 权限校验](#81-权限校验)
  - [8.2 参数校验](#82-参数校验)
  - [8.3 速率限制](#83-速率限制)
  - [8.4 敏感操作的二次确认](#84-敏感操作的二次确认)
  - [8.5 审计日志](#85-审计日志)
  - [8.6 网络隔离建议](#86-网络隔离建议)

---


> MCP = Model Context Protocol，由 Anthropic 提出。JeeSite.NET 作为 MCP Server，通过标准化 JSON-RPC 2.0 协议暴露业务 API 给 LLM 调用。

---

## 一、MCP 简介

MCP 定义了一套 LLM 与外部工具交互的标准协议。JeeSite.NET 将内部业务能力（CMS 文章搜索、用户管理、任务调度、工作流等）以 MCP Server 的形式暴露，供 LLM 客户端（如 Claude Code、Cursor、自定义 AI Assistant）调用。

**核心价值**:

- **统一协议**：所有业务工具以 JSON-RPC 2.0 暴露，LLM 只需一种接入方式。
- **权限隔离**：通过 `sys:mcp:invoke` 权限 + Bearer Token 严格控制调用者。
- **可审计**：每次工具调用写入 `sys_mcp_log` 表，可追溯、可回放。
- **可扩展**：新增工具仅需实现 `IAiTool` 接口并添加 `[AiTool]` 注解，无需改协议层。

**协议栈**:

```
┌─────────────────────────────────────────────────────┐
│ LLM Client (Claude Code / Cursor / 自定义客户端)    │
└────────────────┬────────────────────────────────────┘
                 │ POST /api/v1/sys/mcp/jsonrpc
                 │ Content-Type: application/json
                 │ Authorization: Bearer <token>
                 ▼
┌─────────────────────────────────────────────────────┐
│ JeeSite.NET MCP Server (McpController)              │
│  ├── JSON-RPC 2.0 解析与响应封装                    │
│  ├── 权限校验（sys:mcp:invoke）                     │
│  ├── 工具路由：tools/list、tools/call、session/*    │
│  ├── 参数校验 + 类型安全                            │
│  └── 审计日志记录（sys_mcp_log）                    │
└────────────────┬────────────────────────────────────┘
                 ▼
        ┌────────┴────────┐
        │  AiToolRegistry │  ← 反射扫描所有 IAiTool
        └────────┬────────┘
                 ▼
    ┌───────────────────────────┐
    │ ArticleService / User...  │  ← 实际业务服务
    └───────────────────────────┘
```

---

## 二、协议约定

### 端点与鉴权

| 项目 | 值 |
|------|-----|
| HTTP Method | `POST` |
| Path | `/api/v1/sys/mcp/jsonrpc` |
| Content-Type | `application/json` |
| 鉴权 | `Authorization: Bearer <jwt-token>` |
| 权限要求 | 当前用户必须拥有 `sys:mcp:invoke` 权限标识 |

### JSON-RPC 2.0 请求格式

```json
{
  "jsonrpc": "2.0",
  "method": "tools/list",
  "params": {
    "limit": 50
  },
  "id": "req-001"
}
```

| 字段 | 类型 | 必选 | 说明 |
|------|------|------|------|
| `jsonrpc` | string | ✅ | 固定值 `"2.0"` |
| `method` | string | ✅ | 调用方法名，见第三节 |
| `params` | object | ⬜ | 方法参数（具体字段取决于 method） |
| `id` | string/number | ⬜ | 请求 ID，用于匹配响应。notification 可省略 |

### JSON-RPC 2.0 响应格式（成功）

```json
{
  "jsonrpc": "2.0",
  "result": {
    "tools": [
      { "name": "cms.search_article", "description": "搜索 CMS 文章" }
    ]
  },
  "id": "req-001"
}
```

### JSON-RPC 2.0 响应格式（失败）

```json
{
  "jsonrpc": "2.0",
  "error": {
    "code": -32601,
    "message": "Method not found",
    "data": { "method": "tools/unknown" }
  },
  "id": "req-001"
}
```

**标准错误码**:

| Code | 说明 |
|------|------|
| `-32700` | Parse error（JSON 解析失败） |
| `-32600` | Invalid Request（请求格式不合法） |
| `-32601` | Method not found（方法不存在） |
| `-32602` | Invalid params（参数不合法） |
| `-32603` | Internal error（服务器内部错误） |
| `-32001` | Permission denied（无 `sys:mcp:invoke` 权限） |
| `-32002` | Rate limit exceeded（超出调用频率） |

---

## 三、核心方法

| Method | 说明 |
|--------|------|
| `tools/list` | 列出 JeeSite.NET 暴露的所有 AI 工具 |
| `tools/call` | 调用指定工具（需提供 `name` 和 `params`） |
| `session/list` | 列出当前用户的历史会话 |
| `session/create` | 创建新会话 |
| `user/profile` | 获取当前用户资料 |
| `system/health` | 系统健康检查 |

### 3.1 tools/list — 获取所有工具

**请求**:

```json
{
  "jsonrpc": "2.0",
  "method": "tools/list",
  "params": { "limit": 50, "offset": 0 },
  "id": "1"
}
```

**响应**:

```json
{
  "jsonrpc": "2.0",
  "result": {
    "tools": [
      {
        "name": "cms.search_article",
        "description": "按关键词搜索 CMS 文章，返回最相关的 N 篇",
        "inputSchema": {
          "type": "object",
          "properties": {
            "q":        { "type": "string", "description": "搜索关键词" },
            "category": { "type": "string", "description": "栏目代码（可选）" },
            "limit":    { "type": "integer", "description": "返回数量，默认 10" }
          },
          "required": ["q"]
        }
      }
    ],
    "total": 8
  },
  "id": "1"
}
```

### 3.2 tools/call — 调用指定工具

**请求**:

```json
{
  "jsonrpc": "2.0",
  "method": "tools/call",
  "params": {
    "name": "cms.search_article",
    "arguments": {
      "q": "密码重置",
      "limit": 5
    }
  },
  "id": "2"
}
```

**响应**:

```json
{
  "jsonrpc": "2.0",
  "result": {
    "toolName": "cms.search_article",
    "content": "[\n  {\"title\": \"忘记密码怎么办\", \"url\": \"/article/a001\"},\n  {\"title\": \"用户管理手册\", \"url\": \"/article/a002\"}\n]",
    "contentType": "application/json"
  },
  "id": "2"
}
```

### 3.3 session/list / session/create

| 方法 | params | 说明 |
|------|--------|------|
| `session/list` | `{ "limit": 20 }` | 当前用户的会话列表 |
| `session/create` | `{ "title": "故障排查" }` | 创建新会话，返回 `sessionId` |

### 3.4 user/profile

**请求**: `{"jsonrpc":"2.0","method":"user/profile","id":"3"}`

**响应**:

```json
{
  "jsonrpc": "2.0",
  "result": {
    "userCode": "admin",
    "userName": "系统管理员",
    "roles": ["超级管理员"],
    "corpCode": "JS0001"
  },
  "id": "3"
}
```

### 3.5 system/health

**请求**: `{"jsonrpc":"2.0","method":"system/health","id":"4"}`

**响应**:

```json
{
  "jsonrpc": "2.0",
  "result": {
    "status": "healthy",
    "version": "1.0.0",
    "uptimeSeconds": 86412,
    "components": {
      "postgres": "healthy",
      "redis": "healthy",
      "elasticsearch": "healthy"
    }
  },
  "id": "4"
}
```

---

## 四、MCP 工具映射表

以下是系统内置的工具清单。每个工具都对应一个业务服务。

| Tool Name | 描述 | 参数 | 对应服务 |
|-----------|------|------|---------|
| `cms.search_article` | 搜索 CMS 文章 | `q`(关键词), `category`, `limit` | ArticleService |
| `cms.get_article` | 获取文章详情 | `articleId` | ArticleService |
| `sys.get_user_info` | 获取当前用户信息 | 无 | UserService |
| `sys.send_message` | 发送站内消息 | `receiverCode`, `title`, `content` | MsgService |
| `sys.create_todo` | 创建待办 | `title`, `priority`, `dueDate` | DashboardService |
| `tasks.list_jobs` | 查看任务列表 | `status` | TaskJobService |
| `tasks.run_job` | 立即执行任务 | `jobId` | TaskJobService |
| `sys.list_permissions` | 查看当前用户权限 | 无 | AuthService |
| `sys.audit_log` | 查询最近审计日志 | `limit` | LogService |

> **自定义工具的新增方式**：见第七节。

---

## 五、核心服务：McpService

位置：`modules/JeeSiteNET.Modules.Sys/Services/McpService.cs`

### 主要方法

| 方法 | 说明 |
|------|------|
| `ListToolsAsync()` | 从 `AiToolRegistry` 反射获取所有工具元信息（name/description/schema） |
| `CallToolAsync(name, params)` | 参数校验 → 解析 → 调用对应 `IAiTool.ExecuteAsync` → 统一 JSON 输出 |
| `ListSessionsAsync(userCode, limit)` | 列出用户会话 |
| `CreateSessionAsync(userCode, title)` | 创建新会话 |
| `GetUserProfileAsync(userCode)` | 获取用户资料 |
| `HealthCheckAsync()` | 检查数据库/Redis/ES 状态 |

### 审计记录

每次 `tools/call` 调用后，`McpService` 会异步写入 `sys_mcp_log` 表，记录：

- `user_code`、`tool_name`、`params_json`
- `result_preview`（结果前 500 字符）、`call_duration_ms`
- `call_date`、`ip_address`、`user_agent`

---

## 六、控制器端点

### McpController

位置：`modules/JeeSiteNET.Modules.Sys/Controllers/McpController.cs`

| HTTP | 路由 | 说明 |
|------|------|------|
| POST | `/api/v1/sys/mcp/jsonrpc` | JSON-RPC 2.0 统一入口（主要端点） |
| GET | `/api/v1/sys/mcp/tools` | 工具列表（浏览器调试用，返回 HTML/JSON） |
| GET | `/api/v1/sys/mcp/health` | 健康检查（公开，可用于监控） |

### 典型调用流程

```
1. 客户端先通过 tools/list 拿到所有工具元信息
2. LLM 根据工具 description + inputSchema 决定是否调用
3. LLM 生成 tools/call 请求，携带 name + arguments
4. McpController → McpService → 对应 IAiTool → 业务服务
5. 返回 JSON-RPC 响应，LLM 再结合结果回答用户
```

### 调试点

- `/api/v1/sys/mcp/tools`：浏览器直接打开可查看工具清单（用于开发调试）。
- `/api/v1/sys/mcp/health`：Nagios / Prometheus 可 ping 此端点。

---

## 七、自定义工具扩展步骤

> 目标：新增一个工具 `cms.monthly_stats`，返回本月 CMS 文章统计。

### 步骤 1：实现 `IAiTool` 接口并添加注解

```csharp
[AiTool(
    name: "cms.monthly_stats",
    description: "获取本月 CMS 文章发布统计，包括发布数量、栏目分布、热门文章等",
    InputSchema = """
    {
      "type": "object",
      "properties": {
        "category": { "type": "string", "description": "栏目代码，留空表示全部栏目" },
        "days":     { "type": "integer", "description": "统计最近多少天，默认 30" }
      }
    }
    """)]
public class CmsMonthlyStatsTool : IAiTool
{
    private readonly IArticleService _articleService;

    public CmsMonthlyStatsTool(IArticleService articleService)
    {
        _articleService = articleService;
    }

    public async Task<string> ExecuteAsync(Dictionary<string, object> parameters)
    {
        var category = parameters.GetValueOrDefault("category")?.ToString();
        var daysRaw  = parameters.GetValueOrDefault("days")?.ToString();
        var days = int.TryParse(daysRaw, out var d) ? d : 30;

        var stats = await _articleService.GetMonthlyStatsAsync(category, days);
        return JsonSerializer.Serialize(stats, JsonOptions.Default);
    }
}
```

### 步骤 2：注册到 DI

在 `Program.cs` 或模块初始化类中：

```csharp
services.AddScoped<IAiTool, CmsMonthlyStatsTool>();
```

### 步骤 3：自动发现

`AiToolRegistry` 在启动时扫描所有注册的 `IAiTool` 实现类，读取 `[AiTool]` 元信息。调用 `tools/list` 时即可返回新工具；调用 `tools/call` 时按 `name` 路由到对应实例。

### 验证

启动后访问：`GET /api/v1/sys/mcp/tools`

应能在列表中看到 `cms.monthly_stats`。

---

## 八、安全注意事项

### 8.1 权限校验

- 所有 MCP 调用必须携带有效的 **Bearer Token**（JWT）。
- Token 对应用户必须拥有 **`sys:mcp:invoke`** 权限标识。
- 若 Token 无效或权限不足，返回 `{"code": -32001, "message": "Permission denied"}`。

### 8.2 参数校验

- **类型校验**：`arguments` 中每个字段按 `inputSchema` 声明的类型检查（string / integer / boolean / array）。
- **长度限制**：
  - 字符串最大 **4096 字符**。
  - 整数在 `int.MinValue ~ int.MaxValue` 之间做范围保护。
  - 数组最多 **100 项**。
- **特殊字符**：SQL 注入 / XSS 防护由底层 Service 处理，工具层只做长度和类型。
- **未知字段**：对 `arguments` 中 schema 未声明的字段一律忽略。

### 8.3 速率限制

| 维度 | 默认限制 | 说明 |
|------|----------|------|
| 每用户每分钟 | **10 次** tools/call | 超出返回 `-32002` |
| 每用户每小时 | **200 次** | |
| 全局每分钟 | **500 次** | |
| 全局每日 | **10,000 次** | |

> 可在 `appsettings.json` → `Mcp:RateLimit:*` 中调整。

### 8.4 敏感操作的二次确认

以下操作工具**不直接暴露**为默认可用，需在管理界面显式启用，并设置二次确认（由前端实现）：

| 操作 | 启用方式 |
|------|---------|
| 删除文章 (`cms.delete_article`) | 管理界面 → MCP 工具开关 |
| 修改用户信息 (`sys.update_user`) | 同上 |
| 修改系统配置 (`sys.update_config`) | 同上 |

### 8.5 审计日志

- 所有 MCP 调用（含成功/失败）都写入 `sys_mcp_log` 表。
- 可在 **系统管理 → 审计日志 → MCP 调用日志** 页面查看与筛选。
- 日志保留 **90 天**，超期由定时任务清理。

### 8.6 网络隔离建议

- MCP 端点建议配置在**内网**或通过 API Gateway 暴露，并启用 **IP 白名单**。
- 生产环境建议启用 **HTTPS + HSTS**，避免 Token 被窃听。

---

*文档最后更新：2026-06-12*
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>

---

## 💡 快速参考

### 核心类与接口

| 类型 | 名称 | 命名空间 | 说明 |
|------|------|---------|------|
| Controller | `McpController` | `JeeSiteNET.Modules.Sys.Controllers` | MCP JSON-RPC 2.0 统一入口（/api/v1/sys/mcp） |
| Service | `McpService` | `JeeSiteNET.Modules.Sys.Application.Services` | 工具路由、参数校验、审计写入 |
| Registry | `AiToolRegistry` | `JeeSiteNET.Modules.Cms.Application.Services` | 反射扫描所有 `[AiTool]` 类并维护元信息 |
| Interface | `IAiTool` | `JeeSiteNET.Core.Interfaces` | 所有 AI Tool 的统一接口（`ExecuteAsync(input, ct)`） |
| Attribute | `AiToolAttribute` | `JeeSiteNET.Core.Annotations` | 工具元信息标记（Name/Description/InputSchemaJson/RateLimit） |

### 常用 API 速查

| API | HTTP 方法 | 说明 |
|-----|-----------|------|
| `/api/v1/sys/mcp/jsonrpc` | `POST` | **核心端点**：JSON-RPC 2.0 请求入口，支持 `tools/list`、`tools/call`、`session/list`、`session/create`、`user/profile`、`system/health` |
| `/api/v1/sys/mcp/tools` | `GET` | 浏览器调试用：返回所有已注册工具的 HTML/JSON 清单 |
| `/api/v1/sys/mcp/health` | `GET` | 健康检查（公开端点），返回 postgres/redis/elasticsearch 状态 |
| `tools/list` | JSON-RPC | 列出所有 AI 工具（含名称、描述、JSON Schema） |
| `tools/call` | JSON-RPC | 调用指定工具（传入 `name` + `arguments`） |
| `session/list` | JSON-RPC | 列出当前用户的历史会话 |
| `session/create` | JSON-RPC | 创建新会话，返回 `sessionId` |
| `user/profile` | JSON-RPC | 获取当前登录用户资料 |
| `system/health` | JSON-RPC | 系统健康检查（返回各组件状态与版本号） |

### 最小工作示例

```json
// ===== 请求：列出所有工具 =====
POST /api/v1/sys/mcp/jsonrpc
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "jsonrpc": "2.0",
  "id": "req-001",
  "method": "tools/list",
  "params": { "limit": 50, "offset": 0 }
}

// ===== 响应：工具清单 =====
{
  "jsonrpc": "2.0",
  "id": "req-001",
  "result": {
    "tools": [
      {
        "name": "cms_search_article",
        "description": "按关键词搜索 CMS 文章，返回最相关的 N 篇",
        "inputSchema": {
          "type": "object",
          "properties": {
            "q":        { "type": "string",  "description": "搜索关键词" },
            "category": { "type": "string",  "description": "栏目代码（可选）" },
            "limit":    { "type": "integer", "description": "返回数量，默认 10" }
          },
          "required": ["q"]
        }
      }
      // ... 其他工具
    ],
    "total": 6
  }
}

// ===== 调用工具 =====
{
  "jsonrpc": "2.0",
  "id": "req-002",
  "method": "tools/call",
  "params": {
    "name": "cms_search_article",
    "arguments": { "q": "密码重置", "limit": 5 }
  }
}

// ===== 工具调用响应 =====
{
  "jsonrpc": "2.0",
  "id": "req-002",
  "result": {
    "toolName": "cms_search_article",
    "content": "[{\"title\":\"忘记密码怎么办\",\"url\":\"/article/a001\"}]",
    "contentType": "application/json"
  }
}

// ===== 错误响应（示例） =====
{
  "jsonrpc": "2.0",
  "id": "req-002",
  "error": {
    "code": -32001,
    "message": "Permission denied",
    "data": { "requiredPermission": "sys:mcp:invoke" }
  }
}
```

### 标准错误码速查

| Code | 含义 |
|------|------|
| `-32700` | Parse error（JSON 解析失败） |
| `-32600` | Invalid Request（请求格式不合法） |
| `-32601` | Method not found（方法不存在） |
| `-32602` | Invalid params（参数不合法） |
| `-32603` | Internal error（服务器内部错误） |
| `-32001` | Permission denied（无 `sys:mcp:invoke` 权限） |
| `-32002` | Rate limit exceeded（调用过于频繁） |

### 配置项清单

| 配置键 | 默认值 | 数据类型 | 说明 | 必填 |
|--------|--------|---------|------|------|
| `Mcp:Enabled` | `true` | bool | 是否启用 MCP 端点 | ⬜ |
| `Mcp:RequireBearerToken` | `true` | bool | 是否强制 JWT 鉴权（生产环境必须 true） | ⬜ |
| `Mcp:RateLimit:PerUserPerMinute` | `10` | int | 每用户每分钟 `tools/call` 次数上限 | ⬜ |
| `Mcp:RateLimit:PerUserPerHour` | `200` | int | 每用户每小时次数上限 | ⬜ |
| `Mcp:RateLimit:GlobalPerMinute` | `500` | int | 全局每分钟次数上限 | ⬜ |
| `Mcp:RateLimit:GlobalPerDay` | `10000` | int | 全局每日次数上限 | ⬜ |
| `Mcp:AuditLogRetentionDays` | `90` | int | MCP 调用审计日志保留天数 | ⬜ |
| `Mcp:Tools:EnableDeleteArticle` | `false` | bool | 是否暴露「删除文章」这类有副作用的工具 | ⬜ |

---

## ❓ 常见问题

**Q1：请求 MCP 端点返回 401 Unauthorized？**
- 检查请求头是否携带 `Authorization: Bearer <jwt-token>`。
- 验证 Token 是否过期（可在 `sys/auth/info` 中查看）。
- 确认当前用户拥有 `sys:mcp:invoke` 权限标识。

**Q2：工具未被 LLM 选中调用？**
- 这是最常见问题。检查 `[AiTool]` 的 `Description` 是否用自然语言清晰描述了场景（「用于什么、输入什么、输出什么」）。
- 检查 `InputSchemaJson` 是否完整描述了每个字段的 `type` / `description` / `required`。
- LLM 倾向于选择描述清晰的工具。可使用 Claude Code 的「Tools Debug」模式验证。

**Q3：LLM 构造的 JSON 参数解析失败？**
- `McpService` 内部已使用 `JsonSerializerDefaults.Web` + try-catch 容错降级。
- 若频繁失败，可简化 `InputSchemaJson`（避免复杂嵌套、避免 enum 过多）。
- 对 string/int 字段增加 `minimum` / `maximum` / `minLength` 约束。

**Q4：工具执行超时或外部 API 太慢？**
- HttpClient 默认超时 5 秒，可在 `Program.cs` 中通过 `builder.Services.AddHttpClient("WeatherAPI", c => c.Timeout = TimeSpan.FromSeconds(10))` 调整。
- 工具层必须使用 `CancellationToken` 支持取消（前端可随时终止请求）。
- 对高延迟外部 API 建议加本地缓存（如天气查询结果缓存 10 分钟）。

**Q5：如何让 MCP 端点只对内部网络开放？**
- 生产环境建议在 Nginx / API Gateway 层配置 IP 白名单。
- 在 `Program.cs` 中间件中增加 `RemoteIpAddress` 检查。
- 可设置 `Mcp:RequireBearerToken=true` + 专用 API Key 的双重鉴权。

---

## 📚 相关文档

- [20-AI智能问答](20-AI智能问答) — AI 对话服务的完整架构（MCP 是其协议层）
- [22-Elasticsearch](22-Elasticsearch) — `cms_search_article` 工具的底层实现
- [23-FusionCache缓存](23-FusionCache缓存) — MCP 会话缓存与失效广播
- [26-AI-Tools开发](26-AI-Tools开发) — 如何实现一个自定义 AI Tool（新增工具必读）
- Home: [Wiki 首页](Home)

---

## 🚀 下一步

1. **暴露第一个自定义工具**：参考 26-AI-Tools开发 的 10 条最佳实践，将已有业务服务（如用户查询、任务调度）包装为 MCP 工具。
2. **在 Claude Code 中测试**：使用 Claude Code 的 MCP Client 功能连接 `/api/v1/sys/mcp/jsonrpc`，验证 `tools/list` → `tools/call` 流程。
3. **审计与限流策略调优**：在管理后台查看 `sys_mcp_log`，根据实际用量调整 `Mcp:RateLimit:*` 配置。
4. **为敏感工具加二次确认**：对有副作用的工具（删除、修改）设置前端二次确认弹窗，默认禁用。
5. **WebSocket 推送（可选）**：如需将 MCP 调用结果实时推送到前端，可扩展 `McpController` 为 SignalR/WebSocket 模式。

<div align="center">
  <small>本文档最后更新: 2026-06-13 · JeeSite.NET Wiki</small>
</div>