using JeeSiteNET.Core;
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
using JeeSiteNET.Modules.CodeGen.Domain.Entities;
using JeeSiteNET.Modules.CodeGen.Domain.Interfaces;
using JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;
using Scriban;
using Scriban.Runtime;

namespace JeeSiteNET.Modules.CodeGen.Application.Services;

public class CodeGenService
{
    private readonly IDbIntrospectionProvider _introspection;
    private readonly IGenTableRepository _genTableRepository;
    private readonly IGenTableColumnRepository _genTableColumnRepository;

    public CodeGenService(IDbIntrospectionProvider introspection, IGenTableRepository genTableRepository, IGenTableColumnRepository genTableColumnRepository)
    {
        _introspection = introspection;
        _genTableRepository = genTableRepository;
        _genTableColumnRepository = genTableColumnRepository;
    }

    public Task<List<DbTableInfo>> FindDbTablesAsync() => _introspection.FindDbTablesAsync();

    public Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName) => _introspection.FindDbColumnsAsync(tableName);

    public async Task<ApiResult> ImportTablesAsync(ImportTableRequest request)
    {
        var now = DateTime.Now;
        foreach (var tableName in request.TableNames)
        {
            if (await _genTableRepository.GetAsync(tableName) != null)
                continue;

            var columns = await _introspection.FindDbColumnsAsync(tableName);
            var dbTable = (await _introspection.FindDbTablesAsync()).FirstOrDefault(t => t.TableName == tableName);
            var className = ToPascalCase(tableName);
            var moduleCode = className.Split('_').FirstOrDefault() ?? "Sys";
            var businessName = string.Join("_", className.Split('_').Skip(1)).ToLower();
            if (string.IsNullOrEmpty(businessName)) businessName = className.ToLower();

            var entity = new GenTable
            {
                TableName = tableName,
                ClassName = className,
                ModuleCode = moduleCode,
                FunctionName = dbTable?.TableComment ?? className,
                TableComment = dbTable?.TableComment,
                BusinessName = businessName,
                TplCategory = "crud",
                CreateDate = now,
                UpdateDate = now
            };

            int sort = 10;
            foreach (var col in columns)
            {
                var propName = ToPascalCase(col.ColumnName);
                entity.Columns.Add(new GenTableColumn
                {
                    ColumnId = $"{tableName}.{col.ColumnName}",
                    TableName = tableName,
                    ColumnName = col.ColumnName,
                    ColumnComment = col.ColumnComment ?? propName,
                    ColumnType = col.ColumnType,
                    NetType = col.NetType,
                    PropertyName = propName,
                    ColumnSort = sort,
                    IsPk = col.ColumnName.Equals("id", StringComparison.OrdinalIgnoreCase) ? "1" : "0",
                    IsNullable = col.IsNullable,
                    MaxLength = col.MaxLength,
                    IsInsert = "1",
                    IsEdit = "1",
                    IsList = "1",
                    IsQuery = "0",
                    QueryType = "EQ",
                    HtmlType = "input",
                    CreateDate = now,
                    UpdateDate = now
                });
                sort += 10;
            }
            await _genTableRepository.AddAsync(entity);
        }
        return ApiResult.Ok();
    }

    public async Task<List<GenPreviewItem>> PreviewAsync(string tableName)
    {
        var table = await _genTableRepository.GetWithColumnsAsync(tableName);
        if (table == null) return [];

        GenConfigDto cfg = new()
        {
            ModuleCode = table.ModuleCode,
            ClassName = table.ClassName,
            FunctionName = table.FunctionName ?? table.ClassName,
            BusinessName = table.BusinessName ?? table.ClassName.ToLower(),
            TplCategory = table.TplCategory ?? "crud"
        };

        var result = new List<GenPreviewItem>();
        var ctx = BuildTemplateContext(table, cfg);
        var tpl = cfg.TplCategory;

        result.Add(new() { FileName = $"Domain/Entities/{cfg.ClassName}.cs", Content = Render(tpl == "tree" ? "TreeEntity" : "Entity", ctx) });
        result.Add(new() { FileName = $"Infrastructure/EntityConfigurations/{cfg.ClassName}Configuration.cs", Content = Render(tpl == "tree" ? "TreeConfiguration" : "Configuration", ctx) });

        if (tpl == "tree")
            result.Add(new() { FileName = $"Application/DTOs/{cfg.ClassName}Dto.cs", Content = Render("TreeDto", ctx) });
        else
            result.Add(new() { FileName = $"Application/DTOs/{cfg.ClassName}Dto.cs", Content = Render("Dto", ctx) });

        result.Add(new() { FileName = $"Domain/Interfaces/I{cfg.ClassName}Repository.cs", Content = Render("RepositoryInterface", ctx) });
        result.Add(new() { FileName = $"Infrastructure/Repositories/{cfg.ClassName}Repository.cs", Content = Render("Repository", ctx) });

        string serviceTpl = tpl switch { "tree" => "TreeService", "query" => "QueryService", _ => "Service" };
        if (tpl != "query")
            result.Add(new() { FileName = $"Application/Services/{cfg.ClassName}Service.cs", Content = Render(serviceTpl, ctx) });

        if (tpl != "service")
        {
            string ctrlTpl = tpl switch { "tree" => "TreeController", "query" => "QueryController", _ => "Controller" };
            result.Add(new() { FileName = $"Controllers/{cfg.ClassName}Controller.cs", Content = Render(ctrlTpl, ctx) });
        }

        if (tpl != "query" && tpl != "service")
        {
            string vueTpl = tpl == "tree" ? "VueTree" : "VueList";
            result.Add(new() { FileName = $"frontend/src/views/{table.ModuleCode.ToLower()}/{table.BusinessName}/index.vue", Content = Render(vueTpl, ctx) });
        }

        result.Add(new() { FileName = $"{cfg.ClassName}ModuleInstaller.cs", Content = Render("ModuleInstaller", ctx) });

        return result;
    }

    public async Task<ApiResult> GenerateAsync(string tableName, string? outputDir)
    {
        if (string.IsNullOrEmpty(outputDir))
            outputDir = Path.Combine(Directory.GetCurrentDirectory(), "..", "generated");

        var items = await PreviewAsync(tableName);
        if (items.Count == 0) return ApiResult.NotFound("表配置不存在");

        foreach (var item in items)
        {
            var filePath = Path.Combine(outputDir, item.FileName);
            var dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            await File.WriteAllTextAsync(filePath, item.Content);
        }

        return ApiResult.Ok(items.Select(i => i.FileName).ToList());
    }

    public async Task<byte[]> DownloadAsync(string tableName)
    {
        var items = await PreviewAsync(tableName);
        if (items.Count == 0) return [];

        using var ms = new MemoryStream();
        using (var zip = new System.IO.Compression.ZipArchive(ms, System.IO.Compression.ZipArchiveMode.Create, true))
        {
            foreach (var item in items)
            {
                var entry = zip.CreateEntry(item.FileName);
                using var writer = new StreamWriter(entry.Open());
                await writer.WriteAsync(item.Content);
            }
        }
        return ms.ToArray();
    }

    private ScriptObject BuildTemplateContext(GenTable table, GenConfigDto cfg)
    {
        var pk = table.Columns.FirstOrDefault(c => c.IsPk == "1");
        var pkProp = pk?.PropertyName ?? "Id";
        var pkNetType = pk?.NetType ?? "string";
        var pkName = pk?.ColumnName ?? "id";

        var treeNameField = table.TreeName;
        if (string.IsNullOrEmpty(treeNameField))
        {
            treeNameField = table.Columns.FirstOrDefault(c =>
                c.PropertyName?.Equals("Name", StringComparison.OrdinalIgnoreCase) == true ||
                c.PropertyName?.Equals("TreeName", StringComparison.OrdinalIgnoreCase) == true)?.PropertyName;
        }
        if (string.IsNullOrEmpty(treeNameField))
        {
            treeNameField = table.Columns.FirstOrDefault(c =>
                c.NetType == "string" && c.IsPk == "0")?.PropertyName ?? "Name";
        }

        var so = new ScriptObject();
        so["tpl_category"] = cfg.TplCategory ?? "crud";
        so["class_name"] = cfg.ClassName ?? "MyEntity";
        so["module_namespace"] = $"JeeSiteNET.Modules.{cfg.ModuleCode}";
        so["module_code"] = cfg.ModuleCode;
        so["module_lower"] = cfg.ModuleCode.ToLower();
        so["function_name"] = cfg.FunctionName ?? "MyFunction";
        so["business_name"] = cfg.BusinessName ?? "my_business";
        so["table_name"] = table.TableName ?? "MyTable";
        so["base_class"] = cfg.TplCategory == "tree" ? "TreeEntity" : "DataEntity";
        so["tree_name_field"] = treeNameField ?? "Name";
        so["pk_name"] = pkProp;
        so["pk_net_type"] = pkNetType;
        so["permission_prefix"] = $"{cfg.ModuleCode.ToLower()}:{(cfg.BusinessName ?? "my_business").Replace("_", "")}";
        so["vue_pk"] = pkProp;
        so["vue_pk_name"] = pkName;
        so["columns"] = table.Columns.Where(c => c.Status == "0").OrderBy(c => c.ColumnSort).Select(c =>
        {
            var col = new ScriptObject();
            col["property_name"] = c.PropertyName ?? "";
            col["column_name"] = c.ColumnName ?? "";
            col["net_type"] = c.NetType ?? "string";
            col["is_nullable"] = c.IsNullable ?? "1";
            col["is_pk"] = c.IsPk ?? "0";
            col["is_insert"] = c.IsInsert ?? "1";
            col["is_edit"] = c.IsEdit ?? "1";
            col["is_list"] = c.IsList ?? "1";
            col["is_query"] = c.IsQuery ?? "0";
            col["max_length"] = c.MaxLength ?? 0;
            col["comment"] = c.ColumnComment ?? c.PropertyName ?? "";
            return col;
        }).ToList();
        so["vue_table_fields"] = table.Columns.Where(c => c.IsList == "1" && c.Status == "0").OrderBy(c => c.ColumnSort).Select(c =>
        {
            var col = new ScriptObject();
            col["property_name"] = c.PropertyName ?? "";
            col["comment"] = c.ColumnComment ?? c.PropertyName ?? "";
            return col;
        }).ToList();
        so["vue_form_fields"] = table.Columns.Where(c => c.IsEdit == "1" && c.IsPk == "0" && c.Status == "0").OrderBy(c => c.ColumnSort).Select(c =>
        {
            var col = new ScriptObject();
            col["property_name"] = c.PropertyName ?? "";
            col["comment"] = c.ColumnComment ?? c.PropertyName ?? "";
            return col;
        }).ToList();
        return so;
    }

    private static string Render(string templateName, ScriptObject model)
    {
        var source = templateName switch
        {
            "Entity" => CodeGenTemplates.Entity,
            "TreeEntity" => CodeGenTemplates.TreeEntity,
            "Configuration" => CodeGenTemplates.Configuration,
            "TreeConfiguration" => CodeGenTemplates.TreeConfiguration,
            "RepositoryInterface" => CodeGenTemplates.RepositoryInterface,
            "Repository" => CodeGenTemplates.Repository,
            "Service" => CodeGenTemplates.Service,
            "TreeService" => CodeGenTemplates.TreeService,
            "QueryService" => CodeGenTemplates.QueryService,
            "Dto" => CodeGenTemplates.Dto,
            "TreeDto" => CodeGenTemplates.TreeDto,
            "Controller" => CodeGenTemplates.Controller,
            "TreeController" => CodeGenTemplates.TreeController,
            "QueryController" => CodeGenTemplates.QueryController,
            "ModuleInstaller" => CodeGenTemplates.ModuleInstaller,
            "VueList" => CodeGenTemplates.VueList,
            "VueTree" => CodeGenTemplates.VueTree,
            _ => throw new ArgumentException($"未知模板: {templateName}")
        };
        var template = Template.Parse(source);
        return template.Render(model);
    }

    private static string ToPascalCase(string name)
    {
        if (string.IsNullOrEmpty(name)) return name;
        var parts = name.Split(['_', ' ', '-'], StringSplitOptions.RemoveEmptyEntries);
        return string.Join("", parts.Select(p => char.ToUpper(p[0]) + p[1..].ToLower()));
    }
}
