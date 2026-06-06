using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;

namespace JeeSiteNET.Web.Host.Services;

public class AppAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceProvider _serviceProvider;

    public AppAuthenticationStateProvider(ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        : base(loggerFactory) => _serviceProvider = serviceProvider;

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState state, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return state.User.Identity?.IsAuthenticated == true;
    }
}
