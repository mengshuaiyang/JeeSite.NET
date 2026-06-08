using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace JeeSiteNET.Web.IntegrationTests;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "admin"
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        doc.RootElement.GetProperty("data").GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidPassword_Returns400()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "wrong_password"
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_Returns400()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "nonexistent_user",
            password = "some_password"
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    public async Task Login_WithEmptyCredentials_Returns400()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "",
            password = ""
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(400);
    }

    [Fact]
    public async Task AuthenticatedRequest_WithValidToken_Succeeds()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/sys/user/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task UnauthenticatedRequest_Returns401()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/sys/user/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task AdminUser_HasAllPermissions()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new
        {
            loginCode = "admin",
            password = "admin"
        });

        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var permissions = doc.RootElement.GetProperty("data").GetProperty("user").GetProperty("permissions");
        permissions.GetArrayLength().Should().Be(1);
        permissions[0].GetString().Should().Be("*");
    }
}
