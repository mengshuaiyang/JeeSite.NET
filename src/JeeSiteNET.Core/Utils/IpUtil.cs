using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace JeeSiteNET.Core.Utils;

public static class IpUtil
{
    public static string GetLocalIp()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        return host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)?.ToString() ?? "127.0.0.1";
    }

    public static string GetMacAddress()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(n => n.NetworkInterfaceType != NetworkInterfaceType.Loopback && n.OperationalStatus == OperationalStatus.Up)
            .Select(n => n.GetPhysicalAddress().ToString())
            .FirstOrDefault() ?? "";
    }

    public static long IpToLong(string ip)
    {
        if (IPAddress.TryParse(ip, out var addr))
        {
            var bytes = addr.GetAddressBytes();
            if (bytes.Length == 4)
                return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
        }
        return 0;
    }

    public static string LongToIp(long ipLong)
    {
        return new IPAddress((uint)ipLong).ToString();
    }
}
