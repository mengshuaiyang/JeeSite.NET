    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class Sm2Sm4Tests
// 定义类：Sm2Sm4Tests
public class Sm2Sm4Tests
{
    [Fact]
    // 方法 Sm2_GenerateKeyPair_ShouldReturnValidKeys
    // 方法：Sm2_GenerateKeyPair_ShouldReturnValidKeys
    public void Sm2_GenerateKeyPair_ShouldReturnValidKeys()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        // 断言验证
        priv.Should().NotBeNullOrEmpty();
        // 断言验证
        priv.Length.Should().Be(64); // 256 bits hex
        // 断言验证
        pub.Should().StartWith("04");
        // 断言验证
        pub.Length.Should().Be(130); // 04 + 64 + 64
    }

    [Fact]
    // 方法 Sm2_SignAndVerify_ShouldRoundTrip
    // 方法：Sm2_SignAndVerify_ShouldRoundTrip
    public void Sm2_SignAndVerify_ShouldRoundTrip()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        const string text = "Hello 国密 SM2 with SM3! 测试签名";
        // 声明并初始化变量：sig
        var sig = Sm2Util.Sign(priv, text);
        // 断言验证
        sig.Should().NotBeNullOrEmpty();
        // 断言验证
        Sm2Util.Verify(pub, text, sig).Should().BeTrue();
        // 断言验证
        Sm2Util.Verify(pub, text + "!", sig).Should().BeFalse();
    }

    [Fact]
    // 方法 Sm2_EncryptAndDecrypt_ShouldRoundTrip
    // 方法：Sm2_EncryptAndDecrypt_ShouldRoundTrip
    public void Sm2_EncryptAndDecrypt_ShouldRoundTrip()
    {
        var (priv, pub) = Sm2Util.GenerateKeyPair();
        const string text = "SM2 椭圆曲线加密测试: Hello, World! 你好世界！";
        // 声明并初始化变量：cipher
        var cipher = Sm2Util.Encrypt(pub, text);
        // 断言验证
        cipher.Should().NotBeNullOrEmpty();
        // 声明并初始化变量：plain
        var plain = Sm2Util.Decrypt(priv, cipher);
        // 断言验证
        plain.Should().Be(text);
    }

    [Fact]
    // 方法 Sm3_EmptyString_ShouldReturnKnownValue
    // 方法：Sm3_EmptyString_ShouldReturnKnownValue
    public void Sm3_EmptyString_ShouldReturnKnownValue()
    {
        // SM3("") = 1AB21D8355CF1436536E1A6AE8184D899EE0B5BFE210B4A630CE4C56BF90B81A
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sm3("");
        // 断言验证
        hash.Should().NotBeNullOrEmpty();
        // 断言验证
        hash.Length.Should().Be(64);
    }

    [Fact]
    // 方法 Sm3_KnownString_ShouldReturnConsistent
    // 方法：Sm3_KnownString_ShouldReturnConsistent
    public void Sm3_KnownString_ShouldReturnConsistent()
    {
        // 声明并初始化变量：h1
        var h1 = EncryptUtil.Sm3("abc");
        // 声明并初始化变量：h2
        var h2 = EncryptUtil.Sm3("abc");
        // 断言验证
        h1.Should().Be(h2);
        // 断言验证
        h1.Length.Should().Be(64);
    }

    [Fact]
    // 方法 Sm4_EncryptCbcAndDecryptCbc_ShouldRoundTrip
    // 方法：Sm4_EncryptCbcAndDecryptCbc_ShouldRoundTrip
    public void Sm4_EncryptCbcAndDecryptCbc_ShouldRoundTrip()
    {
        // 声明并初始化变量：key
        var key = Sm4Util.GenerateKey();
        // 断言验证
        key.Length.Should().Be(32); // 16 bytes hex
        const string text = "SM4-CBC 分组加密测试: Hello SM4! 你好国密！";
        // 声明并初始化变量：encrypted
        var encrypted = Sm4Util.EncryptCbc(key, text);
        // 断言验证
        encrypted.Should().NotBeNullOrEmpty();
        // 声明并初始化变量：decrypted
        var decrypted = Sm4Util.DecryptCbc(key, encrypted);
        // 断言验证
        decrypted.Should().Be(text);
    }

    [Fact]
    // 方法 Sm4_EncryptCbc_DifferentIvProducesDifferentOutput
    // 方法：Sm4_EncryptCbc_DifferentIvProducesDifferentOutput
    public void Sm4_EncryptCbc_DifferentIvProducesDifferentOutput()
    {
        // 声明并初始化变量：key
        var key = Sm4Util.GenerateKey();
        const string text = "repeated plaintext";
        // 声明并初始化变量：c1
        var c1 = Sm4Util.EncryptCbc(key, text);
        // 声明并初始化变量：c2
        var c2 = Sm4Util.EncryptCbc(key, text);
        // 断言验证
        c1.Should().NotBe(c2); // 不同 IV 应产生不同密文
        // 但都能解密出相同明文
        // 断言验证
        Sm4Util.DecryptCbc(key, c1).Should().Be(text);
        // 断言验证
        Sm4Util.DecryptCbc(key, c2).Should().Be(text);
    }
}
