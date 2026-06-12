<div align="right">
  <a href="Home">← 返回首页</a>
</div>

---
# CodeGen 代码生成器模块技术手册

## 一、模块概述

### 1.1 模块定位

CodeGen 模块是 JeeSite.NET 的代码生成器，目标是将数据库中的一张业务表 → 生成完整的三层代码（实体/仓储/服务/控制器/DTO）以及前端列表页与编辑页。它不提供直接的业务能力，而是显著提升新业务模块的开发效率：开发者只需在数据库建表，再通过 CodeGen 界面配置「表信息 + 列选项」，即可得到可用的 C# 与 Vue 脚手架代码，之后再在生成代码上进行业务定制。

模块所在程序集：`JeeSiteNET.Modules.CodeGen`
API 根路径：`/api/v1/codegen/`
前端入口：`frontend/src/views/codegen/`

### 1.2 生成内容清单

| 生成产物 | 位置（相对项目） | 典型命名 |
|----------|------------------|---------|
| 实体类 | `modules/{ModuleName}/Domain/Entities/` | `{EntityName}.cs` |
| DTO | `modules/{ModuleName}/Application/DTOs/` | `{EntityName}Dto.cs` |
| 仓储接口 | `modules/{ModuleName}/Domain/Interfaces/` | `I{EntityName}Repository.cs` |
| 仓储实现 | `modules/{ModuleName}/Infrastructure/Repositories/` | `{EntityName}Repository.cs` |
| 应用服务 | `modules/{ModuleName}/Application/Services/` | `{EntityName}Service.cs` |
| 控制器 | `modules/{ModuleName}/Controllers/` | `{EntityName}Controller.cs` |
| 实体配置 | `modules/{ModuleName}/Infrastructure/EntityConfigurations/` | `{EntityName}Configuration.cs` |
| Vue 列表页 | `frontend/src/views/{moduleName}/` | `{entityName}List.vue` |
| Vue 编辑页 | `frontend/src/views/{moduleName}/` | `{entityName}Edit.vue` |

### 1.3 技术依赖

- **Scriban** 模板引擎：用于渲染生成模板字符串
- **数据库自省**：`DbIntrospectionProvider` 读取目标库的表结构、列信息、主键、注释
- **System.IO.Compression**：将所有生成的文本文件打包为 ZIP 供下载

模块在业务上不依赖其他业务模块，但它生成的代码产物会使用 `JeeSiteNET.Modules.Sys` 提供的实体基类、仓储接口。

## 二、核心实体

### 2.1 GenTable（表配置）

命名空间：`JeeSiteNET.Modules.CodeGen.Domain.Entities.GenTable`

| 字段 | 类型 | 含义 |
|------|------|------|
| Id | long | 主键 |
| TableName | string | 数据库表名，如 `biz_customer` |
| EntityName | string | C# 实体类名，如 `Customer` |
| EntityClass | string | 实体完整类名（命名空间+类名） |
| Namespace | string | 生成代码的根命名空间，如 `JeeSiteNET.Modules.Biz` |
| ModuleName | string | 模块名，同时也是生成路径的一级目录，如 `Biz` |
| ModuleCode | string | 模块代码，与 `sys_module.module_code` 对齐 |
| FunctionName | string | 功能描述，如「客户管理」 |
| BusinessDescription | string | 业务描述，用于生成文件注释 |
| Author | string | 作者，写入生成文件头部 |
| TablePrefix | string | 生成类名时需要去除的表前缀，如 `biz_` |
| IsPage | bool | 是否分页（列表页是否带分页） |
| IsTree | bool | 是否树表（当表存在 `parent_code` 等字段时自动识别） |
| PrimaryKey | string | 主键列名，如 `id` 或 `code` |
| Remarks | string | 备注 |
| CreateDate | DateTime | 创建时间 |
| UpdateDate | DateTime | 更新时间 |
| Columns | ICollection<GenTableColumn> | 列配置导航属性 |

### 2.2 GenTableColumn（列配置）

