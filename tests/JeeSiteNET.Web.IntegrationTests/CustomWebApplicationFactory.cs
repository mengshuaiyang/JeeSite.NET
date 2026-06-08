using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace JeeSiteNET.Web.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"JeeSiteNET_Test_{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (File.Exists(_dbPath))
            File.Delete(_dbPath);

                builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["DatabaseProvider"] = "Sqlite",
                ["SqliteDbPath"] = _dbPath
            });
        });

        builder.ConfigureTestServices(services =>
        {
            RemoveService<IFusionCache>(services);
            RemoveService<FusionCache>(services);

            services.AddFusionCache()
                .WithDefaultEntryOptions(new FusionCacheEntryOptions
                {
                    Duration = TimeSpan.FromMinutes(30),
                    Priority = CacheItemPriority.Normal
                })
                .WithSerializer(new FusionCacheSystemTextJsonSerializer());
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing && File.Exists(_dbPath))
        {
            try { File.Delete(_dbPath); }
            catch { }
        }
    }

    public async Task InitializeAsync()
    {
        _ = Services;
        await Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        // IAsyncLifetime cleanup (connection not needed for file-based SQLite)
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(string loginCode = "admin", string password = "admin")
    {
        var client = CreateClient();
        var loginResponse = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new { loginCode, password });
        loginResponse.EnsureSuccessStatusCode();
        using var doc = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        var token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    public async Task<string> GetTokenAsync(string loginCode = "admin", string password = "admin")
    {
        using var client = CreateClient();
        var loginResponse = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new { loginCode, password });
        loginResponse.EnsureSuccessStatusCode();
        using var doc = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        return doc.RootElement.GetProperty("data").GetProperty("token").GetString()!;
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        foreach (var descriptor in services.Where(d => d.ServiceType == typeof(T)).ToList())
            services.Remove(descriptor);
    }
}
