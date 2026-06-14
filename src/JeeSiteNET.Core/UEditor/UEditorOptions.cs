namespace JeeSiteNET.Core.UEditor;

/// <summary>
/// UEditor 运行期配置（等价于官方 config.json）。
/// 所有字段命名与常见 UEditor 配置项保持一一对应，便于前端解析。
/// </summary>
public sealed class UEditorOptions
{
    /* ===================== 上传图片 ===================== */

    /// <summary>
    /// 图片上传 action 名称（uploadimage）
    /// </summary>
    public string ImageActionName { get; set; } = "uploadimage";

    /// <summary>
    /// 图片表单字段名（upfile）
    /// </summary>
    public string ImageFieldName { get; set; } = "upfile";

    /// <summary>
    /// 图片大小上限（字节）
    /// </summary>
    public int ImageMaxSize { get; set; } = 2048000;

    /// <summary>
    /// 图片允许的扩展名
    /// </summary>
    public string[] ImageAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];

    /// <summary>
    /// 是否开启图片压缩
    /// </summary>
    public bool ImageCompressEnable { get; set; } = true;

    /// <summary>
    /// 压缩后图片最大边长（像素）
    /// </summary>
    public int ImageCompressBorder { get; set; } = 1600;

    /// <summary>
    /// 图片插入对齐方式（0:无 / 1:左 / 2:居中 / 3:右）
    /// </summary>
    public int ImageInsertAlign { get; set; } = 0;

    /// <summary>
    /// 图片文件保存路径模板（含 {yyyy}{mm}{dd}{time}{rand:6} 占位符）
    /// </summary>
    public string ImagePathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";

    /// <summary>
    /// 图片 URL 前缀（CDN / 反向代理配置）
    /// </summary>
    public string ImageUrlPrefix { get; set; } = "";

    /* ===================== 涂鸦上传 ===================== */

    /// <summary>
    /// 涂鸦上传 action 名称
    /// </summary>
    public string ScrawlActionName { get; set; } = "uploadscrawl";

    /// <summary>
    /// 涂鸦表单字段名
    /// </summary>
    public string ScrawlFieldName { get; set; } = "upfile";

    /// <summary>
    /// 涂鸦保存路径模板
    /// </summary>
    public string ScrawlPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";

    /// <summary>
    /// 涂鸦大小上限（字节）
    /// </summary>
    public int ScrawlMaxSize { get; set; } = 2048000;

    /// <summary>
    /// 涂鸦允许的扩展名
    /// </summary>
    public string[] ScrawlAllowFiles { get; set; } = [".png"];

    /// <summary>
    /// 涂鸦 URL 前缀
    /// </summary>
    public string ScrawlUrlPrefix { get; set; } = "";

    /// <summary>
    /// 涂鸦插入对齐方式
    /// </summary>
    public int ScrawlInsertAlign { get; set; } = 0;

    /* ===================== 上传视频 ===================== */

    /// <summary>
    /// 视频上传 action 名称
    /// </summary>
    public string VideoActionName { get; set; } = "uploadvideo";

    /// <summary>
    /// 视频表单字段名
    /// </summary>
    public string VideoFieldName { get; set; } = "upfile";

    /// <summary>
    /// 视频保存路径模板
    /// </summary>
    public string VideoPathFormat { get; set; } = "upload/video/{yyyy}{mm}{dd}/{time}{rand:6}";

    /// <summary>
    /// 视频大小上限（字节）
    /// </summary>
    public int VideoMaxSize { get; set; } = 102400000;

    /// <summary>
    /// 视频允许的扩展名
    /// </summary>
    public string[] VideoAllowFiles { get; set; } = [".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid"];

    /// <summary>
    /// 视频 URL 前缀
    /// </summary>
    public string VideoUrlPrefix { get; set; } = "";

    /* ===================== 上传附件 ===================== */

    /// <summary>
    /// 附件上传 action 名称
    /// </summary>
    public string FileActionName { get; set; } = "uploadfile";

    /// <summary>
    /// 附件表单字段名
    /// </summary>
    public string FileFieldName { get; set; } = "upfile";

    /// <summary>
    /// 附件保存路径模板
    /// </summary>
    public string FilePathFormat { get; set; } = "upload/file/{yyyy}{mm}{dd}/{time}{rand:6}";

    /// <summary>
    /// 附件大小上限（字节）
    /// </summary>
    public int FileMaxSize { get; set; } = 51200000;

    /// <summary>
    /// 附件允许的扩展名
    /// </summary>
    public string[] FileAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid", ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md"];

    /// <summary>
    /// 附件 URL 前缀
    /// </summary>
    public string FileUrlPrefix { get; set; } = "";

    /* ===================== 抓取远程图片 ===================== */

    /// <summary>
    /// 抓图 action 名称
    /// </summary>
    public string CatcherActionName { get; set; } = "catchimage";

    /// <summary>
    /// 抓图表单字段名
    /// </summary>
    public string CatcherFieldName { get; set; } = "source";

    /// <summary>
    /// 抓图允许的域名白名单（数组）
    /// </summary>
    public string[] CatcherUrlPrefix { get; set; } = ["127.0.0.1", "localhost"];

    /// <summary>
    /// 抓图保存路径模板
    /// </summary>
    public string CatcherPathFormat { get; set; } = "upload/image/{yyyy}{mm}{dd}/{time}{rand:6}";

    /// <summary>
    /// 抓图大小上限（字节）
    /// </summary>
    public int CatcherMaxSize { get; set; } = 2048000;

    /// <summary>
    /// 抓图允许的扩展名
    /// </summary>
    public string[] CatcherAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];

    /// <summary>
    /// 抓图本地域名（供替换远程资源链接使用）
    /// </summary>
    public string CatcherLocalDomain { get; set; } = "";

    /* ===================== 列出图片 ===================== */

    /// <summary>
    /// 图片列表 action 名称
    /// </summary>
    public string ImageManagerActionName { get; set; } = "listimage";

    /// <summary>
    /// 图片列表起始路径
    /// </summary>
    public string ImageManagerListPath { get; set; } = "upload/image/";

    /// <summary>
    /// 图片列表单次返回数量
    /// </summary>
    public int ImageManagerListSize { get; set; } = 20;

    /// <summary>
    /// 图片列表 URL 前缀
    /// </summary>
    public string ImageManagerUrlPrefix { get; set; } = "";

    /// <summary>
    /// 图片插入对齐字符串数组（none）
    /// </summary>
    public string[] ImageManagerInsertAlign { get; set; } = ["none"];

    /// <summary>
    /// 图片列表允许的扩展名
    /// </summary>
    public string[] ImageManagerAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp"];

    /* ===================== 列出文件 ===================== */

    /// <summary>
    /// 文件列表 action 名称
    /// </summary>
    public string FileManagerActionName { get; set; } = "listfile";

    /// <summary>
    /// 文件列表起始路径
    /// </summary>
    public string FileManagerListPath { get; set; } = "upload/file/";

    /// <summary>
    /// 文件列表单次返回数量
    /// </summary>
    public int FileManagerListSize { get; set; } = 20;

    /// <summary>
    /// 文件列表 URL 前缀
    /// </summary>
    public string FileManagerUrlPrefix { get; set; } = "";

    /// <summary>
    /// 文件列表允许的扩展名
    /// </summary>
    public string[] FileManagerAllowFiles { get; set; } = [".png", ".jpg", ".jpeg", ".gif", ".bmp", ".flv", ".mkv", ".avi", ".rm", ".rmvb", ".mpeg", ".mpg", ".ogg", ".ogv", ".mov", ".wmv", ".mp4", ".webm", ".mp3", ".wav", ".mid", ".rar", ".zip", ".tar", ".gz", ".7z", ".bz2", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".txt", ".md"];
}