命名空间：`JeeSiteNET.Modules.CodeGen.Domain.Entities.GenTableColumn`

| 字段 | 类型 | 含义 |
|------|------|------|
| Id | long | 主键 |
| GenTableId | long | 所属表（外键） |
| ColumnName | string | 数据库列名（如 `customer_name`） |
| PropertyName | string | C# 属性名（如 `CustomerName`） |
| ColumnDescription | string | 字段中文注释，用于生成标签和帮助文本 |
| DbType | string | 数据库类型，如 `varchar(64)` |
| CsharpType | string | C# 类型，如 `string`、`long`、`DateTime`、`decimal` |
| IsList | bool | 是否在列表页显示此列 |
| IsEdit | bool | 是否在编辑页可编辑此列 |
| IsQuery | bool | 是否作为查询条件 |
| IsRequired | bool | 是否必填（生成 `[Required]` 或前端校验） |
| QueryType | string | 查询方式，支持 `EQ` / `LIKE` / `GT` / `GTE` / `LT` / `LTE` / `BETWEEN` |
| HtmlControlType | string | 页面控件类型，支持 `input` / `textarea` / `select` / `radio` / `checkbox` / `date` / `datetime` / `imageEditor` / `fileUpload` |
| DictType | string | 字典类型（当控件为 select/radio/checkbox 时使用，从 `sys_dict_data` 读取选项） |
| DefaultValue | string | 默认值（字符串形式） |
| SortNo | int | 排序号，数值小在前 |
| IsPrimaryKey | bool | 是否主键列 |
| IsAutoIncrement | bool | 是否自增 |

### 2.3 DTO 概览

- **GenTableDto / GenTableListDto**：表配置的创建、编辑、列表展示数据载体
- **GenTableColumnDto**：列配置数据载体（新建/编辑表时整表提交）
- **GenConfigDto**：导入表与查询时的请求参数（分页、关键词过滤）

## 三、核心服务与流程

### 3.1 GenTableService（表管理）

命名空间：`JeeSiteNET.Modules.CodeGen.Application.Services.GenTableService`

#### 主要方法

| 方法 | 签名 | 说明 |
|------|------|------|
| GetPagedListAsync | `(GenConfigDto input) -> PagedList<GenTable>` | 已配置表的分页列表 |
| GetByIdAsync | `(long id) -> GenTable?` | 获取表详情 |
| GetTableColumnsAsync | `(long tableId) -> List<GenTableColumn>` | 获取/填充列配置 |
| GetTableOptions | `(long tableId) -> object` | 预览配置（表+列） |
| ImportTablesAsync | `(string? connectionString, string? provider, List<string>? tableNames) -> List<GenTable>` | 从数据库读取表结构并创建配置记录 |
| SaveTableAsync | `(GenTableDto dto) -> GenTable` | 保存表及列配置 |
| DeleteAsync | `(long tableId)` | 删除配置（连同列配置） |
| GenerateZipAsync | `(long tableId) -> (byte[], string)` | 生成 ZIP 字节流与文件名 |

#### 导入流程

1. 如果传入了 `connectionString`，直接使用；否则使用当前 `DbContext` 的连接串
2. 根据 `provider` 选择对应 `DbIntrospectionProvider`
3. 枚举表列表，按表名过滤（`tableNames` 可选）
4. 对每张表：读取列信息 → 识别主键 → 根据列名/注释推断 C# 属性名、类型、是否列表/编辑/查询
5. 写入数据库（未存在则新增，已存在则跳过或由调用方决定）

### 3.2 CodeGenService（代码生成）

命名空间：`JeeSiteNET.Modules.CodeGen.Application.Services.CodeGenService`

负责把 `GenTable` + `GenTableColumn` 配置转换为各个文本文件。

