<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---
# 工具类手册 · Excel 导入导出工具

本章介绍 `ExcelService`（基于 NPOI / EPPlus 的通用 Excel 导入导出）、`ExcelFieldAttribute`（列注解）以及自定义字段类型体系（`IExcelFieldType`）。

---

## 一、ExcelService

`ExcelService`（`Utils/ExcelService.cs`）提供通用数据到 Excel 的双向转换。它使用反射 + Attribute 标注，让导入导出的定义集中在 DTO 本身，业务代码简洁。

### 1.1 依赖

```xml
<PackageReference Include="NPOI" Version="2.7.*" />        <!-- .xlsx / .xls 读写 -->
<PackageReference Include="EPPlus" Version="7.*" />        <!-- 备选（商业授权需确认） -->
```

> NPOI 支持 .xlsx（Office 2007+）和旧版 .xls，免费使用且在中文环境中稳定，这里以 NPOI 为默认实现。

### 1.2 核心方法

**（1）导出为 Excel**
```csharp
public async Task<byte[]> ExportAsync<T>(
    IEnumerable<T> data,
    string sheetName = "Sheet1")
```
- 使用 `T` 类上的 `[ExcelField]` 注解构建列结构。
- 对每条数据逐行写入。
- 返回内存中的 `byte[]`，控制器可直接返回 `File(bytes, MimeTypeExcel, fileName)`。

**（2）从 Excel 导入**
```csharp
public async Task<ImportResult<T>> ImportAsync<T>(Stream excelStream)
where T : new()
```
- 读取 Excel 的第一个工作表。
- 按 `[ExcelField]` 的 `ColumnIndex` / 列名匹配单元格。
- 每个单元格通过对应的 `IExcelFieldType` 转换为 C# 对象。

**（3）生成导入模板**
```csharp
public byte[] GetTemplate<T>()
```
- 生成仅包含表头（来自 `[ExcelField].Name`）和一行批注（示例值）的空模板。
- 方便运营下载后填充再回传。

**（4）导入汇总信息**
```csharp
public class ImportResult<T>
{
    public List<T> Data { get; set; }
    public List<string> Errors { get; set; }     // 每行错误信息
    public int TotalRows { get; set; }
    public int SuccessRows { get; set; }
    public int FailedRows { get; set; }
}
```

### 1.3 简单使用示例

**导出用户列表：**
```csharp
// Modules/Sys/Controllers/ExcelController.cs
public async Task<IActionResult> ExportUsers()
{
    var data = await _userService.GetListAsync<UserImportDto>();
    var bytes = await _excelService.ExportAsync(data, "用户列表");
    return File(
        bytes,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "用户列表.xlsx");
}
```

**导入用户列表：**
```csharp
public async Task<IActionResult> ImportUsers(IFormFile file)
{
    using var stream = file.OpenReadStream();
    var result = await _excelService.ImportAsync<UserImportDto>(stream);

    if (result.FailedRows > 0)
    {
        return BadRequest(new
        {
            message = $"共 {result.TotalRows} 行，成功 {result.SuccessRows}，失败 {result.FailedRows}",
            errors  = result.Errors
        });
    }
    await _userService.BatchInsertAsync(result.Data);
    return Ok(new { total = result.TotalRows });
}
```

**下载模板：**
```csharp
public IActionResult DownloadTemplate(string entityType)
{
    var bytes = _excelService.GetTemplate<UserImportDto>();
    return File(bytes, MimeTypeExcel, "用户导入模板.xlsx");
}
```

---

## 二、ExcelFieldAttribute（列注解）

通过在 DTO 的属性上标注 `[ExcelField]`，一次定义即完成"列名、列顺序、必填、字典翻译、字段类型"等全部配置。

### 2.1 定义

```csharp
// Utils/ExcelFieldAttribute.cs
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ExcelFieldAttribute : Attribute
{
    public string Name { get; set; }                  // 列显示名称
    public int ColumnIndex { get; set; } = -1;        // 列顺序（从 0 开始）
    public bool IsRequired { get; set; }              // 必填（导入时报错）
    public string DictType { get; set; }              // 字典类型（sys_dict_type.code）
    public Type FieldType { get; set; }               // 自定义字段类型（IExcelFieldType）
    public string Format { get; set; }                // 格式化字符串（导出时用）
    public int Width { get; set; } = 20;              // 列宽（字符）
    public bool IgnoreExport { get; set; }            // 仅导入，不导出
    public bool IgnoreImport { get; set; }            // 仅导出，不导入
}
```

