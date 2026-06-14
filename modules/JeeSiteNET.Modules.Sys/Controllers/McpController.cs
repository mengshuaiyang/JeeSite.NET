    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.AspNetCore.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Authorization
using Microsoft.AspNetCore.Authorization;
    // 引入 Microsoft.AspNetCore.Mvc 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc
using Microsoft.AspNetCore.Mvc;
    // 引入 Microsoft.Extensions.Logging 命名空间
// 引入命名空间：Microsoft.Extensions.Logging
using Microsoft.Extensions.Logging;

// 定义 JeeSiteNET.Modules.Sys.Controllers 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Controllers
namespace JeeSiteNET.Modules.Sys.Controllers;

[Route("api/v1/mcp")]
[ApiController]
[AllowAnonymous]
// 定义class McpController
// 定义类：McpController

public class McpController : ControllerBase
{
    // 字段 _userRepository
    // 字段：_userRepository

    private readonly IUserRepository _userRepository;
    // 字段 _logger
    // 字段：_logger

    private readonly ILogger<McpController> _logger;

    // 方法 McpController
    // 构造函数：McpController

    public McpController(IUserRepository userRepository, ILogger<McpController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    // 方法 Info
    // 方法：Info

    public IActionResult Info()
    {
        // return 返回结果
        return Ok(new
        {
            jsonrpc = "2.0",
            serverInfo = new
            {
                name = "JeeSite.NET MCP Server",
                version = "1.0.0",
                description = "JeeSite.NET 平台 MCP 服务"
            },
            tools = GetTools()
        });
    }

    [HttpPost]
    // 方法 HandleJsonRpc
    // 方法：HandleJsonRpc

    public async Task<IActionResult> HandleJsonRpc([FromBody] JsonElement request)
    {
        // 声明并初始化变量：method
        var method = request.TryGetProperty("method", out var m) ? m.GetString() : null;
        JsonElement? id = request.TryGetProperty("id", out var i) ? i : null;

        // try 异常捕获开始
        try
        {
            // return 返回结果
            return method switch
            {
                "tools/list" => Ok(new { jsonrpc = "2.0", id, result = new { tools = GetTools() } }),
                "tools/call" => await HandleToolCall(request, id),
                _ => Ok(new { jsonrpc = "2.0", id, error = new { code = -32601, message = $"未知方法: {method}" } })
            };
        }
        // catch 捕获异常
        catch (Exception ex)
        {
            // 日志：记录警告
            _logger.LogWarning(ex, "MCP 调用失败");
            // return 返回结果
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32603, message = ex.Message } });
        }
    }

    // 方法 GetTools
    // 方法：GetTools

    private static object[] GetTools()
    {
        // return 返回结果
        return new object[]
        {
            new
            {
                name = "search_users",
                description = "按登录名或用户名搜索用户",
                inputSchema = new
                {
                    type = "object",
                    properties = new
                    {
                        keyword = new { type = "string", description = "搜索关键词" }
                    },
                    required = new[] { "keyword" }
                }
            },
            new
            {
                name = "get_user",
                description = "获取用户详情",
                inputSchema = new
                {
                    type = "object",
                    properties = new
                    {
                        userCode = new { type = "string", description = "用户编码" }
                    },
                    required = new[] { "userCode" }
                }
            },
        };
    }

    // 方法 HandleToolCall
    // 方法：HandleToolCall

    private async Task<IActionResult> HandleToolCall(JsonElement request, JsonElement? id)
    {
        // 声明并初始化变量：toolName
        var toolName = request.TryGetProperty("params", out var p)
            // 三元条件表达式
            ? p.TryGetProperty("name", out var n) ? n.GetString() : null
            : null;
        JsonElement args = p.TryGetProperty("arguments", out var a) ? a : default;

        // return 返回结果
        return toolName switch
        {
            "search_users" => await SearchUsers(args, id),
            "get_user" => await GetUser(args, id),
            _ => Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = $"未知工具: {toolName}" } })
        };
    }

    // 方法 SearchUsers
    // 方法：SearchUsers

    private async Task<IActionResult> SearchUsers(JsonElement args, JsonElement? id)
    {
        // 声明并初始化变量：keyword
        var keyword = args.TryGetProperty("keyword", out var k) ? k.GetString() ?? "" : "";
        var users = await _userRepository.FindListAsync();

        // 声明并初始化变量：result
        var result = users
            // 数据库操作：条件过滤
            .Where(u => string.IsNullOrEmpty(keyword) ||
                        // 集合操作：检查是否包含
                        u.LoginCode?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                        // 集合操作：检查是否包含
                        u.UserName?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            .Take(10)
            // 数据库操作：投影选择
            .Select(u => new
            {
                userCode = u.UserCode,
                loginCode = u.LoginCode,
                userName = u.UserName,
                email = u.Email,
                phone = u.Phone,
                status = u.Status
            // 数据库操作：查询为列表
            }).ToList();

        // return 返回结果
        return Ok(new { jsonrpc = "2.0", id, result = new { users = result } });
    }

    // 方法 GetUser
    // 方法：GetUser

    private async Task<IActionResult> GetUser(JsonElement args, JsonElement? id)
    {
        // 声明并初始化变量：userCode
        var userCode = args.TryGetProperty("userCode", out var u) ? u.GetString() ?? "" : "";
        // if 条件判断
        if (string.IsNullOrEmpty(userCode))
            // return 返回结果
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = "缺少 userCode" } });

        // 缓存：获取值
        var user = await _userRepository.GetAsync(userCode);
        // if 条件判断
        if (user == null)
            // return 返回结果
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = "用户不存在" } });

        // return 返回结果
        return Ok(new
        {
            jsonrpc = "2.0",
            id,
            result = new
            {
                user = new
                {
                    userCode = user.UserCode,
                    loginCode = user.LoginCode,
                    userName = user.UserName,
                    email = user.Email,
                    phone = user.Phone,
                    userType = user.UserType,
                    status = user.Status,
                }
            }
        });
    }
}
