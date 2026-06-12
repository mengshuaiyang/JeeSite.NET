namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 配置（等价传统 config.json）。
/// 为了避免额外依赖外部文件，直接以 C# 对象表达完整能力。
/// </summary>
public sealed class UEditorOptions
{
    /* ===================== 上传图像配置 ===================== */
    public string ImageActionName { get; set; } = "uploadimage";
    public string ImageFieldName { get; set; } = "upfile";
    public int ImageMaxSize { get; set; } = 2048000;
    public string[] ImageAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];
    public bool ImageCompressEnable { get; set; } = true;
    public int ImageCompressBorder { get; set; } = 1600;
    public int ImageInsertAlign { get; set; } = 0;
    public string ImagePathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";
    public string ImageUrlPrefix { get; set; } = "";

    /* ===================== 涂鸦上传配置 ===================== */
    public string ScrawlActionName { get; set; } = "uploadscrawl";
    public string ScrawlFieldName { get; set; } = "upfile";
    public string ScrawlPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";
    public int ScrawlMaxSize { get; set; } = 2048000;
    public string[] ScrawlAllowFiles { get; set; } = [".png"];
    public string ScrawlUrlPrefix { get; set; } = "";
    public int ScrawlInsertAlign { get; set; } = 0;

    /* ===================== 上传视频配置 ===================== */
    public string VideoActionName { get; set; } = "uploadvideo";
    public string VideoFieldName { get; set; } = "upfile";
    public string VideoPathFormat { get; set; } = "upload/video/{yyyy}{mm}{dd}/{time}{rand:6}";
    public int VideoMaxSize { get; set; } = 102400000;
    public string[] VideoAllowFiles { get; set; } = [".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid"];
    public string VideoUrlPrefix { get; set; } = "";

    /* ===================== 上传文件配置 ===================== */
    public string FileActionName { get; set; } = "uploadfile";
    public string FileFieldName { get; set; } = "upfile";
    public string FilePathFormat { get; set; } = "upload/file/{yyyy}{mm}{dd}/{time}{rand:6}";
    public int FileMaxSize { get; set; } = 51200000;
    public string[] FileAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid", ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md"];
    public string FileUrlPrefix { get; set; } = "";

    /* ===================== 抓取远程文件配置 ===================== */
    public string CatcherActionName { get; set; } = "catchimage";
    public string CatcherFieldName { get; set; } = "source";
    public string[] CatcherUrlPrefix { get; set; } = ["127.0.0.1", "localhost"];
    public string CatcherPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";
    public int CatcherMaxSize { get; set; } = 2048000;
    public string[] CatcherAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];
    public string CatcherLocalDomain { get; set; } = "";

    /* ===================== 列表图片配置 ===================== */
    public string ImageManagerActionName { get; set; } = "listimage";
    public string ImageManagerListPath { get; set; } = "upload/image/";
    public int ImageManagerListSize { get; set; } = 20;
    public string ImageManagerUrlPrefix { get; set; } = "";
    public string[] ImageManagerInsertAlign { get; set; } = ["none"];
    public string[] ImageManagerAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];

    /* ===================== 列出文件配置 ===================== */
    public string FileManagerActionName { get; set; } = "listfile";
    public string FileManagerListPath { get; set; } = "upload/file/";
    public int FileManagerListSize { get; set; } = 20;
    public string FileManagerUrlPrefix { get; set; } = "";
    public string[] FileManagerAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid", ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md"];
}
