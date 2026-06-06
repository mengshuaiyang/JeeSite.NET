namespace JeeSiteNET.Core;

public static class CacheKeys
{
    public const string Prefix = "JeeSiteNET:";

    public static string DictByType(string dictType) => $"{Prefix}Dict:Type:{dictType}";
    public static string PermissionsByRoles(string roleCodes) => $"{Prefix}Perm:Roles:{roleCodes}";
    public static string RoleCodesByUser(string userCode) => $"{Prefix}Role:User:{userCode}";
    public static string MenuTree(string moduleCode) => $"{Prefix}Menu:Tree:{moduleCode}";
    public static string ConfigValue(string configKey) => $"{Prefix}Config:{configKey}";
}
