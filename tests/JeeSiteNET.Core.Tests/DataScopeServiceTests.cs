using JeeSiteNET.Core.Security;
using FluentAssertions;
using Moq;

namespace JeeSiteNET.Core.Tests;

public class DataScopeServiceTests
{
    private static DataScopeService CreateService(
        bool isSuperAdmin = false,
        string? orgCode = null,
        string userCode = "u1",
        List<DataScopeRule>? rules = null)
    {
        var user = new Mock<ICurrentUser>();
        user.Setup(u => u.IsSuperAdmin).Returns(isSuperAdmin);
        user.Setup(u => u.OrgCode).Returns(orgCode);
        user.Setup(u => u.UserCode).Returns(userCode);

        var provider = new Mock<IDataScopeRuleProvider>();
        provider.Setup(p => p.GetRules(It.IsAny<ICurrentUser>(), It.IsAny<string>()))
            .Returns(rules ?? []);

        return new DataScopeService(user.Object, provider.Object);
    }

    [Fact]
    public void SuperAdmin_ShouldReturnAll()
    {
        var service = CreateService(isSuperAdmin: true);
        var query = new List<TestEntity> { new() { OrgCode = "org1" } }.AsQueryable();
        var result = service.ApplyDataScope(query, "Test");
        result.Count().Should().Be(1);
    }

    [Fact]
    public void NoRules_ShouldReturnAll()
    {
        var service = CreateService();
        var query = new List<TestEntity> { new() { OrgCode = "org1" } }.AsQueryable();
        var result = service.ApplyDataScope(query, "Test");
        result.Count().Should().Be(1);
    }

    [Fact]
    public void SelfScope_ShouldFilterByCreateBy()
    {
        var service = CreateService(userCode: "u1", rules:
        [
            new DataScopeRule { ScopeType = DataScopeType.Self }
        ]);
        var data = new List<TestEntity>
        {
            new() { CreateBy = "u1" },
            new() { CreateBy = "u2" }
        };
        var result = service.ApplyDataScope(data.AsQueryable(), "Test");
        result.Count().Should().Be(1);
        result.First().CreateBy.Should().Be("u1");
    }

    [Fact]
    public void CompanyScope_ShouldFilterByOrgCode()
    {
        var service = CreateService(orgCode: "org1", rules:
        [
            new DataScopeRule { ScopeType = DataScopeType.Company }
        ]);
        var data = new List<TestEntity>
        {
            new() { OrgCode = "org1" },
            new() { OrgCode = "org2" }
        };
        var result = service.ApplyDataScope(data.AsQueryable(), "Test");
        result.Count().Should().Be(1);
        result.First().OrgCode.Should().Be("org1");
    }

    private class TestEntity
    {
        public string? OrgCode { get; set; }
        public string? ParentCodes { get; set; }
        public string? CreateBy { get; set; }
    }
}
