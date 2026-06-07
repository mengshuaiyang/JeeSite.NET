using JeeSiteNET.Core;
using JeeSiteNET.Modules.Cms.Domain.Entities;

namespace JeeSiteNET.Modules.Cms.Domain.Interfaces;

public interface IGuestbookRepository : IRepository<Guestbook>
{
    Task<PageResult<Guestbook>> FindPageAsync(PageRequest<Guestbook> request);
}
