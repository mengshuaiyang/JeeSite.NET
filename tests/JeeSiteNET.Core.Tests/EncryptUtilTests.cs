    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class EncryptUtilTests
// 定义类：EncryptUtilTests
public class EncryptUtilTests
{
    [Fact]
    // 方法 Md5_ShouldReturnCorrectHash
    // 方法：Md5_ShouldReturnCorrectHash
    public void Md5_ShouldReturnCorrectHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Md5("admin");
        // 断言验证
        hash.Should().Be("21232f297a57a5a743894a0e4a801fc3");
    }

    [Fact]
    // 方法 Md5_EmptyString_ShouldReturnHash
    // 方法：Md5_EmptyString_ShouldReturnHash
    public void Md5_EmptyString_ShouldReturnHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Md5("");
        // 断言验证
        hash.Should().NotBeNullOrEmpty();
        // 断言验证
        hash.Length.Should().Be(32);
    }

    [Fact]
    // 方法 Sha1_ShouldReturnCorrectHash
    // 方法：Sha1_ShouldReturnCorrectHash
    public void Sha1_ShouldReturnCorrectHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sha1("admin");
        // 断言验证
        hash.Should().Be("d033e22ae348aeb5660fc2140aec35850c4da997");
    }

    [Fact]
    // 方法 Sha1_EmptyString_ShouldReturnHash
    // 方法：Sha1_EmptyString_ShouldReturnHash
    public void Sha1_EmptyString_ShouldReturnHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sha1("");
        // 断言验证
        hash.Should().Be("da39a3ee5e6b4b0d3255bfef95601890afd80709");
    }

    [Fact]
    // 方法 Sha256_ShouldReturnCorrectHash
    // 方法：Sha256_ShouldReturnCorrectHash
    public void Sha256_ShouldReturnCorrectHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sha256("admin");
        // 断言验证
        hash.Should().Be("8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918");
    }

    [Fact]
    // 方法 Sha256_EmptyString_ShouldReturnHash
    // 方法：Sha256_EmptyString_ShouldReturnHash
    public void Sha256_EmptyString_ShouldReturnHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sha256("");
        // 断言验证
        hash.Should().Be("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
    }

    [Fact]
    // 方法 Sm3_ShouldReturn64CharHex
    // 方法：Sm3_ShouldReturn64CharHex
    public void Sm3_ShouldReturn64CharHex()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sm3("abc");
        // 断言验证
        hash.Should().NotBeNullOrEmpty();
        // 断言验证
        hash.Length.Should().Be(64);
        // 断言验证
        hash.Should().MatchRegex("^[0-9a-f]{64}$");
    }

    [Fact]
    // 方法 Sm3_EmptyString_ShouldReturnHash
    // 方法：Sm3_EmptyString_ShouldReturnHash
    public void Sm3_EmptyString_ShouldReturnHash()
    {
        // 声明并初始化变量：hash
        var hash = EncryptUtil.Sm3("");
        // 断言验证
        hash.Should().NotBeNullOrEmpty();
        // 断言验证
        hash.Length.Should().Be(64);
    }

    [Fact]
    // 方法 Sm3_DifferentInputs_ShouldReturnDifferentHashes
    // 方法：Sm3_DifferentInputs_ShouldReturnDifferentHashes
    public void Sm3_DifferentInputs_ShouldReturnDifferentHashes()
    {
        // 声明并初始化变量：hash1
        var hash1 = EncryptUtil.Sm3("admin");
        // 声明并初始化变量：hash2
        var hash2 = EncryptUtil.Sm3("admin1");
        // 断言验证
        hash1.Should().NotBe(hash2);
    }
}
