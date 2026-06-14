using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// IP/MAC 地址工具类，包含本机地址查询及 IPv4 与长整型的互转
/// </summary>
public static class IpUtil
{
    /// <summary>
    /// 获取本机第一个处于工作状态的 IPv4 地址
    /// </summary>
    /// <returns>IPv4 地址字符串；获取失败返回 "127.0.0.1"</returns>
    public static string GetLocalIp()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "127.0.0.1";
    }

    /// <summary>
    /// 获取本机第一个非回环且工作中的网卡物理地址（MAC）
    /// </summary>
    /// <returns>MAC 地址字符串（无分隔符，12 位十六进制）；获取失败返回空字符串</returns>
    public static string GetMacAddress()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up)
            .Select(n => n.GetPhysicalAddress().ToString())
            .FirstOrDefault() ?? "";
    }

    /// <summary>
    /// 将 IPv4 地址字符串转换为大端序长整型（big-endian，便于数据库存储与区间比较）
    /// </summary>
    /// <param name="ip">IPv4 地址字符串</param>
    /// <returns>对应的长整型值；解析失败返回 0</returns>
    public static long IpToLong(string ip)
    {
        if (IPAddress.TryParse(ip, out var addr))
        {
            var bytes = addr.GetAddressBytes();
            if (bytes.Length == 4)
                return ((long)bytes[0] << 24) | ((long)bytes[1] << 16) | ((long)bytes[2] << 8) | bytes[3];
        }
        return 0;
    }

    /// <summary>
    /// 将大端序长整型还原为 IPv4 地址字符串
    /// </summary>
    /// <param name="ipLong">IPv4 地址对应的长整型</param>
    /// <returns>IPv4 地址字符串</returns>
    public static string LongToIp(long ipLong)
    {
        return new IPAddress([(byte)(ipLong >> 24), (byte)(ipLong >> 16), (byte)(ipLong >> 8), (byte)ipLong]).ToString();
    }
}
