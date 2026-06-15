using JeeSiteNET.Core.AiTools;
using JeeSiteNET.Core.Modules;
using JeeSiteNET.Modules.Cms.Application.AiTools;
using JeeSiteNET.Modules.Cms.Application.Services;
using JeeSiteNET.Modules.Cms.Domain.Interfaces;
using JeeSiteNET.Modules.Cms.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JeeSiteNET.Modules.Cms;

// ================================================================
// 内容管理模块 (CMS) 安装器
//
// 本模块提供站点、栏目、文章、评论、留言、AI 对话等功能。
// 调用链：
//   Program.cs → ModuleLoader → CmsModuleInstaller
//   → ArticleController / CmsFrontController（前端展示）
//   → AiChatController（AI 对话 + SSE 流式响应）
//   → CmsService（文章管理 / 评论 / 留言 / 统计）
//   → PageCacheService（CMS 页面缓存，键前缀 "CmsPage:"）
// ================================================================

[ModuleDescription(Code = "Cms", Name = "内容管理模块", Version = "1.2.0")]
public class CmsModuleInstaller : IModuleInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // ========== 仓储层 ==========
        services.AddScoped<ISiteRepository, SiteRepository>();            // 站点管理
        services.AddScoped<ICategoryRepository, CategoryRepository>();    // 栏目/分类树
        services.AddScoped<IArticleRepository, ArticleRepository>();      // 文章
        services.AddScoped<IArticleDataRepository, ArticleDataRepository>(); // 文章内容（大字段）
        services.AddScoped<IArticlePosIdRepository, ArticlePosIdRepository>(); // 文章位置索引
        services.AddScoped<IArticleTagRepository, ArticleTagRepository>();    // 文章标签关联
        services.AddScoped<ITagRepository, TagRepository>();              // 标签
        services.AddScoped<ICommentRepository, CommentRepository>();      // 评论
        services.AddScoped<IGuestbookRepository, GuestbookRepository>();  // 留言
        services.AddScoped<IVisitLogRepository, VisitLogRepository>();    // 访问日志
        services.AddScoped<IReportRepository, ReportRepository>();        // 举报

        // ========== 基础服务 ==========
        services.AddScoped<IArticleIndexService, DefaultArticleIndexService>();  // 文章全文搜索索引
        services.AddScoped<IArticleVectorStore, DefaultArticleVectorStore>();    // 文章向量存储（AI RAG）
        services.AddScoped<IArticleAuthService, DefaultArticleAuthService>();    // 文章权限校验

        // ========== 应用服务 ==========
        services.AddScoped<SiteService>();        // 站点 CRUD
        services.AddScoped<CategoryService>();    // 栏目 CRUD + 树操作
        services.AddScoped<ArticleService>();     // 文章 CRUD + 审核
        services.AddScoped<CmsService>();         // 综合操作（评论/留言/举报/统计/前端查询）
        services.AddScoped<AiChatService>();      // AI 对话（调用 DeepSeek API + Function Calling）
        services.AddScoped<PageCacheService>();   // CMS 页面缓存（FusionCache，键前缀 "CmsPage:"）

        // ========== AI 工具注册 ==========
        // AiToolRegistry 管理所有 AI 函数调用工具
        // 当 AI 识别到需要查询数据时会调用这些工具
        services.AddSingleton<AiToolRegistry>();  // AI 工具注册中心（单例，全局共享）
    }
}
