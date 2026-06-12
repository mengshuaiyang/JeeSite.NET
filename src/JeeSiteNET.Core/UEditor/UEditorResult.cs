using System.Text.Json;
using System.Text.Json.Serialization;

namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 标准响应基类。所有 action 返回都遵循一个约定格式。
/// imageUrl, state, list 等字段根据 action 不同填充不同字段。
/// </summary>
public class UEditorResult
{
    [JsonPropertyName("state")]
    public string State { get; set; } = "SUCCESS";

    public static UEditorResult<T> Success<T>(T data) => new() { State = "SUCCESS", Result = data };
    public static UEditorResult Error(string message) => new() { State = message };
}

public class UEditorResult<T> : UEditorResult
{
    [JsonPropertyName("data")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public T? Result { get; set; }
}

/// <summary>
/// 上传响应（image/scrawl/video/file 共用）
/// </summary>
public class UEditorUploadResult
{
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
    [JsonPropertyName("title")] public string Title { get; set; } = string.Empty;
    [JsonPropertyName("original")] public string Original { get; set; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; set; } = string.Empty;
    [JsonPropertyName("size")] public long Size { get; set; }
}

/// <summary>
/// 图片列表响应（listimage/listfile）
/// </summary>
public class UEditorListResult
{
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";
    [JsonPropertyName("list")] public List<UEditorListEntry> List { get; set; } = [];
    [JsonPropertyName("start")] public int Start { get; set; }
    [JsonPropertyName("total")] public int Total { get; set; }
}

public class UEditorListEntry
{
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
    [JsonPropertyName("mtime")] public long Mtime { get; set; }
}

/// <summary>
/// 图片抓取响应（catchimage）
/// </summary>
public class UEditorCatcherResult
{
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";
    [JsonPropertyName("list")] public List<UEditorCatcherEntry> List { get; set; } = [];
}

public class UEditorCatcherEntry
{
    [JsonPropertyName("url")] public string Url { get; set; } = string.Empty;
    [JsonPropertyName("source")] public string Source { get; set; } = string.Empty;
    [JsonPropertyName("state")] public string State { get; set; } = "SUCCESS";
}
