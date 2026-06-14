namespace JeeSiteNET.Core.Security;

/// <summary>
/// CAS 单点登录用户创建接口：当 CAS 认证成功但本地系统中不存在该用户时，
/// 由业务模块实现此接口，根据 CAS 返回的属性字典自动创建本地用户账户
/// </summary>
public interface ICasCreateUser
{
    /// <summary>
    /// 根据 CAS 返回的用户属性创建本地用户
    /// </summary>
    /// <param name="userType">用户类型（由 CAS 服务端约定的业务字段）</param>
    /// <param name="casAttributes">CAS 服务端返回的属性字典（如 loginName、email、displayName 等）</param>
    /// <returns>新建用户的编码（若返回 null 表示创建失败/未创建）</returns>
    string? CreateUser(string userType, Dictionary<string, string> casAttributes);
}
