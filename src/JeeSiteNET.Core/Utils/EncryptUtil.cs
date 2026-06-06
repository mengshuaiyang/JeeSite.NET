using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class EncryptUtil
{
    public static string Md5(string input)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }
}
