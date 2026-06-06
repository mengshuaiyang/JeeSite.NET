using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Modules.Cms.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Cms;

[ModuleDescription(Code = "Cms", Name = "内容管理模块", Version = "1.0.0")]
public class CmsModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<SiteService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ArticleService>();
    }
}
