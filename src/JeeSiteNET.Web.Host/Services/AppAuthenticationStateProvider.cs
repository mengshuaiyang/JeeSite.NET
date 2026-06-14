    // 引入 System.Security.Claims 命名空间
// 引入命名空间：System.Security.Claims
using System.Security.Claims;
    // 引入 Microsoft.AspNetCore.Components.Authorization 命名空间
// 引入命名空间：Microsoft.AspNetCore.Components.Authorization
using Microsoft.AspNetCore.Components.Authorization;
    // 引入 Microsoft.AspNetCore.Components.Server 命名空间
// 引入命名空间：Microsoft.AspNetCore.Components.Server
using Microsoft.AspNetCore.Components.Server;

// 定义 JeeSiteNET.Web.Host.Services 命名空间
// 定义命名空间：JeeSiteNET.Web.Host.Services
namespace JeeSiteNET.Web.Host.Services;

// 定义class AppAuthenticationStateProvider
// 定义类：AppAuthenticationStateProvider
public class AppAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    // 字段 _serviceProvider
    // 字段：_serviceProvider
    private readonly IServiceProvider _serviceProvider;

    // 方法 AppAuthenticationStateProvider
    // 构造函数：AppAuthenticationStateProvider
    public AppAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        : base(loggerFactory) => _serviceProvider = serviceProvider;

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    // 方法：ValidateAuthenticationStateAsync
    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState state, CancellationToken cancellationToken)
    {
        // await 异步等待
        await Task.CompletedTask;
        // return 返回结果
        return state.User.Identity?.IsAuthenticated == true;
    }
}
