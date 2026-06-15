using System.Data;
using System.Data.Common;
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;

// ================================================================
// 数据库自省（Introspection）提供者
//
// 用途：代码生成器通过此服务连上已有数据库，读取表结构和列信息，
//       自动生成实体类、仓储、服务、控制器和 Vue 页面的脚手架代码。
//
// 数据库支持：
//   ① SQL Server（SqlConnection）
//   ② SQLite（SqliteConnection）
//   ③ PostgreSQL / GaussDB / KingbaseES（NpgsqlConnection）
//
// 根据 conn.GetType().Name 自动判断数据库类型，选择对应的 SQL 语句。
// 因为代码生成器需要连用户指定的数据库读取结构，所以注入 DbContext 基类，
// 通过 _db.Database.GetDbConnection() 获取原生连接直接执行 SQL。
//
// 调用链路：
//   CodeGenService → IDbIntrospectionProvider.FindDbTablesAsync / FindDbColumnsAsync
//   → GenTableController（导入表、预览、生成、下载 ZIP）
//
// 注册位置：CodeGenModuleInstaller.cs（AddScoped<IDbIntrospectionProvider, DbIntrospectionProvider>）
// ================================================================

public interface IDbIntrospectionProvider
{
    /// <summary>查询数据库中所有用户表的列表（表名 + 注释）。</summary>
    Task<List<DbTableInfo>> FindDbTablesAsync();

    /// <summary>查询指定表的所有列信息（列名 + 类型 + 注释 + 可空 + 长度）。</summary>
    Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName);
}

/// <summary>数据库自省实现。根据连接类型自动选择 SQL 方言。</summary>
public class DbIntrospectionProvider : IDbIntrospectionProvider
{
    private readonly DbContext _db;

    public DbIntrospectionProvider(DbContext db) => _db = db;

    /// <summary>查询数据库中所有用户表。</summary>
    public async Task<List<DbTableInfo>> FindDbTablesAsync()
    {
        using var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = GetListTablesSql();
        using var reader = await cmd.ExecuteReaderAsync();
        var tables = new List<DbTableInfo>();
        while (await reader.ReadAsync())
        {
            tables.Add(new DbTableInfo
            {
                TableName = reader.GetString(0),
                TableComment = reader.IsDBNull(1) ? null : reader.GetString(1)
            });
        }
        return tables;
    }