### 2.2 示例 DTO

```csharp
// Modules/Sys/Application/DTOs/UserImportDto.cs
public class UserImportDto
{
    [ExcelField("用户账号", IsRequired = true, ColumnIndex = 0, Width = 18)]
    public string UserCode { get; set; }

    [ExcelField("姓名", ColumnIndex = 1, Width = 15)]
    public string UserName { get; set; }

    [ExcelField("手机号", ColumnIndex = 2, Width = 15)]
    public string Mobile { get; set; }

    [ExcelField("邮箱", ColumnIndex = 3, Width = 25)]
    public string Email { get; set; }

    [ExcelField("状态",
        DictType = "sys_user_status",
        ColumnIndex = 4)]
    // 导入时："启用" → "0"、"禁用" → "1"
    // 导出时："0"     → "启用"、"1" → "禁用"
    public string Status { get; set; }

    [ExcelField("创建时间",
        FieldType = typeof(DateTimeFieldType),
        Format = "yyyy-MM-dd HH:mm:ss",
        ColumnIndex = 5)]
    public DateTime CreateDate { get; set; }

    [ExcelField("余额",
        FieldType = typeof(MoneyFieldType),
        Format = "￥#,##0.00",
        ColumnIndex = 6)]
    public decimal Balance { get; set; }

    [ExcelField("性别",
        FieldType = typeof(BoolFieldType),        // "是/否" 或 "男/女"
        ColumnIndex = 7)]
    public string Gender { get; set; }

    [ExcelField("所属机构",
        FieldType = typeof(OfficeFieldType),
        ColumnIndex = 8)]
    public string OfficeId { get; set; }            // 导入时中文机构名称 → 机构 ID
}
```

---

## 三、IExcelFieldType（自定义字段类型体系）

每个自定义字段类型实现**两个方向**的转换：
- **导入方向**：Excel 单元格（字符串 / 数字 / 日期）→ C# 属性值
- **导出方向**：C# 属性值 → Excel 单元格（字符串 / 数字 / 日期）

### 3.1 接口定义

```csharp
// Utils/ExcelFieldTypes/IExcelFieldType.cs
public interface IExcelFieldType
{
    // 导入：Excel 单元格 → C# 值
    object Import(object cellValue, string param);

    // 导出：C# 值 → Excel 单元格
    object Export(object value, string param);
}
```

### 3.2 内置字段类型一览

| 类型 | 导入方向（Excel Cell → C#） | 导出方向（C# → Excel Cell） |
|------|-----------------------------|----------------------------|
| `DecimalFieldType` | `decimal.Parse(cell)` | `value.ToString()` |
| `MoneyFieldType` | `decimal.Parse(cell)` + 去除货币符号（`$` / `￥`） | `value.ToString("C")` 或按 `Format` |
| `DateTimeFieldType` | `DateTime.Parse(cell)` 或识别 Excel 日期序列值 | `value.ToString("yyyy-MM-dd HH:mm:ss")` 或按 `Format` |
| `DateFieldType` | 同上，仅保留 Date 部分 | `value.ToString("yyyy-MM-dd")` |
| `DictFieldType` | 字典标签 → 字典值（查 `sys_dict_data`） | 字典值 → 字典标签 |
| `BoolFieldType` | `"是"/"否"`、`"Y"/"N"`、`"男"/"女"`、`true/false` → `"0"/"1"` 或 `bool` | `"0"/"1"` → `"是"/"否"` |
| `EnumFieldType` | 枚举字符串或底层数字 → `Enum` | `Enum` → 显示名称 |
| `UserFieldType` | 用户名 / 工号 → `userId`（查 `sys_user`） | `userId` → 用户名 |
| `OfficeFieldType` | 机构名称 → `officeId`（查 `sys_office`） | `officeId` → 机构全路径名称 |
| `CompanyFieldType` | 公司名称 → `companyId` | `companyId` → 公司名称 |
| `ImageFieldType` | 图片路径 / Base64 → 字节数组 或下载 URL | 字节数组 / URL → 在 Excel 中插入图片 |

### 3.3 内置类型实现示例：BoolFieldType

