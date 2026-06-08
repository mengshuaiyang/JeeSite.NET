namespace JeeSiteNET.Core;

public enum DbOperation
{
    Read,
    Write
}

public interface IDbConnectionStringResolver
{
    string GetConnectionString(string? tenantCode = null, DbOperation operation = DbOperation.Write);
    string GetConnectionString(DbOperation operation);
}
