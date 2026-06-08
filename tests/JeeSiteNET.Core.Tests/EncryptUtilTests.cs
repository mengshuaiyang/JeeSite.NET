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

    [Fact]
    public void Sha1_ShouldReturnCorrectHash()
    {
        var hash = EncryptUtil.Sha1("admin");
        hash.Should().Be("d033e22ae348aeb5660fc2140aec35850c4da997");
    }

    [Fact]
    public void Sha1_EmptyString_ShouldReturnHash()
    {
        var hash = EncryptUtil.Sha1("");
        hash.Should().Be("da39a3ee5e6b4b0d3255bfef95601890afd80709");
    }

    [Fact]
    public void Sha256_ShouldReturnCorrectHash()
    {
        var hash = EncryptUtil.Sha256("admin");
        hash.Should().Be("8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918");
    }

    [Fact]
    public void Sha256_EmptyString_ShouldReturnHash()
    {
        var hash = EncryptUtil.Sha256("");
        hash.Should().Be("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
    }

    [Fact]
    public void Sm3_ShouldReturn64CharHex()
    {
        var hash = EncryptUtil.Sm3("abc");
        hash.Should().NotBeNullOrEmpty();
        hash.Length.Should().Be(64);
        hash.Should().MatchRegex("^[0-9a-f]{64}$");
    }

    [Fact]
    public void Sm3_EmptyString_ShouldReturnHash()
    {
        var hash = EncryptUtil.Sm3("");
        hash.Should().NotBeNullOrEmpty();
        hash.Length.Should().Be(64);
    }

    [Fact]
    public void Sm3_DifferentInputs_ShouldReturnDifferentHashes()
    {
        var hash1 = EncryptUtil.Sm3("admin");
        var hash2 = EncryptUtil.Sm3("admin1");
        hash1.Should().NotBe(hash2);
    }
}
