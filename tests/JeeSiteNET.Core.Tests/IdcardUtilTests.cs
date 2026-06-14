    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class IdcardUtilTests
// 定义类：IdcardUtilTests
public class IdcardUtilTests
{
    [Fact]
    // 方法 ValidateIdcard_Valid18Digit_ReturnsTrue
    // 方法：ValidateIdcard_Valid18Digit_ReturnsTrue
    public void ValidateIdcard_Valid18Digit_ReturnsTrue()
    {
        // Valid 18-digit ID with correct checksum
        // 断言验证
        IdcardUtil.ValidateIdcard("11010519491231002X").Should().BeTrue();
    }

    [Fact]
    // 方法 ValidateIdcard_InvalidChecksum_ReturnsFalse
    // 方法：ValidateIdcard_InvalidChecksum_ReturnsFalse
    public void ValidateIdcard_InvalidChecksum_ReturnsFalse()
    {
        // "11010519491231002X" has checksum 'X'; changing last char makes it invalid
        // 断言验证
        IdcardUtil.ValidateIdcard("110105194912310020").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateIdcard_TooShort_ReturnsFalse
    // 方法：ValidateIdcard_TooShort_ReturnsFalse
    public void ValidateIdcard_TooShort_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateIdcard("12345").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateIdcard_ContainsLetters_ReturnsFalse
    // 方法：ValidateIdcard_ContainsLetters_ReturnsFalse
    public void ValidateIdcard_ContainsLetters_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateIdcard("1101051949123100AB").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateIdcard_EmptyString_ReturnsFalse
    // 方法：ValidateIdcard_EmptyString_ReturnsFalse
    public void ValidateIdcard_EmptyString_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateIdcard("").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateMobile_ValidMobile_ReturnsTrue
    // 方法：ValidateMobile_ValidMobile_ReturnsTrue
    public void ValidateMobile_ValidMobile_ReturnsTrue()
    {
        // 断言验证
        IdcardUtil.ValidateMobile("13800138000").Should().BeTrue();
    }

    [Fact]
    // 方法 ValidateMobile_TooShort_ReturnsFalse
    // 方法：ValidateMobile_TooShort_ReturnsFalse
    public void ValidateMobile_TooShort_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateMobile("12345").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateMobile_InvalidPrefix_ReturnsFalse
    // 方法：ValidateMobile_InvalidPrefix_ReturnsFalse
    public void ValidateMobile_InvalidPrefix_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateMobile("12345678901").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateEmail_ValidEmail_ReturnsTrue
    // 方法：ValidateEmail_ValidEmail_ReturnsTrue
    public void ValidateEmail_ValidEmail_ReturnsTrue()
    {
        // 断言验证
        IdcardUtil.ValidateEmail("test@example.com").Should().BeTrue();
    }

    [Fact]
    // 方法 ValidateEmail_NoAt_ReturnsFalse
    // 方法：ValidateEmail_NoAt_ReturnsFalse
    public void ValidateEmail_NoAt_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateEmail("testexample.com").Should().BeFalse();
    }

    [Fact]
    // 方法 ValidateEmail_EmptyString_ReturnsFalse
    // 方法：ValidateEmail_EmptyString_ReturnsFalse
    public void ValidateEmail_EmptyString_ReturnsFalse()
    {
        // 断言验证
        IdcardUtil.ValidateEmail("").Should().BeFalse();
    }
}
