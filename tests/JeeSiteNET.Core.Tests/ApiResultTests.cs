    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class ApiResultTests
// 定义类：ApiResultTests
public class ApiResultTests
{
    [Fact]
    // 方法 Ok_ShouldReturnSuccess
    // 方法：Ok_ShouldReturnSuccess
    public void Ok_ShouldReturnSuccess()
    {
        // 声明并初始化变量：result
        var result = ApiResult.Ok();
        // 断言验证
        result.Code.Should().Be(200);
        // 断言验证
        result.Message.Should().Be("操作成功");
    }

    [Fact]
    // 方法 Ok_WithData_ShouldReturnData
    // 方法：Ok_WithData_ShouldReturnData
    public void Ok_WithData_ShouldReturnData()
    {
        // 声明并初始化变量：result
        var result = ApiResult<string>.Ok("test");
        // 断言验证
        result.Code.Should().Be(200);
        // 断言验证
        result.Data.Should().Be("test");
    }

    [Fact]
    // 方法 Error_ShouldReturnError
    // 方法：Error_ShouldReturnError
    public void Error_ShouldReturnError()
    {
        // 声明并初始化变量：result
        var result = ApiResult.Error("出错了");
        // 断言验证
        result.Code.Should().Be(500);
        // 断言验证
        result.Message.Should().Be("出错了");
    }

    [Fact]
    // 方法 NotFound_ShouldReturn404
    // 方法：NotFound_ShouldReturn404
    public void NotFound_ShouldReturn404()
    {
        // 声明并初始化变量：result
        var result = ApiResult.NotFound();
        // 断言验证
        result.Code.Should().Be(404);
        // 断言验证
        result.Message.Should().Be("资源不存在");
    }

    [Fact]
    // 方法 Unauthorized_ShouldReturn401
    // 方法：Unauthorized_ShouldReturn401
    public void Unauthorized_ShouldReturn401()
    {
        // 声明并初始化变量：result
        var result = ApiResult.Unauthorized();
        // 断言验证
        result.Code.Should().Be(401);
    }

    [Fact]
    // 方法 Forbidden_ShouldReturn403
    // 方法：Forbidden_ShouldReturn403
    public void Forbidden_ShouldReturn403()
    {
        // 声明并初始化变量：result
        var result = ApiResult.Forbidden();
        // 断言验证
        result.Code.Should().Be(403);
    }
}