| 方法 | 签名 | 说明 |
|------|------|------|
| GenerateAllAsync | `(GenTable table) -> List<GeneratedFile>` | 生成所有代码文件（实体/DTO/仓储/服务/控制器/实体配置/Vue 列表/Vue 编辑） |
| GenerateEntityAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | C# 实体类 |
| GenerateDtoAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | DTO（列表/创建/编辑） |
| GenerateServiceAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | 应用服务（增删改查） |
| GenerateRepositoryAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | 仓储接口 + 仓储实现（通常两个文件同方法返回或分别生成） |
| GenerateControllerAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | 控制器 |
| GenerateEntityConfigurationAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | EF Core 实体配置 |
| GenerateVueListAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | Vue 3 列表页（带查询/分页） |
| GenerateVueEditAsync | `(GenTable table, List<GenTableColumn> columns) -> GeneratedFile` | Vue 3 编辑页（带必填/字典） |
| GenerateZip | `(IEnumerable<GeneratedFile> files) -> byte[]` | 将文本文件打包为 ZIP |

**说明**：

- 每个 `GenerateXxxAsync` 内部调用 `CodeGenTemplates` 的对应属性获取 Scriban 模板
- 传入 `table`、`columns`、`primaryKey` 作为模板变量
- 返回 `GeneratedFile`（包含 `RelativePath` 与 `Content`）用于 ZIP 打包或预览

### 3.3 CodeGenTemplates（模板管理）

命名空间：`JeeSiteNET.Modules.CodeGen.Application.Services.CodeGenTemplates`

这是一组 C# 属性（或静态成员），每个返回一个 Scriban 模板字符串。模板引擎使用 `{{ ... }}` 语法插值与条件判断。

| 属性 | 产物 |
|------|------|
| EntityTemplate | C# 实体类 |
| DtoTemplate | DTO（列表、创建、编辑） |
| ServiceTemplate | 应用服务 |
| RepositoryTemplate | 仓储接口与实现 |
| ControllerTemplate | 控制器 |
| EntityConfigurationTemplate | EF Core 实体配置 |
| VueListTemplate | Vue 列表页 |
| VueEditTemplate | Vue 编辑页 |

#### 模板可用变量

```
{{ table.TableName }}          数据库表名
{{ table.EntityName }}         实体类名
{{ table.Namespace }}          根命名空间
{{ table.ModuleName }}         模块名（小写形式用于路由）
{{ table.FunctionName }}       功能描述
{{ table.Author }}             作者
{{ table.PrimaryKey }}         主键列名
{{ table.IsTree }}             是否树表
{{ columns }}                  列列表（IEnumerable<GenTableColumn>）
{{ primaryKey }}               主键列对象（快捷变量）
```

列对象常用字段：

```
{{ column.ColumnName }}        数据库列名
{{ column.PropertyName }}      C# 属性名
{{ column.ColumnDescription }} 注释
{{ column.CsharpType }}        C# 类型
{{ column.IsList }}            是否列表
{{ column.IsEdit }}            是否编辑
{{ column.IsQuery }}           是否查询
{{ column.IsRequired }}        是否必填
{{ column.QueryType }}         查询方式（EQ/LIKE/...）
{{ column.HtmlControlType }}   控件类型
{{ column.DictType }}          字典类型
```

### 3.4 DbIntrospectionProvider（数据库自省）

命名空间：`JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection.DbIntrospectionProvider`

负责解析数据库中的表元数据。不同 Provider（SqlServer/PostgreSql/MySql）可能存在派生实现。核心能力：

- `ListTablesAsync(connectionString)`：返回表名列表
- `GetColumnsAsync(connectionString, tableName)`：返回列信息（列名、类型、是否可空、注释、是否主键、是否自增）

## 四、控制器与 API

控制器：`JeeSiteNET.Modules.CodeGen.Controllers.GenTableController`

