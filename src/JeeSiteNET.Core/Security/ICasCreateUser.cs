namespace JeeSiteNET.Core.Security;

public interface ICasCreateUser
{
    string? CreateUser(string userType, Dictionary<string, string> casAttributes);
}
