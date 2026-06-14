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

// 定义class CodeGenTests
// 定义类：CodeGenTests
public class CodeGenTests : IClassFixture<CustomWebApplicationFactory>
{
    // 字段 _factory
    // 字段：_factory
    private readonly CustomWebApplicationFactory _factory;

    // 方法 CodeGenTests
    // 构造函数：CodeGenTests
    public CodeGenTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    // 方法 List_Tables_ReturnsOk
    // 方法：List_Tables_ReturnsOk
    public async Task List_Tables_ReturnsOk()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/list", new
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
    }

    [Fact]
    // 方法 Get_NonExistentTable_Returns404
    // 方法：Get_NonExistentTable_Returns404
    public async Task Get_NonExistentTable_Returns404()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();
        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/codegen/table/get?tableName=nonexistent_table");

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(404);
    }

    [Fact]
    // 方法 Save_NewTableConfig_Succeeds
    // 方法：Save_NewTableConfig_Succeeds
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

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    // 方法 Preview_ExistingTable_ReturnsTemplates
    // 方法：Preview_ExistingTable_ReturnsTemplates
    public async Task Preview_ExistingTable_ReturnsTemplates()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        // await 异步等待
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

        // 缓存：获取值
        var response = await client.GetAsync("/api/v1/codegen/table/preview?tableName=Preview_Test");
        var body = await response.Content.ReadAsStringAsync();
        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, $"Expected 200 but got {body}");

    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(body);
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
        // 声明并初始化变量：data
        var data = doc.RootElement.GetProperty("data");
        // 断言验证
        data.GetArrayLength().Should().BeGreaterThan(0);

        // 数据库操作：投影选择
        var fileNames = data.EnumerateArray().Select(x => x.GetProperty("fileName").GetString()).ToList();
        // 集合操作：检查是否包含
        fileNames.Should().Contain(f => f!.Contains("PreviewTest"));
    }

    [Fact]
    // 方法 Delete_ExistingTable_Succeeds
    // 方法：Delete_ExistingTable_Succeeds
    public async Task Delete_ExistingTable_Succeeds()
    {
        var client = await _factory.CreateAuthenticatedClientAsync();

        // await 异步等待
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
        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    // 引入 var doc 命名空间
        // 调用 Parse
        using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        // 断言验证
        doc.RootElement.GetProperty("code").GetInt32().Should().Be(200);
    }

    [Fact]
    // 方法 UnauthenticatedAccess_Returns401
    // 方法：UnauthenticatedAccess_Returns401
    public async Task UnauthenticatedAccess_Returns401()
    {
    // 引入 var client 命名空间
        using var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/v1/codegen/table/list", new
        {
            pageNo = 1,
            pageSize = 10
        });

        // 断言验证
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
}
