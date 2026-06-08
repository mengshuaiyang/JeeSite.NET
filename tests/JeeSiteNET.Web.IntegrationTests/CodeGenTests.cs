using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace JeeSiteNET.Web.IntegrationTests;

public class CodeGenTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public CodeGenTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task List_Tables_ReturnsOk()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    public async Task Get_NonExistentTable_Returns404()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.GetAsync("/api/v1/codegen/table/get?tableName=nonexistent_table");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(404);
    }

    [Fact]
    public async Task Save_NewTableConfig_Succeeds()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/save", new
        {
            tableName = "Test_Table",
            className = "TestTable",
            moduleCode = "Test",
            functionName = "测试表",
            businessName = "test",
            tplCategory = "crud",
            columns = new[]
            {
                new
                {
                    columnId = "Test_Table.id",
                    tableName = "Test_Table",
                    columnName = "id",
                    columnComment = "主键",
                    columnType = "nvarchar(64)",
                    netType = "string",
                    propertyName = "Id",
                    columnSort = 10,
                    isPk = "1",
                    isNullable = "0",
                    maxLength = 64,
                    isInsert = "1",
                    isEdit = "1",
                    isList = "1",
                    isQuery = "1",
                    queryType = "EQ",
                    htmlType = "input",
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now
                },
                new
                {
                    columnId = "Test_Table.name",
                    tableName = "Test_Table",
                    columnName = "name",
                    columnComment = "名称",
                    columnType = "nvarchar(200)",
                    netType = "string",
                    propertyName = "Name",
                    columnSort = 20,
                    isPk = "0",
                    isNullable = "1",
                    maxLength = 200,
                    isInsert = "1",
                    isEdit = "1",
                    isList = "1",
                    isQuery = "1",
                    queryType = "LIKE",
                    htmlType = "input",
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now
                }
            }
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    public async Task Preview_ExistingTable_ReturnsTemplates()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        await client.PostAsJsonAsync("/api/v1/codegen/table/save", new
        {
            tableName = "Preview_Test",
            className = "PreviewTest",
            moduleCode = "Cms",
            functionName = "预览测试",
            businessName = "preview",
            tplCategory = "crud",
            columns = new[]
            {
                new
                {
                    columnId = "Preview_Test.id",
                    tableName = "Preview_Test",
                    columnName = "id",
                    columnComment = "主键",
                    columnType = "nvarchar(64)",
                    netType = "string",
                    propertyName = "Id",
                    columnSort = 10,
                    isPk = "1",
                    isNullable = "0",
                    maxLength = 64,
                    isInsert = "1",
                    isEdit = "0",
                    isList = "1",
                    isQuery = "1",
                    queryType = "EQ",
                    htmlType = "input",
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now
                },
                new
                {
                    columnId = "Preview_Test.title",
                    tableName = "Preview_Test",
                    columnName = "title",
                    columnComment = "标题",
                    columnType = "nvarchar(200)",
                    netType = "string",
                    propertyName = "Title",
                    columnSort = 20,
                    isPk = "0",
                    isNullable = "0",
                    maxLength = 200,
                    isInsert = "1",
                    isEdit = "1",
                    isList = "1",
                    isQuery = "1",
                    queryType = "LIKE",
                    htmlType = "input",
                    createDate = DateTime.Now,
                    updateDate = DateTime.Now
                }
            }
        });

        var response = await client.GetAsync("/api/v1/codegen/table/preview?tableName=Preview_Test");
        var body = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Expected 200 but got {body}");

        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        var data = doc.RootElement.GetProperty("data");
        data.GetArrayLength().Should().BeGreaterThan(0);

        var fileNames = data.EnumerateArray().Select(x => x.GetProperty("fileName").GetString()).ToList();
        fileNames.Should().Contain(f => f!.Contains("PreviewTest"));
    }

    [Fact]
    public async Task Delete_ExistingTable_Succeeds()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        await client.PostAsJsonAsync("/api/v1/codegen/table/save", new
        {
            tableName = "Delete_Test",
            className = "DeleteTest",
            moduleCode = "Test",
            functionName = "删除测试",
            businessName = "delete",
            tplCategory = "crud",
            columns = new[]
            {
                new
                {
                    columnId = "Delete_Test.id",
                    tableName = "Delete_Test",
                    columnName = "id",
                    columnComment = "主键",
                    columnType = "nvarchar(64)",
                    netType = "string",
                    propertyName = "Id",
                    columnSort = 10,
                    isPk = "1",
                    isNullable = "0",
                    maxLength = 64,
                    isInsert = "1", isEdit = "1", isList = "1", isQuery = "1",
                    queryType = "EQ", htmlType = "input",
                    createDate = DateTime.Now, updateDate = DateTime.Now
                }
            }
        });

        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/delete", new { tableName = "Delete_Test" });
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    public async Task UnauthenticatedAccess_Returns401()
    {
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
