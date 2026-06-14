using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 图片 EXIF/GPS 地理信息读取工具类（基于 SkiaSharp）
/// </summary>
public static class ImageGeoUtil
{
    /// <summary>
    /// 同步读取图片中的地理/拍摄元信息
    /// </summary>
    /// <param name="imagePath">图片文件绝对或相对路径</param>
    /// <returns>GeoInfo 对象；文件不存在或无有效元数据时返回 null</returns>
    public static GeoInfo? ReadGeoInfo(string imagePath)
    {
        if (!File.Exists(imagePath)) return null;

        using var stream = File.OpenRead(imagePath);
        using var codec = SKCodec.Create(stream);
        if (codec == null) return null;

        // SkiaSharp 未直接暴露完整 EXIF 字段读取，此处以占位为主；
        // 具体经纬度/海拔/拍摄时间需通过第三方 EXIF 库补全。
        var info = new GeoInfo();

        return info.HasData ? info : null;
    }

    /// <summary>
    /// 异步读取图片中的地理/拍摄元信息
    /// </summary>
    /// <param name="imagePath">图片文件绝对或相对路径</param>
    /// <returns>GeoInfo 对象；文件不存在或无有效元数据时返回 null</returns>
    public static async Task<GeoInfo?> ReadGeoInfoAsync(string imagePath)
    {
        if (!File.Exists(imagePath)) return null;

        using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        using var codec = SKCodec.Create(stream);
        if (codec == null) return null;

        var info = new GeoInfo();
        return info.HasData ? info : null;
    }

    /// <summary>
    /// 获取图片地理位置与拍摄信息的摘要字符串
    /// </summary>
    /// <param name="imagePath">图片文件路径</param>
    /// <returns>人类可读的摘要字符串；无信息时返回 null</returns>
    public static string? GetGeoLocationSummary(string imagePath)
    {
        var geo = ReadGeoInfo(imagePath);
        return geo?.ToString();
    }
}

/// <summary>
/// 图片地理与拍摄元信息容器
/// </summary>
public class GeoInfo
{
    /// <summary>
    /// 经度（-180 ~ 180）
    /// </summary>
    public double? Longitude { get; set; }

    /// <summary>
    /// 纬度（-90 ~ 90）
    /// </summary>
    public double? Latitude { get; set; }

    /// <summary>
    /// 海拔（米）
    /// </summary>
    public double? Altitude { get; set; }

    /// <summary>
    /// 经度参考方向（"E" 东经 / "W" 西经）
    /// </summary>
    public string? LongitudeRef { get; set; }

    /// <summary>
    /// 纬度参考方向（"N" 北纬 / "S" 南纬）
    /// </summary>
    public string? LatitudeRef { get; set; }

    /// <summary>
    /// 原始拍摄时间（EXIF DateTimeOriginal）
    /// </summary>
    public DateTime? DateTimeOriginal { get; set; }

    /// <summary>
    /// 设备制造商
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// 设备型号
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// 图像方向（EXIF Orientation 1-8）
    /// </summary>
    public ushort? Orientation { get; set; }

    /// <summary>
    /// 当前对象是否包含任一有效元数据字段
    /// </summary>
    public bool HasData => Longitude.HasValue || Latitude.HasValue || Altitude.HasValue
        || DateTimeOriginal.HasValue || Make != null || Model != null;

    /// <summary>
    /// 格式化为人类可读的摘要字符串（如："纬度: 39.904200N | 经度: 116.407400E | 拍摄时间: 2025-01-01 12:00:00 | 设备: Apple iPhone 15"）
    /// </summary>
    /// <returns>拼接后的摘要字符串</returns>
    public override string ToString()
    {
        var parts = new List<string>();
        if (Latitude.HasValue) parts.Add($"纬度: {Latitude.Value:F6}{(LatitudeRef ?? "N")}");
        if (Longitude.HasValue) parts.Add($"经度: {Longitude.Value:F6}{(LongitudeRef ?? "E")}");
        if (Altitude.HasValue) parts.Add($"海拔: {Altitude.Value:F1}m");
        if (DateTimeOriginal.HasValue) parts.Add($"拍摄时间: {DateTimeOriginal.Value:yyyy-MM-dd HH:mm:ss}");
        if (Make != null) parts.Add($"设备: {Make} {Model}");
        return string.Join(" | ", parts);
    }
}