```csharp
public class BoolFieldType : IExcelFieldType
{
    private static readonly Dictionary<string, string> TrueMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["是"] = "1", ["Y"] = "1", ["YES"] = "1",
            ["TRUE"] = "1", ["男"] = "1", ["启用"] = "1"
        };

    private static readonly Dictionary<string, string> FalseMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["否"] = "0", ["N"] = "0", ["NO"] = "0",
            ["FALSE"] = "0", ["女"] = "0", ["禁用"] = "0"
        };

    public object Import(object cellValue, string param)
    {
        var s = cellValue?.ToString()?.Trim() ?? "";
        if (TrueMap.TryGetValue(s, out var t)) return t;
        if (FalseMap.TryGetValue(s, out var f)) return f;
        throw new FormatException($"无法将 '{s}' 解析为布尔值");
    }

    public object Export(object value, string param)
    {
        // param 可选："yesno" / "gender" / "enable"，默认 "是/否"
        return value?.ToString() == "1" || value?.ToString() == "True" ? "是" : "否";
    }
}
```

---

## 四、自定义字段类型扩展步骤

当需要对接新的业务数据（如 "项目编号 → 项目名称"、"仓库编码 → 仓库名称"）时，按以下三步完成扩展。

### 步骤 1：实现 `IExcelFieldType`

```csharp
public class ProjectFieldType : IExcelFieldType
{
    private readonly IProjectRepository _projects;
    // 建议通过静态缓存 / 服务定位器 解析依赖

    public object Import(object cellValue, string param)
    {
        var name = cellValue?.ToString()?.Trim();
        var proj = _projects.FindByName(name);
        if (proj == null)
            throw new FormatException($"项目名称 '{name}' 不存在");
        return proj.Id;
    }

    public object Export(object value, string param)
    {
        var id = value?.ToString();
        var proj = _projects.FindById(id);
        return proj?.Name ?? "";
    }
}
```

### 步骤 2：在 DTO 的 `[ExcelField]` 中指定

```csharp
[ExcelField("项目", FieldType = typeof(ProjectFieldType), ColumnIndex = 10)]
public string ProjectId { get; set; }
```

### 步骤 3：编译后即生效

`ExcelService` 通过 `Activator.CreateInstance(fieldType)` 动态构造类型实例，反射调用 `Import` / `Export`。

---

## 五、完整使用示例（导入 + 导出）

### 5.1 定义 DTO

```csharp
public class ProductImportDto
{
    [ExcelField("SKU", IsRequired = true, ColumnIndex = 0)]
    public string Sku { get; set; }

    [ExcelField("商品名称", ColumnIndex = 1)]
    public string Name { get; set; }

    [ExcelField("分类", DictType = "product_category", ColumnIndex = 2)]
    public string CategoryCode { get; set; }

    [ExcelField("单价", FieldType = typeof(MoneyFieldType), ColumnIndex = 3)]
    public decimal Price { get; set; }

    [ExcelField("上架日期", FieldType = typeof(DateFieldType), ColumnIndex = 4)]
    public DateTime LaunchDate { get; set; }

    [ExcelField("负责人", FieldType = typeof(UserFieldType), ColumnIndex = 5)]
    public string OwnerId { get; set; }
}
```

### 5.2 导出

```csharp
List<ProductImportDto> list = await _productService.GetListAsync();
byte[] bytes = await _excelService.ExportAsync(list, "商品列表");
return File(bytes, MimeTypeExcel, "products.xlsx");
```

### 5.3 导入

```csharp
using var stream = Request.Form.Files[0].OpenReadStream();
var result = await _excelService.ImportAsync<ProductImportDto>(stream);

if (result.Errors.Any())
{
    // 返回每行的错误信息，便于运营修正
    return BadRequest(new { result.Errors });
}
await _productService.BatchInsertAsync(result.Data);
return Ok(new { imported = result.SuccessRows });
```

### 5.4 下载模板

```csharp
byte[] template = _excelService.GetTemplate<ProductImportDto>();
return File(template, MimeTypeExcel, "商品导入模板.xlsx");
```

---

## 六、控制器端点（ExcelController）

典型路由结构：

```text
POST /api/v1/sys/excel/import/{entityType}     → 上传 Excel → 解析 → 批量写入
GET  /api/v1/sys/excel/export/{entityType}     → 按筛选条件查询 → 导出 Excel
GET  /api/v1/sys/excel/template/{entityType}   → 下载空白导入模板
```

