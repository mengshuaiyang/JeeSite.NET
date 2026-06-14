    // 引入 JeeSiteNET.Core.AiTools 命名空间
// 引入命名空间：JeeSiteNET.Core.AiTools
using JeeSiteNET.Core.AiTools;
    // 引入 JeeSiteNET.Core.Modules 命名空间
// 引入命名空间：JeeSiteNET.Core.Modules
using JeeSiteNET.Core.Modules;
    // 引入 JeeSiteNET.Modules.Cms.Application.AiTools 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.AiTools
using JeeSiteNET.Modules.Cms.Application.AiTools;
    // 引入 JeeSiteNET.Modules.Cms.Application.Services 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Application.Services
using JeeSiteNET.Modules.Cms.Application.Services;
    // 引入 JeeSiteNET.Modules.Cms.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Domain.Interfaces
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
    // 引入 JeeSiteNET.Modules.Cms.Infrastructure.Repositories 命名空间
// 引入命名空间：JeeSiteNET.Modules.Cms.Infrastructure.Repositories
using JeeSiteNET.Modules.Cms.Infrastructure.Repositories;
    // 引入 Microsoft.Extensions.Configuration 命名空间
// 引入命名空间：Microsoft.Extensions.Configuration
using Microsoft.Extensions.Configuration;
    // 引入 Microsoft.Extensions.DependencyInjection 命名空间
// 引入命名空间：Microsoft.Extensions.DependencyInjection
using Microsoft.Extensions.DependencyInjection;

// 定义 JeeSiteNET.Modules.Cms 命名空间
// 定义命名空间：JeeSiteNET.Modules.Cms
namespace JeeSiteNET.Modules.Cms;

[ModuleDescription(Code = "Cms", Name = "内容管理模块", Version = "1.2.0")]
// 定义class CmsModuleInstaller
// 定义类：CmsModuleInstaller
public class CmsModuleInstaller : IModuleInstaller
{
    // 方法 ConfigureServices
    // 方法：ConfigureServices
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticleDataRepository, ArticleDataRepository>();
        services.AddScoped<IArticlePosIdRepository, ArticlePosIdRepository>();
        services.AddScoped<IArticleTagRepository, ArticleTagRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IGuestbookRepository, GuestbookRepository>();
        services.AddScoped<IVisitLogRepository, VisitLogRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IArticleIndexService, DefaultArticleIndexService>();
        services.AddScoped<IArticleVectorStore, DefaultArticleVectorStore>();
        services.AddScoped<IArticleAuthService, DefaultArticleAuthService>();
        services.AddScoped<SiteService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<ArticleService>();
        services.AddScoped<CmsService>();
        services.AddScoped<AiChatService>();
        services.AddScoped<PageCacheService>();

        services.AddSingleton<AiToolRegistry>();
    }
}
