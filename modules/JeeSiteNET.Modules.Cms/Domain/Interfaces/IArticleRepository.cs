using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface IArticleRepository : IRepository<Article>
{
    Task<PageResult<Article>> FindPageAsync(PageRequest<Article> request);
    Task<Article?> GetWithDetailAsync(string articleCode);
}
