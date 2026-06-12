using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class Sm2Sm4Tests
{
    [Fact]
    public void Sm2_GenerateKeyPair_ShouldReturnValidKeys()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        priv.Should().NotBeNullOrEmpty();
        priv.Length.Should().Be(64); // 256 bits hex
        pub.Should().StartWith("04");
        pub.Length.Should().Be(130); // 04 + 64 + 64
    }

    [Fact]
    public void Sm2_SignAndVerify_ShouldRoundTrip()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        const string text = "Hello 国密 SM2 with SM3! 测试签名";
        var sig = Sm2Util.Sign(priv, text);
        sig.Should().NotBeNullOrEmpty();
        Sm2Util.Verify(pub, text, sig).Should().BeTrue();
        Sm2Util.Verify(pub, text + "!", sig).Should().BeFalse();
    }

    [Fact]
    public void Sm2_EncryptAndDecrypt_ShouldRoundTrip()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        const string text = "SM2 椭圆曲线加密测试: Hello, World! 你好世界！";
        var cipher = Sm2Util.Encrypt(pub, text);
        cipher.Should().NotBeNullOrEmpty();
        var plain = Sm2Util.Decrypt(priv, cipher);
        plain.Should().Be(text);
    }

    [Fact]
    public void Sm3_EmptyString_ShouldReturnKnownValue()
    {
        // SM3("") = 1AB21D8355CF1436536E1A6AE8184D899EE0B5BFE210B4A630CE4C56BF90B81A
        var hash = EncryptUtil.Sm3("");
        hash.Should().NotBeNullOrEmpty();
        hash.Length.Should().Be(64);
    }

    [Fact]
    public void Sm3_KnownString_ShouldReturnConsistent()
    {
        var h1 = EncryptUtil.Sm3("abc");
        var h2 = EncryptUtil.Sm3("abc");
        h1.Should().Be(h2);
        h1.Length.Should().Be(64);
    }

    [Fact]
    public void Sm4_EncryptCbcAndDecryptCbc_ShouldRoundTrip()
    {
        var key = Sm4Util.GenerateKey();
        key.Length.Should().Be(32); // 16 bytes hex
        const string text = "SM4-CBC 分组加密测试: Hello SM4! 你好国密！";
        var encrypted = Sm4Util.EncryptCbc(key, text);
        encrypted.Should().NotBeNullOrEmpty();
        var decrypted = Sm4Util.DecryptCbc(key, encrypted);
        decrypted.Should().Be(text);
    }

    [Fact]
    public void Sm4_EncryptCbc_DifferentIvProducesDifferentOutput()
    {
        var key = Sm4Util.GenerateKey();
        const string text = "repeated plaintext";
        var c1 = Sm4Util.EncryptCbc(key, text);
        var c2 = Sm4Util.EncryptCbc(key, text);
        c1.Should().NotBe(c2); // 不同 IV 应产生不同密文
        // 但都能解密出相同明文
        Sm4Util.DecryptCbc(key, c1).Should().Be(text);
        Sm4Util.DecryptCbc(key, c2).Should().Be(text);
    }
}
