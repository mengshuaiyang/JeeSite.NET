using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class PageRequestTests
{
    [Fact]
    public void Default_ShouldHavePageNo1()
    {
        var request = new PageRequest<object> { Entity = new() };
        request.PageNo.Should().Be(1);
        request.PageSize.Should().Be(20);
    }

    [Fact]
    public void PageResult_ShouldStoreValues()
    {
        var result = new PageResult<string>
        {
            List = ["a", "b"],
            Total = 2,
            PageNo = 1,
            PageSize = 20
        };
        result.List.Count.Should().Be(2);
        result.Total.Should().Be(2);
    }
}
