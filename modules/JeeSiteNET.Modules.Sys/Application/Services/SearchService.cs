    // 引入 JeeSiteNET.Core.Search 命名空间
// 引入命名空间：JeeSiteNET.Core.Search
using JeeSiteNET.Core.Search;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Entities 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Entities
using JeeSiteNET.Modules.Sys.Domain.Entities;
    // 引入 JeeSiteNET.Modules.Sys.Domain.Interfaces 命名空间
// 引入命名空间：JeeSiteNET.Modules.Sys.Domain.Interfaces
using JeeSiteNET.Modules.Sys.Domain.Interfaces;
    // 引入 System.Threading 命名空间（SemaphoreSlim）
// 引入命名空间：System.Threading
using System.Threading;
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
        // 分页批量重建索引，避免全表一次性拉入内存；并行建索引并限制并发度
        const int pageSize = 200;
        const int maxDegreeOfParallelism = 8;
        using var throttler = new SemaphoreSlim(maxDegreeOfParallelism);

        // 用户索引
        await ReindexPagedAsync(
            page => _userRepo.Query().OrderBy(u => u.UserCode).Skip(page * pageSize).Take(pageSize).ToListAsync(),
            users => IndexUsersAsync(users, throttler));

        // 角色索引
        await ReindexPagedAsync(
            page => _roleRepo.Query().OrderBy(r => r.RoleCode).Skip(page * pageSize).Take(pageSize).ToListAsync(),
            roles => IndexRolesAsync(roles, throttler));

        // 日志索引（保留原 10000 条上限）
        await ReindexPagedAsync(
            page => _logRepo.Query().OrderBy(l => l.LogId).Skip(page * pageSize).Take(pageSize).ToListAsync(),
            logs => IndexLogsAsync(logs, throttler),
            maxItems: 10000);
    }

    /// <summary>分页读取并受控并行处理每一页实体，直到取完或无更多数据。</summary>
    private static async Task ReindexPagedAsync<T>(
        Func<int, Task<List<T>>> fetchPageAsync,
        Func<List<T>, Task> processPageAsync,
        int? maxItems = null)
    {
        var page = 0;
        var fetched = 0;
        while (true)
        {
            var items = await fetchPageAsync(page);
            if (items.Count == 0)
                break;

            await processPageAsync(items);

            fetched += items.Count;
            page++;
            if (maxItems.HasValue && fetched >= maxItems.Value)
                break;
        }
    }

    private async Task IndexUsersAsync(List<User> users, SemaphoreSlim throttler)
    {
        var tasks = users.Select(async u =>
        {
            await throttler.WaitAsync();
            try { await _search.IndexAsync("users", u.UserCode, new { u.UserCode, u.LoginCode, u.UserName, u.Email, u.Phone, u.UserType, u.OrgCode, u.OrgName, u.RefName }); }
            finally { throttler.Release(); }
        });
        await Task.WhenAll(tasks);
    }

    private async Task IndexRolesAsync(List<Role> roles, SemaphoreSlim throttler)
    {
        var tasks = roles.Select(async r =>
        {
            await throttler.WaitAsync();
            try { await _search.IndexAsync("roles", r.RoleCode, new { r.RoleCode, r.RoleName, r.RoleType }); }
            finally { throttler.Release(); }
        });
        await Task.WhenAll(tasks);
    }

    private async Task IndexLogsAsync(List<Log> logs, SemaphoreSlim throttler)
    {
        var tasks = logs.Select(async l =>
        {
            await throttler.WaitAsync();
            try { await _search.IndexAsync("logs", l.LogId, new { l.LogId, l.LogTitle, l.LogType, l.RemoteIp, l.RequestUri, l.CreateBy, l.CreateDate }); }
            finally { throttler.Release(); }
        });
        await Task.WhenAll(tasks);
    }

    // 方法：PingAsync
    public async Task<bool> PingAsync() => await _search.PingAsync();
}
