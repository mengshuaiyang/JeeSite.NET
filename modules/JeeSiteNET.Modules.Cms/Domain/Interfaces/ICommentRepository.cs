using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<PageResult<Comment>> FindPageAsync(PageRequest<Comment> request);
}
