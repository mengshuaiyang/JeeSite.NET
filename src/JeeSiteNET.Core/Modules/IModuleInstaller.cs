using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Core.Modules;

public interface IModuleInstaller
{
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
}
