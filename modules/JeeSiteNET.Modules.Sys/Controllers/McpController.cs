using System.Text.Json;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JeeSiteNET.Modules.Sys.Controllers;

[Route("api/v1/mcp")]
[ApiController]
[AllowAnonymous]
public class McpController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<McpController> _logger;

    public McpController(IUserRepository userRepository, ILogger<McpController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Info()
    {
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
    public async Task<IActionResult> HandleJsonRpc([FromBody] JsonElement request)
    {
        var method = request.TryGetProperty("method", out var m) ? m.GetString() : null;
        JsonElement? id = request.TryGetProperty("id", out var i) ? i : null;

        try
        {
            return method switch
            {
                "tools/list" => Ok(new { jsonrpc = "2.0", id, result = new { tools = GetTools() } }),
                "tools/call" => await HandleToolCall(request, id),
                _ => Ok(new { jsonrpc = "2.0", id, error = new { code = -32601, message = $"未知方法: {method}" } })
            };
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "MCP 调用失败");
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32603, message = ex.Message } });
        }
    }

    private static object[] GetTools()
    {
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

    private async Task<IActionResult> HandleToolCall(JsonElement request, JsonElement? id)
    {
        var toolName = request.TryGetProperty("params", out var p)
            ? p.TryGetProperty("name", out var n) ? n.GetString() : null
            : null;
        JsonElement args = p.TryGetProperty("arguments", out var a) ? a : default;

        return toolName switch
        {
            "search_users" => await SearchUsers(args, id),
            "get_user" => await GetUser(args, id),
            _ => Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = $"未知工具: {toolName}" } })
        };
    }

    private async Task<IActionResult> SearchUsers(JsonElement args, JsonElement? id)
    {
        var keyword = args.TryGetProperty("keyword", out var k) ? k.GetString() ?? "" : "";
        var users = await _userRepository.FindListAsync();

        var result = users
            .Where(u => string.IsNullOrEmpty(keyword) ||
                        u.LoginCode?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true ||
                        u.UserName?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true)
            .Take(10)
            .Select(u => new
            {
                userCode = u.UserCode,
                loginCode = u.LoginCode,
                userName = u.UserName,
                email = u.Email,
                phone = u.Phone,
                status = u.Status
            }).ToList();

        return Ok(new { jsonrpc = "2.0", id, result = new { users = result } });
    }

    private async Task<IActionResult> GetUser(JsonElement args, JsonElement? id)
    {
        var userCode = args.TryGetProperty("userCode", out var u) ? u.GetString() ?? "" : "";
        if (string.IsNullOrEmpty(userCode))
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = "缺少 userCode" } });

        var user = await _userRepository.GetAsync(userCode);
        if (user == null)
            return Ok(new { jsonrpc = "2.0", id, error = new { code = -32602, message = "用户不存在" } });

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