| 方法 | HTTP | 路由 | 说明 |
|------|------|------|------|
| GetListAsync | GET | `/api/v1/codegen/gen-table/list` | 已配置表分页列表 |
| GetAsync | GET | `/api/v1/codegen/gen-table/{id}` | 表详情 + 列配置 |
| ImportAsync | POST | `/api/v1/codegen/gen-table/import` | 从数据库导入 |
| SaveAsync | PUT | `/api/v1/codegen/gen-table/{id}` | 保存表与列配置 |
| DeleteAsync | DELETE | `/api/v1/codegen/gen-table/{id}` | 删除配置 |
| PreviewAsync | GET | `/api/v1/codegen/gen-table/preview/{id}` | 预览生成的代码（JSON，键=路径，值=内容） |
| DownloadAsync | GET | `/api/v1/codegen/gen-table/download/{id}` | 下载 ZIP 文件（`application/zip`） |

### 4.1 请求体示例：ImportAsync

```json
{
  "connectionString": null,
  "provider": "SqlServer",
  "tableNames": ["biz_customer", "biz_order"]
}
```

### 4.2 请求体示例：SaveAsync

```json
{
  "id": 1,
  "tableName": "biz_customer",
  "entityName": "Customer",
  "namespace": "JeeSiteNET.Modules.Biz",
  "moduleName": "Biz",
  "functionName": "客户管理",
  "businessDescription": "客户基础信息维护",
  "author": "admin",
  "tablePrefix": "biz_",
  "isPage": true,
  "isTree": false,
  "primaryKey": "code",
  "columns": [
    {
      "id": 1,
      "columnName": "code",
      "propertyName": "Code",
      "columnDescription": "客户编码",
      "csharpType": "string",
      "isList": true, "isEdit": true, "isQuery": true, "isRequired": true,
      "queryType": "EQ",
      "htmlControlType": "input",
      "dictType": null,
      "isPrimaryKey": true,
      "sortNo": 1
    }
  ]
}
```

## 五、使用步骤

1. **建业务表**。在数据库中创建业务表（命名建议：`biz_xxx` / `{module}_xxx`），为每个字段写中文注释。
2. **导入表**。进入「代码生成 → 表管理」，点击「从数据库导入」，选择目标表。
3. **配置表信息**。点击「配置」，填写：
   - `EntityName`：实体类名（Pascal 命名）
   - `Namespace`：根命名空间（如 `JeeSiteNET.Modules.Biz`）
   - `ModuleName`：模块名（如 `Biz`；影响生成路径与路由）
   - `FunctionName`：功能描述
   - `Author`、`TablePrefix`、`IsPage`、`IsTree`、`PrimaryKey`
4. **配置列**。为每一列调整：
   - 是否在列表展示（`IsList`）
   - 是否可编辑（`IsEdit`）
   - 是否为查询条件（`IsQuery`）及查询方式（`QueryType`）
   - 控件类型（`HtmlControlType`，字典类型列注意配 `DictType`）
   - 是否必填（`IsRequired`）
   - 排序号（`SortNo`）
5. **预览**。点击「预览」查看各文件生成内容是否符合预期。
6. **下载**。点击「下载」获取 ZIP 文件，解压后：
   - 将 C# 文件拷贝到 `modules/{ModuleName}/` 对应目录
   - 将 Vue 文件拷贝到 `frontend/src/views/{moduleName}/`
   - 在前端 `api/` 目录新增对应接口封装文件
7. **编译与接入**。
   - 后端：`dotnet build` 编译；若模块为新增，还需要在 `ModuleInstaller` 注册 `DbSet`/Service 等
   - 前端：`pnpm dev` 启动
   - 菜单：在 `sys_menu` 表插入菜单记录（`menu_type = 1` 菜单，`menu_href = views/{module}/{entityName}List`，`permission = {module}:{entity}:list/view/edit/add/delete`）

## 六、表设计规范

### 6.1 命名

- 表名：下划线 + 业务前缀，如 `biz_customer`、`cms_article`
- 列名：下划线命名，如 `customer_name`、`create_by`
- 主键：推荐 `code`（字符串业务主键）或 `id`（long 自增）

### 6.2 审计字段（推荐）

