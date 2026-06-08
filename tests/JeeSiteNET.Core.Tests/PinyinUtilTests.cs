using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class PinyinUtilTests
{
    [Fact]
    public void ToPinyin_ChineseText_ReturnsPinyin()
    {
        PinyinUtil.ToPinyin("中国").Should().Be("zhongguo");
    }

    [Fact]
    public void ToPinyin_MixedText_ReturnsMixed()
    {
        PinyinUtil.ToPinyin("Hello中国").Should().Be("Hellozhongguo");
    }

    [Fact]
    public void ToPinyin_EmptyString_ReturnsEmpty()
    {
        PinyinUtil.ToPinyin("").Should().Be("");
    }

    [Fact]
    public void ToPinyin_Null_ReturnsNull()
    {
        PinyinUtil.ToPinyin(null!).Should().BeNull();
    }

    [Fact]
    public void ToPinyinInitials_ChineseText_ReturnsInitials()
    {
        PinyinUtil.ToPinyinInitials("中国").Should().Be("zg");
    }

    [Fact]
    public void ToPinyinInitials_MixedText_ReturnsMixedInitials()
    {
        PinyinUtil.ToPinyinInitials("Hello中国").Should().Be("Hellozg");
    }

    [Fact]
    public void ToPinyinInitials_EmptyString_ReturnsEmpty()
    {
        PinyinUtil.ToPinyinInitials("").Should().Be("");
    }

    [Fact]
    public void ToPinyin_SingleCharacter_ReturnsPinyin()
    {
        PinyinUtil.ToPinyin("人").Should().Be("ren");
    }

    [Fact]
    public void ToPinyinInitials_SingleCharacter_ReturnsInitial()
    {
        PinyinUtil.ToPinyinInitials("人").Should().Be("r");
    }
}
