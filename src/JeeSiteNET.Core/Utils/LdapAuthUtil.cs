using System.DirectoryServices.Protocols;
using System.Net;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// LDAP（轻量目录访问协议）身份认证工具类，典型用于 OpenLDAP / Active Directory 登录验证
/// </summary>
public static class LdapAuthUtil
{
    /// <summary>
    /// 对 LDAP 目录中的用户进行身份验证
    /// </summary>
    /// <param name="ldapUrl">LDAP 服务器地址（如 ldap://dc.example.com 或 ldaps://dc.example.com）</param>
    /// <param name="bindDn">用于检索用户 DN 的绑定账号（如 cn=admin,dc=example,dc=com）</param>
    /// <param name="bindPassword">绑定账号密码</param>
    /// <param name="searchBase">搜索根 DN（如 ou=users,dc=example,dc=com）</param>
    /// <param name="filter">LDAP 过滤表达式（如 (uid={0}) / (sAMAccountName={0})）</param>
    /// <param name="userPassword">待验证用户的密码</param>
    /// <returns>验证通过返回 true；用户不存在或密码错误返回 false</returns>
    public static bool Authenticate(string ldapUrl, string bindDn, string bindPassword, string searchBase, string filter, string userPassword)
    {
        // 以管理员账号绑定连接，用于后续查找用户 DN；协议版本采用 v3
        using var conn = new LdapConnection(new LdapDirectoryIdentifier(ldapUrl));
        conn.SessionOptions.ProtocolVersion = 3;
        conn.SessionOptions.SecureSocketLayer = ldapUrl.StartsWith("ldaps", StringComparison.OrdinalIgnoreCase);
        conn.AuthType = AuthType.Basic;
        conn.Credential = new NetworkCredential(bindDn, bindPassword);
        conn.Bind();

        // 在指定 searchBase 下按 filter 查找用户，默认返回第一条匹配
        var searchRequest = new SearchRequest(searchBase, filter, SearchScope.Subtree);
        var searchResponse = (SearchResponse)conn.SendRequest(searchRequest);

        if (searchResponse.Entries.Count == 0) return false;

        // 使用用户的真实 DN + 输入密码再次 Bind，完成密码校验
        var userDn = searchResponse.Entries[0].DistinguishedName;
        using var userConn = new LdapConnection(new LdapDirectoryIdentifier(ldapUrl));
        userConn.SessionOptions.ProtocolVersion = 3;
        userConn.SessionOptions.SecureSocketLayer = ldapUrl.StartsWith("ldaps", StringComparison.OrdinalIgnoreCase);
        userConn.AuthType = AuthType.Basic;
        userConn.Credential = new NetworkCredential(userDn, userPassword);

        try
        {
            userConn.Bind();
            return true;
        }
        catch (LdapException)
        {
            return false;
        }
    }
}
