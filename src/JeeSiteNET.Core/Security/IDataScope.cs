namespace JeeSiteNET.Core.Security;

public enum DataScopeType
{
    All,                   // 全部数据
    Company,               // 本公司
    CompanyAndChild,       // 本公司及下属
    Office,                // 本部门
    OfficeAndChild,        // 本部门及下属
    Self,                  // 仅本人
    Custom                 // 自定义SQL
}

public interface IDataScopeRule
{
    string TargetType { get; }
    string? TargetValue { get; }
    DataScopeType ScopeType { get; }
    string? ScopeCustomSql { get; }
}

public interface IDataScopeService
{
    IQueryable<T> ApplyDataScope<T>(IQueryable<T> query, string targetType) where T : class;
}
