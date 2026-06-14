    // 引入 System.Net.Http.Json 命名空间
// 引入命名空间：System.Net.Http.Json
using System.Net.Http.Json;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Web.IntegrationTests 命名空间
// 定义命名空间：JeeSiteNET.Web.IntegrationTests
namespace JeeSiteNET.Web.IntegrationTests;

// 定义class AuthTests
// 定义类：AuthTests
public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    // 字段 _factory
    // 字段：_factory
    private readonly CustomWebApplicationFactory _factory;

    // 方法 AuthTests
    // 构造函数：AuthTests
    public AuthTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    // 方法 Login_WithValidCredentials_ReturnsToken
    // 方法：Login_WithValidCredentials_ReturnsToken
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "admin"
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        // 断言验证
        doc.RootElement.GetProperty("data").GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    // 方法 Login_WithInvalidPassword_Returns400
    // 方法：Login_WithInvalidPassword_Returns400
    public async Task Login_WithInvalidPassword_Returns400()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "wrong_password"
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    // 方法 Login_WithNonExistentUser_Returns400
    // 方法：Login_WithNonExistentUser_Returns400
    public async Task Login_WithNonExistentUser_Returns400()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "nonexistent_user",
            password = "some_password"
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    // 方法 Login_WithEmptyCredentials_Returns400
    // 方法：Login_WithEmptyCredentials_Returns400
    public async Task Login_WithEmptyCredentials_Returns400()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "",
            password = ""
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    // 方法 AuthenticatedRequest_WithValidToken_Succeeds
    // 方法：AuthenticatedRequest_WithValidToken_Succeeds
    public async Task AuthenticatedRequest_WithValidToken_Succeeds()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/sys/user/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    // 方法 UnauthenticatedRequest_Returns401
    // 方法：UnauthenticatedRequest_Returns401
    public async Task UnauthenticatedRequest_Returns401()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/user/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    // 方法 AdminUser_HasAllPermissions
    // 方法：AdminUser_HasAllPermissions
    public async Task AdminUser_HasAllPermissions()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "admin"
        });

    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 声明并初始化变量：permissions
        var permissions = doc.RootElement.GetProperty("data").GetProperty("user").GetProperty("permissions");
        // 断言验证
        permissions.GetArrayLength().Should().Be(1);
        // 断言验证
        permissions[0].GetString().Should().Be("*");
    }
}
