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

    [Fact]
    // 方法 PageNo_ShouldClampToMinimum1
    // 方法：PageNo_ShouldClampToMinimum1
    public void PageNo_ShouldClampToMinimum1()
    {
        // 非法页码（0/负数）应被钳制为 1，防止分页 Skip 负值崩溃
        var request = new PageRequest { PageNo = 0, PageSize = 0 };
        // 断言验证
        request.PageNo.Should().Be(1);
        request.PageSize.Should().Be(1);

        // 断言验证
        var neg = new PageRequest { PageNo = -5 };
        neg.PageNo.Should().Be(1);
    }

    [Fact]
    // 方法 PageSize_ShouldClampToMaximum200
    // 方法：PageSize_ShouldClampToMaximum200
    public void PageSize_ShouldClampToMaximum200()
    {
        // 过大页大小应被钳制为 200，避免异常分页/性能问题
        var request = new PageRequest { PageSize = 9999 };
        // 断言验证
        request.PageSize.Should().Be(200);
    }
}
