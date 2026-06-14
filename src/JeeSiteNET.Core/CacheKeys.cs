namespace JeeSiteNET.Core;

/// <summary>
/// 缓存键生成器：统一管理 FusionCache 的 key 命名规则，避免硬编码分散
/// </summary>
public static class CacheKeys
{
    /// <summary>
    /// 缓存键前缀（统一命名空间，避免与其他系统冲突）
    /// </summary>
    public const string Prefix = "JeeSiteNET:";

    /// <summary>
    /// 字典类型缓存键：按 dictType 缓存字典项
    /// </summary>
    /// <param name="dictType">字典类型编码</param>
    /// <returns>完整缓存键</returns>
    public static string DictByType(string dictType) => $"{Prefix}Dict:Type:{dictType}";

    /// <summary>
    /// 角色权限缓存键：按角色编码集合缓存权限列表
    /// </summary>
    /// <param name="roleCodes">角色编码集合（逗号分隔）</param>
    /// <returns>完整缓存键</returns>
    public static string PermissionsByRoles(string roleCodes) => $"{Prefix}Perm:Roles:{roleCodes}";

    /// <summary>
    /// 用户角色编码缓存键：按用户编码缓存其拥有的角色编码
    /// </summary>
    /// <param name="userCode">用户编码</param>
    /// <returns>完整缓存键</returns>
    public static string RoleCodesByUser(string userCode) => $"{Prefix}Role:User:{userCode}";

    /// <summary>
    /// 菜单树缓存键：按模块编码缓存菜单树
    /// </summary>
    /// <param name="moduleCode">模块编码</param>
    /// <returns>完整缓存键</returns>
    public static string MenuTree(string moduleCode) => $"{Prefix}Menu:Tree:{moduleCode}";

    /// <summary>
    /// 系统配置缓存键：按配置键缓存配置值
    /// </summary>
    /// <param name="configKey">配置键</param>
    /// <returns>完整缓存键</returns>
    public static string ConfigValue(string configKey) => $"{Prefix}Config:{configKey}";

    /// <summary>
    /// CMS 文章缓存键：按文章编码缓存文章详情
    /// </summary>
    /// <param name="articleCode">文章编码</param>
    /// <returns>完整缓存键</returns>
    public static string CmsArticle(string articleCode) => $"{Prefix}Cms:Article:{articleCode}";
}
