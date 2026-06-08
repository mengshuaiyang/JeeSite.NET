using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace JeeSiteNET.Web.IntegrationTests;

public class CmsFrontTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CmsFrontTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetSites_ReturnsList()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/cms/front/site");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    public async Task GetCategories_WithNonExistentSite_ReturnsEmpty()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/cms/front/category/list/nonexistent_site");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        doc.RootElement.GetProperty("data").GetArrayLength().Should().Be(0);
    }

    [Fact]
    public async Task ArticleList_ReturnsEmpty()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/cms/front/article/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        doc.RootElement.GetProperty("data").GetProperty("total").GetInt32().Should().Be(0);
    }

    [Fact]
    public async Task ArticleGet_NonExistent_Returns404()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/cms/front/article/get/nonexistent_article");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(404);
    }

    [Fact]
    public async Task ArticleSearch_EmptyKeyword_ReturnsEmpty()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/cms/front/article/search", new
        {
            pageNo = 1,
            pageSize = 10,
            entity = new { }
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    public async Task TagCloud_ReturnsList()
    {
        using var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v1/cms/front/tag/cloud");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }
}
