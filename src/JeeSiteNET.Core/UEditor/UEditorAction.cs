namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 协议支持的 action 枚举（与官方配置字符串保持一致）
/// </summary>
public enum UEditorAction
{
    /// <summary>
    /// 拉取配置（返回 config.json）
    /// </summary>
    Config = 0,

    /// <summary>
    /// 上传图片（uploadimage）
    /// </summary>
    UploadImage = 1,

    /// <summary>
    /// 上传涂鸦（uploadscrawl）
    /// </summary>
    UploadScrawl = 2,

    /// <summary>
    /// 上传视频（uploadvideo）
    /// </summary>
    UploadVideo = 3,

    /// <summary>
    /// 上传附件（uploadfile）
    /// </summary>
    UploadFile = 4,

    /// <summary>
    /// 抓取远程图片（catchimage）
    /// </summary>
    CatchImage = 5,

    /// <summary>
    /// 列出图片（listimage）
    /// </summary>
    ListImage = 6,

    /// <summary>
    /// 列出文件（listfile）
    /// </summary>
    ListFile = 7
}

/// <summary>
/// action 解析器：将 HttpContext.Request.Query["action"] 的字符串（或数字）映射到 UEditorAction
/// </summary>
public static class UEditorActionMap
{
    /// <summary>
    /// 按字符串解析 action
    /// </summary>
    /// <param name="action">请求中的 action 参数（如 "uploadimage" / "0"）</param>
    /// <returns>解析后的枚举值；输入无效时返回 null</returns>
    public static UEditorAction? Parse(string? action)
    {
        if (string.IsNullOrWhiteSpace(action)) return null;
        return action.ToLowerInvariant() switch
        {
            "config" or "0" => UEditorAction.Config,
            "uploadimage" or "1" => UEditorAction.UploadImage,
            "uploadscrawl" or "2" => UEditorAction.UploadScrawl,
            "uploadvideo" or "3" => UEditorAction.UploadVideo,
            "uploadfile" or "4" => UEditorAction.UploadFile,
            "catchimage" or "5" => UEditorAction.CatchImage,
            "listimage" or "6" => UEditorAction.ListImage,
            "listfile" or "7" => UEditorAction.ListFile,
            _ => null
        };
    }
}
