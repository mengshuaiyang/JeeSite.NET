    // 引入 JeeSiteNET.Core.Search 命名空间
// 引入命名空间：JeeSiteNET.Core.Search
using JeeSiteNET.Core.Search;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 Microsoft.EntityFrameworkCore 命名空间
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义 JeeSiteNET.Modules.Sys.Application.Services 命名空间
// 定义命名空间：JeeSiteNET.Modules.Sys.Application.Services
namespace JeeSiteNET.Modules.Sys.Application.Services;

// 定义class SearchService
// 定义类：SearchService
public class SearchService
{
    // 字段 _search
    // 字段：_search
    private readonly ISearchService _search;
    // 字段 _userRepo
    // 字段：_userRepo
    private readonly IUserRepository _userRepo;
    // 字段 _roleRepo
    // 字段：_roleRepo
    private readonly IRoleRepository _roleRepo;
    // 字段 _logRepo
    // 字段：_logRepo
    private readonly ILogRepository _logRepo;

    // 构造函数 SearchService
    // 构造函数：SearchService
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

    // 方法 SearchAsync
    // 方法：SearchAsync
    public async Task<SearchResult<object>> SearchAsync(SearchQuery query)
    {
        // return 返回结果
        return await _search.SearchAsync<object>(query);
    }

    // 方法 ReindexAllAsync
    // 方法：ReindexAllAsync
    public async Task ReindexAllAsync()
    {
        // 数据库操作：异步查询为列表
        var users = await _userRepo.Query().ToListAsync();
        // foreach 遍历集合
        foreach (var u in users)
            // await 异步等待
            await _search.IndexAsync("users", u.UserCode, new
            {
                u.UserCode, u.LoginCode, u.UserName, u.Email, u.Phone,
                u.UserType, u.OrgCode, u.OrgName, u.RefName,
            });

        // 数据库操作：异步查询为列表
        var roles = await _roleRepo.Query().ToListAsync();
        // foreach 遍历集合
        foreach (var r in roles)
            // await 异步等待
            await _search.IndexAsync("roles", r.RoleCode, new
            {
                r.RoleCode, r.RoleName, r.RoleType,
            });

        // 数据库操作：异步查询为列表
        var logs = await _logRepo.Query().Take(10000).ToListAsync();
        // foreach 遍历集合
        foreach (var l in logs)
            // await 异步等待
            await _search.IndexAsync("logs", l.LogId, new
            {
                l.LogId, l.LogTitle, l.LogType, l.RemoteIp, l.RequestUri,
                l.CreateBy, l.CreateDate,
            });
    }

    // 方法：PingAsync
    public async Task<bool> PingAsync() => await _search.PingAsync();
}
