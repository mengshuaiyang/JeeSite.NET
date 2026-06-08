using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class IpUtilTests
{
    [Fact]
    public void IpToLong_ValidIPv4_ReturnsLong()
    {
        IpUtil.IpToLong("192.168.1.1").Should().Be(3232235777);
    }

    [Fact]
    public void IpToLong_Localhost_ReturnsLong()
    {
        IpUtil.IpToLong("127.0.0.1").Should().Be(2130706433);
    }

    [Fact]
    public void IpToLong_InvalidIp_ReturnsZero()
    {
        IpUtil.IpToLong("invalid").Should().Be(0);
    }

    [Fact]
    public void LongToIp_RoundTrip_ReturnsOriginal()
    {
        var original = "10.0.0.1";
        var longVal = IpUtil.IpToLong(original);
        IpUtil.LongToIp(longVal).Should().Be(original);
    }

    [Fact]
    public void LongToIp_Zero_ReturnsLocalhost()
    {
        IpUtil.LongToIp(0).Should().Be("0.0.0.0");
    }

    [Fact]
    public void IpToLong_LongToIp_AreInverses()
    {
        var ips = new[] { "0.0.0.0", "255.255.255.255", "8.8.8.8", "192.168.0.1" };
        foreach (var ip in ips)
            IpUtil.LongToIp(IpUtil.IpToLong(ip)).Should().Be(ip);
    }
}
