    // 引入 System.Net.Http.Headers 命名空间
// 引入命名空间：System.Net.Http.Headers
using System.Net.Http.Headers;
    // 引入 System.Net.Http.Json 命名空间
// 引入命名空间：System.Net.Http.Json
using System.Net.Http.Json;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 Microsoft.AspNetCore.Hosting 命名空间
// 引入命名空间：Microsoft.AspNetCore.Hosting
using Microsoft.AspNetCore.Hosting;
    // 引入 Microsoft.AspNetCore.Mvc.Testing 命名空间
// 引入命名空间：Microsoft.AspNetCore.Mvc.Testing
using Microsoft.AspNetCore.Mvc.Testing;
    // 引入 Microsoft.AspNetCore.TestHost 命名空间
// 引入命名空间：Microsoft.AspNetCore.TestHost
using Microsoft.AspNetCore.TestHost;
    // 引入 Microsoft.Extensions.Caching.Memory 命名空间
// 引入命名空间：Microsoft.Extensions.Caching.Memory
using Microsoft.Extensions.Caching.Memory;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;
    // 引入 ZiggyCreatures.Caching.Fusion 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion
using ZiggyCreatures.Caching.Fusion;
    // 引入 ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson 命名空间
// 引入命名空间：ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

// 定义 JeeSiteNET.Web.IntegrationTests 命名空间
// 定义命名空间：JeeSiteNET.Web.IntegrationTests
namespace JeeSiteNET.Web.IntegrationTests;

// 定义class CustomWebApplicationFactory
// 定义类：CustomWebApplicationFactory
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // 字段 _dbPath
    // 字段：_dbPath
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"JeeSiteNET_Test_{Guid.NewGuid():N}.db");

    // 方法 ConfigureWebHost
    // 方法：ConfigureWebHost
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // if 条件判断
        if (File.Exists(_dbPath))
            // 数据库操作：删除
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

    // 方法 Dispose
    // 方法：Dispose
    protected override void Dispose(bool disposing)
    {
        // 文件/流操作：释放资源
        base.Dispose(disposing);
        // if 条件判断
        if (disposing && File.Exists(_dbPath))
        {
            // 数据库操作：删除
            try { File.Delete(_dbPath); }
            catch { }
        }
    }

    // 方法 InitializeAsync
    // 方法：InitializeAsync
    public async Task InitializeAsync()
    {
        _ = Services;
        // await 异步等待
        await Task.CompletedTask;
    }

    // 方法 DisposeAsync
    // 方法：DisposeAsync
    public new async Task DisposeAsync()
    {
        // IAsyncLifetime cleanup (connection not needed for file-based SQLite)
    }

    // 方法 CreateAuthenticatedClientAsync
    // 方法：CreateAuthenticatedClientAsync
    public async Task<HttpClient> CreateAuthenticatedClientAsync(string loginCode = "admin", string password = "admin")
    {
        // 声明并初始化变量：client
        var client = CreateClient();
        var loginResponse = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new { loginCode, password });
        loginResponse.EnsureSuccessStatusCode();
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        // 声明并初始化变量：token
        var token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // return 返回结果
        return client;
    }

    // 方法 GetTokenAsync
    // 方法：GetTokenAsync
    public async Task<string> GetTokenAsync(string loginCode = "admin", string password = "admin")
    {
    // 引入 var client 命名空间
        using var client = CreateClient();
        var loginResponse = await client.PostAsJsonAsync("/api/v1/sys/auth/login", new { loginCode, password });
        loginResponse.EnsureSuccessStatusCode();
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        // return 返回结果
        return doc.RootElement.GetProperty("data").GetProperty("token").GetString()!;
    }

    private static void RemoveService<T>(IServiceCollection services)
    {
        // foreach 遍历集合
        foreach (var descriptor in services.Where(d => d.ServiceType == typeof(T)).ToList())
            // 集合操作：移除元素
            services.Remove(descriptor);
    }
}
