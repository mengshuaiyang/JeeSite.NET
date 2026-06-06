namespace JeeSiteNET.Core.Security;

public interface ICurrentUser
{
    string UserCode { get; }
    string UserName { get; }
    string? UserType { get; }
    string? OrgCode { get; }
    string? OrgName { get; }
    List<string> RoleCodes { get; }
    List<string> Permissions { get; }
    bool IsSuperAdmin { get; }
    bool IsAuthenticated { get; }
}
