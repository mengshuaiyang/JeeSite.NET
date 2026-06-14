// 引入命名空间：System.Data
using System.Data;
// 引入命名空间：System.Data.Common
using System.Data.Common;
// 引入命名空间：JeeSiteNET.Modules.CodeGen.Application.DTOs
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
// 引入命名空间：Microsoft.EntityFrameworkCore
using Microsoft.EntityFrameworkCore;

// 定义命名空间：JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection
namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;

// 定义接口：IDbIntrospectionProvider
public interface IDbIntrospectionProvider
{
    Task<List<DbTableInfo>> FindDbTablesAsync();
    Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName);
}

// 定义类：DbIntrospectionProvider
public class DbIntrospectionProvider : IDbIntrospectionProvider
{
    // 字段：_db
    private readonly DbContext _db;

    // 构造函数：DbIntrospectionProvider
    public DbIntrospectionProvider(DbContext db) => _db = db;

    // 方法：FindDbTablesAsync
    public async Task<List<DbTableInfo>> FindDbTablesAsync()
    {
        using var conn = _db.Database.GetDbConnection();
        // await 异步等待
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = GetListTablesSql();
        using var reader = await cmd.ExecuteReaderAsync();
        // 创建 List实例并赋给 tables
        var tables = new List<DbTableInfo>();
        // while 循环
        while (await reader.ReadAsync())
        {
            // 集合操作：添加元素
            tables.Add(new DbTableInfo
            {
                TableName = reader.GetString(0),
                TableComment = reader.IsDBNull(1) ? null : reader.GetString(1)
            });
        }
        // return 返回结果
        return tables;
    }

    // 方法：FindDbColumnsAsync
    public async Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName)
    {
        using var conn = _db.Database.GetDbConnection();
        // await 异步等待
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = GetListColumnsSql();
        AddParameter(cmd, "tableName", tableName);
        using var reader = await cmd.ExecuteReaderAsync();
        // 创建 List实例并赋给 columns
        var columns = new List<ColumnInfo>();

        // if 条件判断
        if (conn.GetType().Name == "SqliteConnection")
        {
            // PRAGMA table_info returns: cid, name, type, notnull, dflt_value, pk
            // while 循环
            while (await reader.ReadAsync())
            {
                // 声明并初始化变量：colName
                var colName = reader.GetString(1);
                // 调用 ToLower
                var dataType = reader.IsDBNull(2) ? "text" : reader.GetString(2).ToLower();
                // 集合操作：添加元素
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
        // else 否则分支
        else
        {
            // while 循环
            while (await reader.ReadAsync())
            {
                // 调用 ToLower
                var dataType = reader.GetString(2).ToLower();
                // 集合操作：添加元素
                columns.Add(new ColumnInfo
                {
                    ColumnName = reader.GetString(0),
                    ColumnComment = reader.IsDBNull(1) ? null : reader.GetString(1),
                    ColumnType = dataType,
                    NetType = MapToNetType(dataType),
                    IsNullable = reader.GetString(3) == "YES" ? "1" : "0",
                    // 读取配置项
                    MaxLength = reader.IsDBNull(4) ? null : (int?)Convert.ToInt32(reader.GetValue(4))
                });
            }
        }
        // return 返回结果
        return columns;
    }

    // 方法：GetListTablesSql
    private string GetListTablesSql()
    {
        // 声明并初始化变量：connType
        var connType = _db.Database.GetDbConnection().GetType().Name;
        // return 返回结果
        return connType switch
        {
            "SqlConnection" => @"
                SELECT t.TABLE_NAME, ISNULL(ep.value, '') AS TABLE_COMMENT
                FROM INFORMATION_SCHEMA.TABLES t
                LEFT JOIN sys.tables st ON st.name = t.TABLE_NAME
                LEFT JOIN sys.extended_properties ep
                    ON ep.major_id = st.object_id AND ep.minor_id = 0 AND ep.name = 'MS_Description'
                WHERE t.TABLE_TYPE = 'BASE TABLE' AND t.TABLE_SCHEMA = 'dbo'
                ORDER BY t.TABLE_NAME",

            "SqliteConnection" => @"
                SELECT m.name AS TABLE_NAME, COALESCE(s.sql, '') AS TABLE_COMMENT
                FROM sqlite_master m
                LEFT JOIN sqlite_master s ON s.type = 'table' AND s.name = m.name
                WHERE m.type = 'table' AND m.name NOT LIKE 'sqlite_%' AND m.name NOT LIKE '__EFMigrations%'
                ORDER BY m.name",

            // NpgsqlConnection for PostgreSQL / GaussDB / KingbaseES
            _ => @"
                SELECT t.table_name, COALESCE(pg_catalog.obj_description(c.oid), '') AS TABLE_COMMENT
                FROM information_schema.tables t
                LEFT JOIN pg_catalog.pg_class c ON c.relname = t.table_name
                WHERE t.table_schema = 'public' AND t.table_type = 'BASE TABLE'
                ORDER BY t.table_name"
        };
    }

    // 方法：GetListColumnsSql
    private string GetListColumnsSql()
    {
        // 声明并初始化变量：connType
        var connType = _db.Database.GetDbConnection().GetType().Name;
        // return 返回结果
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

            "SqliteConnection" => @"
                PRAGMA table_info(@tableName)",

            // NpgsqlConnection for PostgreSQL / GaussDB / KingbaseES
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

    // 方法：AddParameter
    private static void AddParameter(DbCommand cmd, string name, object value)
    {
        // 声明并初始化变量：param
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        // 集合操作：添加元素
        cmd.Parameters.Add(param);
    }

    // 方法：MapToNetType
    private static string MapToNetType(string sqlType)
    {
        // 调用 Split
        var type = sqlType.ToLowerInvariant().Split('(')[0].Trim();
        // return 返回结果
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
