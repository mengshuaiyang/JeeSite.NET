using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class RsaUtilTests
{
    [Fact]
    public void GenerateKeyPair_ShouldReturnBothKeys()
    {
        var (pub, pri) = RsaUtil.GenerateKeyPair();
        pub.Should().NotBeNullOrEmpty();
        pri.Should().NotBeNullOrEmpty();
        pub.Should().MatchRegex("^[0-9A-F]+$");
        pri.Should().MatchRegex("^[0-9A-F]+$");
    }

    [Fact]
    public void EncryptAndDecrypt_ShouldRoundTrip()
    {
        var (pub, pri) = RsaUtil.GenerateKeyPair();
        const string original = "Hello RSA! 你好世界！@#$%^&*()";
        var encrypted = RsaUtil.Encrypt(pub, original);
        encrypted.Should().NotBeNullOrEmpty();
        var decrypted = RsaUtil.Decrypt(pri, encrypted);
        decrypted.Should().Be(original);
    }

    [Fact]
    public void SignAndVerify_ShouldRoundTrip()
    {
        var (pub, pri) = RsaUtil.GenerateKeyPair();
        const string data = "Important data to sign";
        var signature = RsaUtil.Sign(pri, data);
        signature.Should().NotBeNullOrEmpty();
        RsaUtil.Verify(pub, data, signature).Should().BeTrue();
    }

    [Fact]
    public void SignAndVerify_TamperedData_ShouldFail()
    {
        var (pub, pri) = RsaUtil.GenerateKeyPair();
        const string data = "Original data";
        var signature = RsaUtil.Sign(pri, data);
        RsaUtil.Verify(pub, data + "tampered", signature).Should().BeFalse();
    }
}
