    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class IpUtilTests
// 定义类：IpUtilTests
public class IpUtilTests
{
    [Fact]
    // 方法 IpToLong_ValidIPv4_ReturnsLong
    // 方法：IpToLong_ValidIPv4_ReturnsLong
    public void IpToLong_ValidIPv4_ReturnsLong()
    {
        // 断言验证
        IpUtil.IpToLong("192.168.1.1").Should().Be(3232235777);
    }

    [Fact]
    // 方法 IpToLong_Localhost_ReturnsLong
    // 方法：IpToLong_Localhost_ReturnsLong
    public void IpToLong_Localhost_ReturnsLong()
    {
        // 断言验证
        IpUtil.IpToLong("127.0.0.1").Should().Be(2130706433);
    }

    [Fact]
    // 方法 IpToLong_InvalidIp_ReturnsZero
    // 方法：IpToLong_InvalidIp_ReturnsZero
    public void IpToLong_InvalidIp_ReturnsZero()
    {
        // 断言验证
        IpUtil.IpToLong("invalid").Should().Be(0);
    }

    [Fact]
    // 方法 LongToIp_RoundTrip_ReturnsOriginal
    // 方法：LongToIp_RoundTrip_ReturnsOriginal
    public void LongToIp_RoundTrip_ReturnsOriginal()
    {
        // 声明并初始化变量：original
        var original = "10.0.0.1";
        // 声明并初始化变量：longVal
        var longVal = IpUtil.IpToLong(original);
        // 断言验证
        IpUtil.LongToIp(longVal).Should().Be(original);
    }

    [Fact]
    // 方法 LongToIp_Zero_ReturnsLocalhost
    // 方法：LongToIp_Zero_ReturnsLocalhost
    public void LongToIp_Zero_ReturnsLocalhost()
    {
        // 断言验证
        IpUtil.LongToIp(0).Should().Be("0.0.0.0");
    }

    [Fact]
    // 方法 IpToLong_LongToIp_AreInverses
    // 方法：IpToLong_LongToIp_AreInverses
    public void IpToLong_LongToIp_AreInverses()
    {
        var ips = new[] { "0.0.0.0", "255.255.255.255", "8.8.8.8", "192.168.0.1" };
        // foreach 遍历集合
        foreach (var ip in ips)
            // 断言验证
            IpUtil.LongToIp(IpUtil.IpToLong(ip)).Should().Be(ip);
    }
}
