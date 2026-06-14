    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class PageRequestTests
// 定义类：PageRequestTests
public class PageRequestTests
{
    [Fact]
    // 方法 Default_ShouldHavePageNo1
    // 方法：Default_ShouldHavePageNo1
    public void Default_ShouldHavePageNo1()
    {
        // 创建 PageRequest实例并赋给 request
        var request = new PageRequest<object> { Entity = new() };
        // 断言验证
        request.PageNo.Should().Be(1);
        // 断言验证
        request.PageSize.Should().Be(20);
    }

    [Fact]
    // 方法 PageResult_ShouldStoreValues
    // 方法：PageResult_ShouldStoreValues
    public void PageResult_ShouldStoreValues()
    {
        // 创建 PageResult实例并赋给 result
        var result = new PageResult<string>
        {
            List = ["a", "b"],
            Total = 2,
            PageNo = 1,
            PageSize = 20
        };
        // 断言验证
        result.List.Count.Should().Be(2);
        // 断言验证
        result.Total.Should().Be(2);
    }
}
