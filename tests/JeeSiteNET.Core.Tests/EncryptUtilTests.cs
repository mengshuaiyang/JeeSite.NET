using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class EncryptUtilTests
{
    [Fact]
    public void Md5_ShouldReturnCorrectHash()
    {
        var hash = EncryptUtil.Md5("admin");
        hash.Should().Be("21232f297a57a5a743894a0e4a801fc3");
    }

    [Fact]
    public void Md5_EmptyString_ShouldReturnHash()
    {
        var hash = EncryptUtil.Md5("");
        hash.Should().NotBeNullOrEmpty();
        hash.Length.Should().Be(32);
    }
}