    /// <summary>查询指定表的所有列信息。</summary>
    public async Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName)
    {
        using var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = GetListColumnsSql();
        AddParameter(cmd, "tableName", tableName);
        using var reader = await cmd.ExecuteReaderAsync();
        var columns = new List<ColumnInfo>();

        // SQLite 使用 PRAGMA table_info 返回格式不同
        if (conn.GetType().Name == "SqliteConnection")
        {
            // PRAGMA table_info 列顺序：cid, name, type, notnull, dflt_value, pk
            while (await reader.ReadAsync())
            {
                var colName = reader.GetString(1);
                var dataType = reader.IsDBNull(2) ? "text" : reader.GetString(2).ToLower();
                columns.Add(new ColumnInfo
                {
                    ColumnName = colName,
                    ColumnComment = colName,
                    ColumnType = dataType,
                    NetType = MapToNetType(dataType),
                    IsNullable = reader.GetInt32(3) == 0 ? "0" : "1",
                    MaxLength = null
                });
            }
        }
        else
        {
            // SQL Server / PostgreSQL 使用 INFORMATION_SCHEMA 列顺序一致
            while (await reader.ReadAsync())
            {
                var dataType = reader.GetString(2).ToLower();
                columns.Add(new ColumnInfo
                {
                    ColumnName = reader.GetString(0),
                    ColumnComment = reader.IsDBNull(1) ? null : reader.GetString(1),
                    ColumnType = dataType,
                    NetType = MapToNetType(dataType),
                    IsNullable = reader.GetString(3) == "YES" ? "1" : "0",
                    MaxLength = reader.IsDBNull(4) ? null : (int?)Convert.ToInt32(reader.GetValue(4))
                });
            }
        }
        return columns;
    }

    /// <summary>根据数据库类型返回查询表列表的 SQL。</summary>
    private string GetListTablesSql()
    {
        var connType = _db.Database.GetDbConnection().GetType().Name;
        return connType switch
        {
            // SQL Server：从 INFORMATION_SCHEMA 查表，左连 sys.extended_properties 获取表注释
            "SqlConnection" => @"
                SELECT t.TABLE_NAME, ISNULL(ep.value, '') AS TABLE_COMMENT
                FROM INFORMATION_SCHEMA.TABLES t
                LEFT JOIN sys.tables st ON st.name = t.TABLE_NAME
                LEFT JOIN sys.extended_properties ep
                    ON ep.major_id = st.object_id AND ep.minor_id = 0 AND ep.name = 'MS_Description'
                WHERE t.TABLE_TYPE = 'BASE TABLE' AND t.TABLE_SCHEMA = 'dbo'
                ORDER BY t.TABLE_NAME",

            // SQLite：从 sqlite_master 查表定义
            "SqliteConnection" => @"
                SELECT m.name AS TABLE_NAME, COALESCE(s.sql, '') AS TABLE_COMMENT
                FROM sqlite_master m
                LEFT JOIN sqlite_master s ON s.type = 'table' AND s.name = m.name
                WHERE m.type = 'table' AND m.name NOT LIKE 'sqlite_%' AND m.name NOT LIKE '__EFMigrations%'
                ORDER BY m.name",

            // PostgreSQL 系列：通过 pg_catalog.obj_description 获取注释
            _ => @"
                SELECT t.table_name, COALESCE(pg_catalog.obj_description(c.oid), '') AS TABLE_COMMENT
                FROM information_schema.tables t
                LEFT JOIN pg_catalog.pg_class c ON c.relname = t.table_name
                WHERE t.table_schema = 'public' AND t.table_type = 'BASE TABLE'
                ORDER BY t.table_name"
        };
    }

    /// <summary>根据数据库类型返回查询列信息的 SQL。</summary>
    private string GetListColumnsSql()
    {
        var connType = _db.Database.GetDbConnection().GetType().Name;
        return connType switch
        {
            "SqlConnection" => @"
                SELECT c.COLUMN_NAME, ISNULL(ep.value, '') AS COLUMN_COMMENT,
                       c.DATA_TYPE, c.IS_NULLABLE, c.CHARACTER_MAXIMUM_LENGTH
                FROM INFORMATION_SCHEMA.COLUMNS c
                LEFT JOIN sys.columns sc
                    ON sc.name = c.COLUMN_NAME AND OBJECT_ID('[' + c.TABLE_NAME + ']') = sc.object_id
                LEFT JOIN sys.extended_properties ep
                    ON ep.major_id = sc.object_id AND ep.minor_id = sc.column_id AND ep.name = 'MS_Description'
                WHERE c.TABLE_NAME = @tableName AND c.TABLE_SCHEMA = 'dbo'
                ORDER BY c.ORDINAL_POSITION",

            "SqliteConnection" => @"PRAGMA table_info(@tableName)",

            _ => @"
                SELECT c.column_name, COALESCE(pg_catalog.col_description(c.oid, a.attnum), '') AS COLUMN_COMMENT,
                       c.data_type, c.is_nullable, c.character_maximum_length
                FROM information_schema.columns c
                JOIN pg_catalog.pg_class cls ON cls.relname = c.table_name
                JOIN pg_catalog.pg_attribute a ON a.attrelid = cls.oid AND a.attname = c.column_name
                WHERE c.table_name = @tableName AND c.table_schema = 'public'
                ORDER BY c.ordinal_position"
        };
    }

    /// <summary>给 DbCommand 添加参数。</summary>
    private static void AddParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    /// <summary>将 SQL 数据库类型映射为 C# 类型名称。</summary>
    private static string MapToNetType(string sqlType)
    {
        var type = sqlType.ToLowerInvariant().Split('(')[0].Trim();
        return type switch
        {
            "bigint" or "int8" => "long",
            "binary" or "varbinary" or "blob" or "bytea" or "image" => "byte[]",
            "bit" or "boolean" or "bool" => "bool",
            "char" or "nchar" or "ntext" or "nvarchar" or "text" or "varchar"
                or "character varying" or "character" or "clob" or "longtext"
                or "tinytext" or "mediumtext" or "longtext" => "string",
            "date" or "datetime" or "datetime2" or "smalldatetime"
                or "timestamp" or "timestamp without time zone" => "DateTime",
            "datetimeoffset" or "timestamptz" or "timestamp with time zone" => "DateTimeOffset",
            "decimal" or "money" or "numeric" or "smallmoney" or "number" => "decimal",
            "float" or "double" or "double precision" => "double",
            "int" or "integer" or "int4" or "mediumint" => "int",
            "real" => "float",
            "smallint" or "int2" => "short",
            "time" => "TimeSpan",
            "tinyint" => "byte",
            "uniqueidentifier" or "uuid" => "Guid",
            _ => "string"
        };
    }
}
