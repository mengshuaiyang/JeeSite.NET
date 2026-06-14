using System.Text.Json;
using System.Text.Json.Serialization;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 协议标准响应对象：所有 action 均返回一个 JSON 对象；
/// 本类提供通用成功/错误工厂方法，派生类承载具体字段。
/// </summary>
public class UEditorResult
{
    /// <summary>
    /// 状态字段（通常为 "SUCCESS" 或错误描述），序列化为 JSON state 字段
    /// </summary>
    [JsonPropertyName("state")]
    public string State { get; set; } = "SUCCESS";

    /// <summary>
    /// 返回成功响应
    /// </summary>
    /// <typeparam name="T">Data 类型</typeparam>
    /// <param name="data">响应数据</param>
    /// <returns>State = SUCCESS 的强类型结果</returns>
    public static UEditorResult<T> Success<T>(T data) => new() { State = "SUCCESS", Result = data };

    /// <summary>
    /// 返回错误响应
    /// </summary>
    /// <param name="message">错误描述</param>
    /// <returns>State = 错误描述的通用结果</returns>
    public static UEditorResult Error(string message) => new() { State = message };
}

/// <summary>
/// 带泛型 Result 字段的响应（便于某些 action 返回强类型数据）
/// </summary>
/// <typeparam name="T">业务数据类型</typeparam>
public class UEditorResult<T> : UEditorResult
{
    /// <summary>
    /// 业务数据；默认不序列化以避免破坏 UEditor 协议格式
    /// </summary>
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Result { get; set; }
}

/// <summary>
/// 上传响应（图片/涂鸦/视频/附件 通用）
/// </summary>
public class UEditorUploadResult
{
    /// <summary>
    /// 状态（SUCCESS 表示成功）
    /// </summary>
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";

    /// <summary>
    /// 文件访问 URL
    /// </summary>
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 文件标题（通常为生成后的文件名）
    /// </summary>
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 原始文件名
    /// </summary>
    [JsonPropertyName("original")] public string Original { get; set; } = string.Empty;

    /// <summary>
    /// 文件扩展名
    /// </summary>
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [JsonPropertyName("size")] public long Size { get; set; }
}

/// <summary>
/// 图片/文件 列表响应
/// </summary>
public class UEditorListResult
{
    /// <summary>
    /// 状态（SUCCESS 表示成功）
    /// </summary>
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";

    /// <summary>
    /// 文件条目数组
    /// </summary>
    [JsonPropertyName("list")] public List<UEditorListEntry> List { get; set; } = [];

    /// <summary>
    /// 当前分页起始索引
    /// </summary>
    [JsonPropertyName("start")] public int Start { get; set; }

    /// <summary>
    /// 总数目
    /// </summary>
    [JsonPropertyName("total")] public int Total { get; set; }
}

/// <summary>
/// 文件列表中的单条记录
/// </summary>
public class UEditorListEntry
{
    /// <summary>
    /// 文件 URL
    /// </summary>
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 文件最后修改时间的 Unix 时间戳（秒）
    /// </summary>
    [JsonPropertyName("mtime")] public long Mtime { get; set; }
}

/// <summary>
/// 图片抓取响应（批量抓取时返回）
/// </summary>
public class UEditorCatcherResult
{
    /// <summary>
    /// 状态（SUCCESS 表示成功）
    /// </summary>
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";

    /// <summary>
    /// 抓取结果列表（每个源 URL 对应一条）
    /// </summary>
    [JsonPropertyName("list")] public List<UEditorCatcherEntry> List { get; set; } = [];
}

/// <summary>
/// 单张远程图片抓取结果
/// </summary>
public class UEditorCatcherEntry
{
    /// <summary>
    /// 本地保存后的访问 URL
    /// </summary>
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;

    /// <summary>
    /// 原始远程 URL
    /// </summary>
    [JsonPropertyName("source")] public string Source { get; set; } = string.Empty;

    /// <summary>
    /// 状态（SUCCESS 或 错误描述）
    /// </summary>
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";
}
