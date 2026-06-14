    // 引入 JeeSiteNET.Core.Security 命名空间
// 引入命名空间：JeeSiteNET.Core.Security
using JeeSiteNET.Core.Security;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;
    // 引入 Moq 命名空间
// 引入命名空间：Moq
using Moq;

// 定义 JeeSiteNET.Core.Tests 命名空间
// 定义命名空间：JeeSiteNET.Core.Tests
namespace JeeSiteNET.Core.Tests;

// 定义class DataScopeServiceTests
// 定义类：DataScopeServiceTests
public class DataScopeServiceTests
{
    // 方法：CreateService
    private static DataScopeService CreateService(
        // 声明并初始化变量：isSuperAdmin
        bool isSuperAdmin = false,
        string? orgCode = null,
        // 声明并初始化变量：userCode
        string userCode = "u1",
        List<DataScopeRule>? rules = null)
    {
        // 创建 Mock实例并赋给 user
        var user = new Mock<ICurrentUser>();
        user.Setup(u => u.IsSuperAdmin).Returns(isSuperAdmin);
        user.Setup(u => u.OrgCode).Returns(orgCode);
        user.Setup(u => u.UserCode).Returns(userCode);

        // 创建 Mock实例并赋给 provider
        var provider = new Mock<IDataScopeRuleProvider>();
        provider.Setup(p => p.GetRules(It.IsAny<ICurrentUser>(), It.IsAny<string>()))
            // null 合并操作 ??（若为 null 则使用右侧值）
            .Returns(rules ?? []);

        // return 返回结果
        return new DataScopeService(user.Object, provider.Object);
    }

    [Fact]
    // 方法 SuperAdmin_ShouldReturnAll
    // 方法：SuperAdmin_ShouldReturnAll
    public void SuperAdmin_ShouldReturnAll()
    {
        // 声明并初始化变量：service
        var service = CreateService(isSuperAdmin: true);
        // 创建 List实例并赋给 query
        var query = new List<TestEntity> { new() { OrgCode = "org1" } }.AsQueryable();
        // 声明并初始化变量：result
        var result = service.ApplyDataScope(query, "Test");
        // 数据库操作：统计数量
        result.Count().Should().Be(1);
    }

    [Fact]
    // 方法 NoRules_ShouldReturnAll
    // 方法：NoRules_ShouldReturnAll
    public void NoRules_ShouldReturnAll()
    {
        // 声明并初始化变量：service
        var service = CreateService();
        // 创建 List实例并赋给 query
        var query = new List<TestEntity> { new() { OrgCode = "org1" } }.AsQueryable();
        // 声明并初始化变量：result
        var result = service.ApplyDataScope(query, "Test");
        // 数据库操作：统计数量
        result.Count().Should().Be(1);
    }

    [Fact]
    // 方法 SelfScope_ShouldFilterByCreateBy
    // 方法：SelfScope_ShouldFilterByCreateBy
    public void SelfScope_ShouldFilterByCreateBy()
    {
        // 声明并初始化变量：service
        var service = CreateService(userCode: "u1", rules:
        [
            new DataScopeRule { ScopeType = DataScopeType.Self }
        ]);
        // 创建 List实例并赋给 data
        var data = new List<TestEntity>
        {
            new() { CreateBy = "u1" },
            new() { CreateBy = "u2" }
        };
        // 声明并初始化变量：result
        var result = service.ApplyDataScope(data.AsQueryable(), "Test");
        // 数据库操作：统计数量
        result.Count().Should().Be(1);
        // 数据库操作：取首条
        result.First().CreateBy.Should().Be("u1");
    }

    [Fact]
    // 方法 CompanyScope_ShouldFilterByOrgCode
    // 方法：CompanyScope_ShouldFilterByOrgCode
    public void CompanyScope_ShouldFilterByOrgCode()
    {
        // 声明并初始化变量：service
        var service = CreateService(orgCode: "org1", rules:
        [
            new DataScopeRule { ScopeType = DataScopeType.Company }
        ]);
        // 创建 List实例并赋给 data
        var data = new List<TestEntity>
        {
            new() { OrgCode = "org1" },
            new() { OrgCode = "org2" }
        };
        // 声明并初始化变量：result
        var result = service.ApplyDataScope(data.AsQueryable(), "Test");
        // 数据库操作：统计数量
        result.Count().Should().Be(1);
        // 数据库操作：取首条
        result.First().OrgCode.Should().Be("org1");
    }

    // 定义类：TestEntity
    private class TestEntity
    {
        // 属性：OrgCode
        public string? OrgCode { get; set; }
        // 属性：ParentCodes
        public string? ParentCodes { get; set; }
        // 属性：CreateBy
        public string? CreateBy { get; set; }
    }
}