实现要点：

```csharp
// Modules/Sys/Controllers/ExcelController.cs
[ApiController]
[Route("api/v1/sys/excel")]
public class ExcelController : ControllerBase
{
    private readonly ExcelService _excelService;
    private readonly IEntityTypeRegistry _entityRegistry;  // entityType → DTO Type

    [HttpPost("import/{entityType}")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Import(
        string entityType,
        IFormFile file)
    {
        var dtoType = _entityRegistry.Resolve(entityType);
        // 使用反射构造泛型方法 ImportAsync<>
        var method = typeof(ExcelService).GetMethod("ImportAsync")
            .MakeGenericMethod(dtoType);

        dynamic result = await (dynamic)method.Invoke(
            _excelService,
            new object[] { file.OpenReadStream() });

        if (result.FailedRows > 0)
            return BadRequest(new { result.Errors, result.TotalRows });

        // 批量写入（此处简化）
        return Ok(new { result.TotalRows, result.SuccessRows });
    }

    [HttpGet("export/{entityType}")]
    public async Task<IActionResult> Export(string entityType,
        [FromQuery] string keyword = null)
    {
        var dtoType = _entityRegistry.Resolve(entityType);
        // 根据 entityType 查找对应 Service 的 GetListAsync
        // ... 省略反射路由 ...
        byte[] bytes = /* ... */;
        return File(bytes, MimeTypeExcel, $"{entityType}.xlsx");
    }

    [HttpGet("template/{entityType}")]
    public IActionResult GetTemplate(string entityType)
    {
        var dtoType = _entityRegistry.Resolve(entityType);
        var method = typeof(ExcelService).GetMethod("GetTemplate")
            .MakeGenericMethod(dtoType);
        var bytes = (byte[])method.Invoke(_excelService, null);
        return File(bytes, MimeTypeExcel, $"{entityType}导入模板.xlsx");
    }
}
```

---

## 七、导入流程的防御性检查清单

使用 ExcelService 时，不要依赖 Excel 的格式"看起来正确"，建议在导入流程中追加以下验证：

1. [ ] **文件大小**：限制为 10MB（防止超大文件导致内存溢出）。
2. [ ] **扩展名校验**：仅接受 `.xlsx` / `.xls`。
3. [ ] **签名校验**：调用 `FileSecurityUtil.ValidateSignature` 校验 Excel 真的是 OLE2 / ZIP 结构。
4. [ ] **行数上限**：单文件限制为 1 万行（超出提示分批导入）。
5. [ ] **必填字段**：`[ExcelField(IsRequired = true)]` 的属性不能为空白。
6. [ ] **字典合法性**：字典值必须在 `sys_dict_data` 中存在。
7. [ ] **唯一约束**：如 SKU、工号等在导入前做重复检测。
8. [ ] **日期范围**：日期必须在合理区间（如 1900-01-01 ~ 当前日期）。
9. [ ] **权限检查**：当前用户必须对目标实体具有 `Import` 权限。
10. [ ] **审计日志**：记录导入人、时间、总行数、失败行数、错误内容。

---

## 八、字段类型与 Excel 单元格数据类型对应表

| C# 类型 | Excel 单元格类型（导出） | 导入时解析方式 |
|--------|-------------------------|----------------|
| `string` | String | `.ToString().Trim()` |
| `int` / `long` | Number | `int.TryParse` / `long.TryParse` |
| `decimal` / `double` | Number | `decimal.TryParse` |
| `DateTime` | Date（带 NumberFormat） | `DateTime.TryParse` 或识别 Excel 日期序列 |
| `bool` | Boolean / String（"是/否"） | `BoolFieldType` 映射 |
| `Enum` | String（显示名称） | `EnumFieldType` 映射 |
| `byte[]`（图片） | Picture（插入图片） | 暂不支持自动导入图片 |

---

> **相关文件**
> - `src/JeeSiteNET.Core/Utils/ExcelService.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelUtil.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldAttribute.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/IExcelFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/DecimalFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/MoneyFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/DateTimeFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/DateFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/DictFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/BoolFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/EnumFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/UserFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/OfficeFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/CompanyFieldType.cs`
> - `src/JeeSiteNET.Core/Utils/ExcelFieldTypes/ImageFieldType.cs`
> - `src/JeeSiteNET.Modules.Sys/Controllers/ExcelController.cs`
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>