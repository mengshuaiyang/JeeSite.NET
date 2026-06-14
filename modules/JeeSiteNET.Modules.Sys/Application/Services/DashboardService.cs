using JeeSiteNET.Modules.Sys.Domain.Interfaces;

namespace JeeSiteNET.Modules.Sys.Application.Services;

/// <summary>首页仪表板统计 DTO。</summary>
public class DashboardStats
{
    public int UserCount { get; set; }
    public int RoleCount { get; set; }
    public int MenuCount { get; set; }
    public int OrgCount { get; set; }
    public int PostCount { get; set; }
    public int DictCount { get; set; }
    public int LogCountToday { get; set; }
    public List<RecentLogin> RecentLogins { get; set; } = [];
}

/// <summary>最近登录记录 DTO。</summary>
public class RecentLogin
{
    public string? UserName { get; set; }
    public string? LoginCode { get; set; }
    public DateTime? LoginDate { get; set; }
    public string? IpAddress { get; set; }
}

/// <summary>仪表板服务，负责首页各业务模块的统计数据聚合。</summary>
public class DashboardService
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IMenuRepository _menuRepo;
    private readonly IOrganizationRepository _orgRepo;
    private readonly IPostRepository _postRepo;
    private readonly IDictTypeRepository _dictRepo;
    private readonly ILogRepository _logRepo;

    /// <summary>依赖注入构造函数。</summary>
    public DashboardService(
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        IMenuRepository menuRepo,
        IOrganizationRepository orgRepo,
        IPostRepository postRepo,
        IDictTypeRepository dictRepo,
        ILogRepository logRepo)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _menuRepo = menuRepo;
        _orgRepo = orgRepo;
        _postRepo = postRepo;
        _dictRepo = dictRepo;
        _logRepo = logRepo;
    }

    /// <summary>获取首页综合统计（用户/角色/菜单/机构/岗位/字典总量 + 今日日志 + 最近 10 次登录）。</summary>
    /// <returns>仪表板统计对象。</returns>
    public async Task<DashboardStats> GetStatsAsync()
    {
        var today = DateTime.Today;
        var users = await _userRepo.FindListAsync();

        return new DashboardStats
        {
            UserCount = users.Count,
            RoleCount = (await _roleRepo.FindListAsync()).Count,
            MenuCount = (await _menuRepo.FindListAsync()).Count,
            OrgCount = (await _orgRepo.FindListAsync()).Count,
            PostCount = (await _postRepo.FindListAsync()).Count,
            DictCount = (await _dictRepo.FindListAsync()).Count,
            // 今日日志数量（CreateDate >= 今天 0 点）
            LogCountToday = _logRepo.Query().Count(l => l.CreateDate >= today),
            // 最近 10 条登录日志，按时间倒序
            RecentLogins = _logRepo.Query()
                .Where(l => l.LogType == "login" && l.CreateDate != null)
                .OrderByDescending(l => l.CreateDate)
                .Take(10)
                .ToList()
                .Select(l => new RecentLogin
                {
                    UserName = l.UserName,
                    LoginCode = l.CreateBy,
                    LoginDate = l.CreateDate,
                    IpAddress = l.RemoteIp
                })
                .ToList()
        };
    }
}
