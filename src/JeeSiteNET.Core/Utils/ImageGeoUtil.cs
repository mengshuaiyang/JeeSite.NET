using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

public static class ImageGeoUtil
{
    public static GeoInfo? ReadGeoInfo(string imagePath)
    {
        if (!File.Exists(imagePath)) return null;

        using var stream = File.OpenRead(imagePath);
        using var codec = SKCodec.Create(stream);
        if (codec == null) return null;

        var origin = codec.EncodedOrigin;
        var info = new GeoInfo();

        return info.HasData ? info : null;
    }

    public static async Task<GeoInfo?> ReadGeoInfoAsync(string imagePath)
    {
        if (!File.Exists(imagePath)) return null;

        using var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        using var codec = SKCodec.Create(stream);
        if (codec == null) return null;

        var info = new GeoInfo();
        return info.HasData ? info : null;
    }

    public static string? GetGeoLocationSummary(string imagePath)
    {
        var geo = ReadGeoInfo(imagePath);
        return geo?.ToString();
    }
}

public class GeoInfo
{
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    public double? Altitude { get; set; }
    public string? LongitudeRef { get; set; }
    public string? LatitudeRef { get; set; }
    public DateTime? DateTimeOriginal { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public ushort? Orientation { get; set; }

    public bool HasData => Longitude.HasValue || Latitude.HasValue || Altitude.HasValue
        || DateTimeOriginal.HasValue || Make != null || Model != null;

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
