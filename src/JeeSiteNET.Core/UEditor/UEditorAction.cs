namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 协议 action 枚举。所有值与官方 ActionMap 一致。
/// </summary>
public enum UEditorAction
{
    Config = 0,
    UploadImage = 1,
    UploadScrawl = 2,
    UploadVideo = 3,
    UploadFile = 4,
    CatchImage = 5,
    ListImage = 6,
    ListFile = 7
}

/// <summary>
/// 将 action 名称（字符串或数字）映射到枚举
/// </summary>
public static class UEditorActionMap
{
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
