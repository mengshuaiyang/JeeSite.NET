using System.Data;
using System.Data.Common;
using JeeSiteNET.Modules.CodeGen.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace JeeSiteNET.Modules.CodeGen.Infrastructure.Introspection;

public interface IDbIntrospectionProvider
{
    Task<List<DbTableInfo>> FindDbTablesAsync();
    Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName);
}

public class DbIntrospectionProvider : IDbIntrospectionProvider
{
    private readonly DbContext _db;

    public DbIntrospectionProvider(DbContext db) => _db = db;

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

    public async Task<List<ColumnInfo>> FindDbColumnsAsync(string tableName)
    {
        using var conn = _db.Database.GetDbConnection();
        await conn.OpenAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = GetListColumnsSql();
        AddParameter(cmd, "tableName", tableName);
        using var reader = await cmd.ExecuteReaderAsync();
        var columns = new List<ColumnInfo>();

        if (conn.GetType().Name == "SqliteConnection")
        {
            // PRAGMA table_info returns: cid, name, type, notnull, dflt_value, pk
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

    private string GetListTablesSql()
    {
        var connType = _db.Database.GetDbConnection().GetType().Name;
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

    private static void AddParameter(DbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

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
