using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Application.Services;

public interface IArticleIndexService
{
    Task IndexAsync(Article article);
    Task RemoveAsync(string articleCode);
}

public class DefaultArticleIndexService : IArticleIndexService
{
    public Task IndexAsync(Article article) => Task.CompletedTask;
    public Task RemoveAsync(string articleCode) => Task.CompletedTask;
}
