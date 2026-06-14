    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class PinyinUtilTests
// 定义类：PinyinUtilTests
public class PinyinUtilTests
{
    [Fact]
    // 方法 ToPinyin_ChineseText_ReturnsPinyin
    // 方法：ToPinyin_ChineseText_ReturnsPinyin
    public void ToPinyin_ChineseText_ReturnsPinyin()
    {
        // 断言验证
        PinyinUtil.ToPinyin("中国").Should().Be("zhongguo");
    }

    [Fact]
    // 方法 ToPinyin_MixedText_ReturnsMixed
    // 方法：ToPinyin_MixedText_ReturnsMixed
    public void ToPinyin_MixedText_ReturnsMixed()
    {
        // 断言验证
        PinyinUtil.ToPinyin("Hello中国").Should().Be("Hellozhongguo");
    }

    [Fact]
    // 方法 ToPinyin_EmptyString_ReturnsEmpty
    // 方法：ToPinyin_EmptyString_ReturnsEmpty
    public void ToPinyin_EmptyString_ReturnsEmpty()
    {
        // 断言验证
        PinyinUtil.ToPinyin("").Should().Be("");
    }

    [Fact]
    // 方法 ToPinyin_Null_ReturnsNull
    // 方法：ToPinyin_Null_ReturnsNull
    public void ToPinyin_Null_ReturnsNull()
    {
        // 断言验证
        PinyinUtil.ToPinyin(null!).Should().BeNull();
    }

    [Fact]
    // 方法 ToPinyinInitials_ChineseText_ReturnsInitials
    // 方法：ToPinyinInitials_ChineseText_ReturnsInitials
    public void ToPinyinInitials_ChineseText_ReturnsInitials()
    {
        // 断言验证
        PinyinUtil.ToPinyinInitials("中国").Should().Be("zg");
    }

    [Fact]
    // 方法 ToPinyinInitials_MixedText_ReturnsMixedInitials
    // 方法：ToPinyinInitials_MixedText_ReturnsMixedInitials
    public void ToPinyinInitials_MixedText_ReturnsMixedInitials()
    {
        // 断言验证
        PinyinUtil.ToPinyinInitials("Hello中国").Should().Be("Hellozg");
    }

    [Fact]
    // 方法 ToPinyinInitials_EmptyString_ReturnsEmpty
    // 方法：ToPinyinInitials_EmptyString_ReturnsEmpty
    public void ToPinyinInitials_EmptyString_ReturnsEmpty()
    {
        // 断言验证
        PinyinUtil.ToPinyinInitials("").Should().Be("");
    }

    [Fact]
    // 方法 ToPinyin_SingleCharacter_ReturnsPinyin
    // 方法：ToPinyin_SingleCharacter_ReturnsPinyin
    public void ToPinyin_SingleCharacter_ReturnsPinyin()
    {
        // 断言验证
        PinyinUtil.ToPinyin("人").Should().Be("ren");
    }

    [Fact]
    // 方法 ToPinyinInitials_SingleCharacter_ReturnsInitial
    // 方法：ToPinyinInitials_SingleCharacter_ReturnsInitial
    public void ToPinyinInitials_SingleCharacter_ReturnsInitial()
    {
        // 断言验证
        PinyinUtil.ToPinyinInitials("人").Should().Be("r");
    }
}