| 字段 | 类型 | 作用 |
|------|------|------|
| create_by | varchar(64) | 创建人 user_code |
| create_date | datetime | 创建时间 |
| update_by | varchar(64) | 更新人 |
| update_date | datetime | 更新时间 |
| status | char(1) | 状态（0 正常，1 停用） |
| is_deleted | bit / tinyint | 软删除标记 |
| remarks | varchar(500) | 备注 |

这些字段不是必须，但 CodeGen 会在生成代码中适配它们。

### 6.3 树形表（`IsTree = true`）

当表满足以下字段约定时 CodeGen 会将其识别为树表，并使用 `TreeEntity` 风格的生成模板：

| 字段 | 含义 |
|------|------|
| parent_code | 父级 code |
| tree_sort | 同级排序（字符串） |
| tree_sorts | 祖先路径（如 `0001,0020`） |
| tree_leaf | 是否叶子节点 |
| tree_level | 层级（0=根） |

### 6.4 注释

- 所有字段必须添加中文列注释（`COMMENT 'xxx'`），CodeGen 会将其用作页面标签与生成代码注释。

### 6.5 示例建表 SQL（SqlServer）

```sql
CREATE TABLE biz_customer (
    code         VARCHAR(32)  NOT NULL CONSTRAINT pk_biz_customer PRIMARY KEY,
    customer_name VARCHAR(100) NOT NULL,
    contact_phone VARCHAR(20),
    email        VARCHAR(100),
    address      VARCHAR(200),
    status       CHAR(1)      NOT NULL DEFAULT '0',
    create_by    VARCHAR(64),
    create_date  DATETIME2    NOT NULL DEFAULT GETDATE(),
    update_by    VARCHAR(64),
    update_date  DATETIME2,
    is_deleted   BIT          NOT NULL DEFAULT 0,
    remarks      VARCHAR(500)
);

EXEC sp_addextendedproperty 'MS_Description', '客户编码', 'SCHEMA', 'dbo', 'TABLE', 'biz_customer', 'COLUMN', 'code';
EXEC sp_addextendedproperty 'MS_Description', '客户名称', 'SCHEMA', 'dbo', 'TABLE', 'biz_customer', 'COLUMN', 'customer_name';
```

## 七、自定义模板扩展

### 7.1 修改默认模板

1. 打开 `modules/JeeSiteNET.Modules.CodeGen/Application/Services/CodeGenTemplates.cs`
2. 在对应属性的模板字符串中修改 Scriban 语法（例如 `EntityTemplate` 控制实体类生成）
3. 重新编译 `JeeSiteNET.Modules.CodeGen` 即可生效

### 7.2 常用 Scriban 写法

```
{{- for column in columns -}}
{{-   if column.IsList -}} ... {{- end -}}
{{- end -}}

{{ if table.IsTree }}
// 树表特有代码
{{ end }}

{{- primaryKey.PropertyName -}}          // 主键属性名
{{ column.ColumnDescription | string.replace "'", "''" }}   // 字符串转义
```

### 7.3 新增生成文件

1. 在 `CodeGenTemplates` 中新增对应属性（如 `EnumTemplate`）
2. 在 `CodeGenService` 新增 `GenerateEnumAsync`，将模板渲染结果包装为 `GeneratedFile`
3. 在 `GenerateAllAsync` 调用新增方法
4. 重新编译

### 7.4 常见变量速查

```
{{ table.EntityName }}       Customer
{{ table.TableName }}        biz_customer
{{ table.Namespace }}        JeeSiteNET.Modules.Biz
{{ table.ModuleName }}       Biz
{{ table.FunctionName }}     客户管理
{{ column.ColumnName }}      customer_name
{{ column.PropertyName }}    CustomerName
{{ column.CsharpType }}      string
{{ column.IsList }}          true
{{ column.IsEdit }}          true
{{ column.IsQuery }}         true
{{ column.IsRequired }}      true
{{ column.QueryType }}       EQ
{{ column.HtmlControlType }} input
{{ column.DictType }}        null
{{ column.SortNo }}          1
```

## 八、代码生成后的步骤清单

生成的代码可直接运行，但还需要完成以下接入步骤：

