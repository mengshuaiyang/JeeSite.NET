namespace JeeSiteNET.Core.AiTools;

public class AiToolResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? Data { get; set; }
    public string? ContentType { get; set; }

    public static AiToolResult Ok(string? data = null, string? contentType = "text")
        => new() { Success = true, Data = data, ContentType = contentType };

    public static AiToolResult Fail(string error)
        => new() { Success = false, Error = error };
}
