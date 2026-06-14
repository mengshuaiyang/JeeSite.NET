    // 引入 System.Net.Http.Json 命名空间
// 引入命名空间：System.Net.Http.Json
using System.Net.Http.Json;
    // 引入 System.Text.Json 命名空间
// 引入命名空间：System.Text.Json
using System.Text.Json;
    // 引入 FluentAssertions 命名空间
// 引入命名空间：FluentAssertions
using FluentAssertions;

// 定义 JeeSiteNET.Web.IntegrationTests 命名空间
// 定义命名空间：JeeSiteNET.Web.IntegrationTests
namespace JeeSiteNET.Web.IntegrationTests;

// 定义class CmsFrontTests
// 定义类：CmsFrontTests
public class CmsFrontTests : IClassFixture<CustomWebApplicationFactory>
{
    // 字段 _factory
    // 字段：_factory
    private readonly CustomWebApplicationFactory _factory;

    // 方法 CmsFrontTests
    // 构造函数：CmsFrontTests
    public CmsFrontTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    // 方法 GetSites_ReturnsList
    // 方法：GetSites_ReturnsList
    public async Task GetSites_ReturnsList()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/cms/front/site");

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    // 方法 GetCategories_WithNonExistentSite_ReturnsEmpty
    // 方法：GetCategories_WithNonExistentSite_ReturnsEmpty
    public async Task GetCategories_WithNonExistentSite_ReturnsEmpty()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/cms/front/category/list/nonexistent_site");

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        // 断言验证
        doc.RootElement.GetProperty("data").GetArrayLength().Should().Be(0);
    }

    [Fact]
    // 方法 ArticleList_ReturnsEmpty
    // 方法：ArticleList_ReturnsEmpty
    public async Task ArticleList_ReturnsEmpty()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/cms/front/article/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        // 断言验证
        doc.RootElement.GetProperty("data").GetProperty("total").GetInt32().Should().Be(0);
    }

    [Fact]
    // 方法 ArticleGet_NonExistent_Returns404
    // 方法：ArticleGet_NonExistent_Returns404
    public async Task ArticleGet_NonExistent_Returns404()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/cms/front/article/get/nonexistent_article");

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(404);
    }

    [Fact]
    // 方法 ArticleSearch_EmptyKeyword_ReturnsEmpty
    // 方法：ArticleSearch_EmptyKeyword_ReturnsEmpty
    public async Task ArticleSearch_EmptyKeyword_ReturnsEmpty()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/cms/front/article/search", new
        {
            pageNo = 1,
            pageSize = 10,
            entity = new { }
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    // 方法 TagCloud_ReturnsList
    // 方法：TagCloud_ReturnsList
    public async Task TagCloud_ReturnsList()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/cms/front/tag/cloud");

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }
}
