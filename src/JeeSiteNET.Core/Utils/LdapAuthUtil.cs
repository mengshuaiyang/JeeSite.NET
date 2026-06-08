using System.DirectoryServices.Protocols;
using System.Net;

namespace JeeSiteNET.Core.Utils;

public static class LdapAuthUtil
{
    public static bool Authenticate(string ldapUrl, string bindDn, string bindPassword, string searchBase, string filter, string userPassword)
    {
        using var conn = new LdapConnection(new LdapDirectoryIdentifier(ldapUrl));
        conn.SessionOptions.ProtocolVersion = 3;
        conn.SessionOptions.SecureSocketLayer = ldapUrl.StartsWith("ldaps", StringComparison.OrdinalIgnoreCase);
        conn.AuthType = AuthType.Basic;
        conn.Credential = new NetworkCredential(bindDn, bindPassword);
        conn.Bind();

        var searchRequest = new SearchRequest(searchBase, filter, SearchScope.Subtree);
        var searchResponse = (SearchResponse)conn.SendRequest(searchRequest);

        if (searchResponse.Entries.Count == 0) return false;

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
