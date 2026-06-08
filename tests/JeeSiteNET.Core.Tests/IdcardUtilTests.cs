using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class IdcardUtilTests
{
    [Fact]
    public void ValidateIdcard_Valid18Digit_ReturnsTrue()
    {
        // Valid 18-digit ID with correct checksum
        IdcardUtil.ValidateIdcard("11010519491231002X").Should().BeTrue();
    }

    [Fact]
    public void ValidateIdcard_InvalidChecksum_ReturnsFalse()
    {
        // "11010519491231002X" has checksum 'X'; changing last char makes it invalid
        IdcardUtil.ValidateIdcard("110105194912310020").Should().BeFalse();
    }

    [Fact]
    public void ValidateIdcard_TooShort_ReturnsFalse()
    {
        IdcardUtil.ValidateIdcard("12345").Should().BeFalse();
    }

    [Fact]
    public void ValidateIdcard_ContainsLetters_ReturnsFalse()
    {
        IdcardUtil.ValidateIdcard("1101051949123100AB").Should().BeFalse();
    }

    [Fact]
    public void ValidateIdcard_EmptyString_ReturnsFalse()
    {
        IdcardUtil.ValidateIdcard("").Should().BeFalse();
    }

    [Fact]
    public void ValidateMobile_ValidMobile_ReturnsTrue()
    {
        IdcardUtil.ValidateMobile("13800138000").Should().BeTrue();
    }

    [Fact]
    public void ValidateMobile_TooShort_ReturnsFalse()
    {
        IdcardUtil.ValidateMobile("12345").Should().BeFalse();
    }

    [Fact]
    public void ValidateMobile_InvalidPrefix_ReturnsFalse()
    {
        IdcardUtil.ValidateMobile("12345678901").Should().BeFalse();
    }

    [Fact]
    public void ValidateEmail_ValidEmail_ReturnsTrue()
    {
        IdcardUtil.ValidateEmail("test@example.com").Should().BeTrue();
    }

    [Fact]
    public void ValidateEmail_NoAt_ReturnsFalse()
    {
        IdcardUtil.ValidateEmail("testexample.com").Should().BeFalse();
    }

    [Fact]
    public void ValidateEmail_EmptyString_ReturnsFalse()
    {
        IdcardUtil.ValidateEmail("").Should().BeFalse();
    }
}
