using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class ApiResultTests
{
    [Fact]
    public void Ok_ShouldReturnSuccess()
    {
        var result = ApiResult.Ok();
        result.Code.Should().Be(200);
        result.Message.Should().Be("操作成功");
    }

    [Fact]
    public void Ok_WithData_ShouldReturnData()
    {
        var result = ApiResult<string>.Ok("test");
        result.Code.Should().Be(200);
        result.Data.Should().Be("test");
    }

    [Fact]
    public void Error_ShouldReturnError()
    {
        var result = ApiResult.Error("出错了");
        result.Code.Should().Be(500);
        result.Message.Should().Be("出错了");
    }

    [Fact]
    public void NotFound_ShouldReturn404()
    {
        var result = ApiResult.NotFound();
        result.Code.Should().Be(404);
        result.Message.Should().Be("资源不存在");
    }

    [Fact]
    public void Unauthorized_ShouldReturn401()
    {
        var result = ApiResult.Unauthorized();
        result.Code.Should().Be(401);
    }

    [Fact]
    public void Forbidden_ShouldReturn403()
    {
        var result = ApiResult.Forbidden();
        result.Code.Should().Be(403);
    }
}