1. **拷贝后端代码**：将生成的 C# 文件放入 `modules/{ModuleName}/` 的 `Domain/Entities`、`Domain/Interfaces`、`Application/DTOs`、`Application/Services`、`Infrastructure/Repositories`、`Infrastructure/EntityConfigurations`、`Controllers` 等子目录。
2. **注册 `DbSet`**：如果模块首次加入，在 `{Module}ModuleInstaller` 中注册 `DbSet<Entity>`、服务（`services.AddScoped<IService, Service>`）。
3. **迁移数据库**：若新增实体需要自动建表，执行 `dotnet ef migrations add Add{Entity}` / `dotnet ef database update`（项目使用自己的迁移方案时走对应流程）。
4. **拷贝前端代码**：将 Vue 文件放入 `frontend/src/views/{moduleName}/`。
5. **新增前端 API 封装**：在 `frontend/src/api/` 下新增 `{entityName}.ts`，与后端控制器路由对齐。
6. **新增菜单**：在 `sys_menu` 表插入目录/菜单记录，示例：

   ```sql
   INSERT INTO sys_menu(menu_code, parent_code, menu_name, menu_type, menu_href, permission, sort_no, status)
   VALUES ('biz_customer_list', 'biz', '客户管理', '1', 'views/biz/customerList', 'biz:customer:list', 10, '0');
   ```

7. **新增字典数据**（可选）：若业务字段使用字典类型（如 `status`、`leave_type`），在 `sys_dict_data` 添加字典项。
8. **编译测试**：
   - 后端：`dotnet build` / `dotnet run --project src/Host`
   - 前端：`pnpm dev`
9. **浏览器验证**：登录后进入「客户管理」菜单，验证列表、查询、新增、编辑、删除。

### 8.1 常见问题处理

- **编译报错「找不到 EntityBase」**：确认实体引用了 `JeeSiteNET.Core.Domain.Entities` 的基类，或使用 `TreeEntity` 基类（树形表）。
- **路由 404**：控制器是否已被 `ModuleInstaller` 注册或已被 `AddApplicationPart` 发现；前端菜单 `menu_href` 是否与路由匹配。
- **字典下拉无选项**：`DictType` 拼写错误或 `sys_dict_data` 无该类型数据。
- **树表展示异常**：列名是否按树形表字段约定命名，或 `IsTree` 开关未打开。
- **权限不足**：新增菜单后为角色分配权限（`role_menu` 或 `role_data_scope`），并重新登录。

## 九、模块主要文件结构

```
modules/JeeSiteNET.Modules.CodeGen/
├─ Application/
│   ├─ DTOs/
│   │   ├─ GenConfigDto.cs
│   │   ├─ GenTableColumnDto.cs
│   │   └─ GenTableDto.cs
│   └─ Services/
│       ├─ CodeGenService.cs
│       ├─ CodeGenTemplates.cs
│       └─ GenTableService.cs
├─ Controllers/
│   └─ GenTableController.cs
├─ Domain/
│   ├─ Entities/
│   │   ├─ GenTable.cs
│   │   └─ GenTableColumn.cs
│   └─ Interfaces/
│       ├─ IGenTableRepository.cs
│       └─ IGenTableColumnRepository.cs
├─ Infrastructure/
│   ├─ EntityConfigurations/
│   │   ├─ GenTableConfiguration.cs
│   │   └─ GenTableColumnConfiguration.cs
│   ├─ Introspection/
│   │   └─ DbIntrospectionProvider.cs
│   └─ Repositories/
│       ├─ GenTableRepository.cs
│       └─ GenTableColumnRepository.cs
├─ CodeGenModuleInstaller.cs
└─ JeeSiteNET.Modules.CodeGen.csproj

frontend/src/
├─ api/
│   └─ codegen.ts
└─ views/codegen/
    ├─ GenTableList.vue
    └─ GenTableEdit.vue
```
---

<div align="center">
  <small>本文档最后更新: 2026-06-12 · JeeSite.NET Wiki</small>
</div>