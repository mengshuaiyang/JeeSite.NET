using JeeSiteNET.Core.Search;
using JeeSiteNET.Modules.Sys.Domain.Entities;
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.Sys.Application.Services;

public class SearchService
{
    private readonly ISearchService _search;
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly ILogRepository _logRepo;

    public SearchService(
        ISearchService search,
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        ILogRepository logRepo)
    {
        _search = search;
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _logRepo = logRepo;
    }

    public async Task<SearchResult<object>> SearchAsync(SearchQuery query)
    {
        return await _search.SearchAsync<object>(query);
    }

    public async Task ReindexAllAsync()
    {
        var users = await _userRepo.Query().ToListAsync();
        foreach (var u in users)
            await _search.IndexAsync("users", u.UserCode, new
            {
                u.UserCode, u.LoginCode, u.UserName, u.Email, u.Phone,
                u.UserType, u.OrgCode, u.OrgName, u.RefName,
            });

        var roles = await _roleRepo.Query().ToListAsync();
        foreach (var r in roles)
            await _search.IndexAsync("roles", r.RoleCode, new
            {
                r.RoleCode, r.RoleName, r.RoleType,
            });

        var logs = await _logRepo.Query().Take(10000).ToListAsync();
        foreach (var l in logs)
            await _search.IndexAsync("logs", l.LogId, new
            {
                l.LogId, l.LogTitle, l.LogType, l.RemoteIp, l.RequestUri,
                l.CreateBy, l.CreateDate,
            });
    }

    public async Task<bool> PingAsync() => await _search.PingAsync();
}
