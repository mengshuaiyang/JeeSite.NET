using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 图片处理工具类（基于 SkiaSharp 跨平台实现：缩略图生成、缩放、旋转、压缩、信息读取）
/// </summary>
public static class ImageUtil
{
    /// <summary>
    /// 按最大宽高生成等比例缩略图；源图小于目标尺寸时原样返回
    /// </summary>
    /// <param name="imageBytes">源图片字节数据</param>
    /// <param name="maxWidth">最大宽度（像素）</param>
    /// <param name="maxHeight">最大高度（像素）</param>
    /// <returns>压缩后的 JPEG 字节数组；解析失败返回原数据</returns>
    public static byte[] GenerateThumbnail(byte[] imageBytes, int maxWidth, int maxHeight)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        // 计算缩放比例：取宽、高中较小的比例以保证不超过任一维度
        float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
        if (scale >= 1) return imageBytes;

        int newWidth = (int)(original.Width * scale);
        int newHeight = (int)(original.Height * scale);

        using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);
        return data.ToArray();
    }

    /// <summary>
    /// 将图片强制缩放到指定尺寸（可能改变原始比例）
    /// </summary>
    /// <param name="imageBytes">源图片字节数据</param>
    /// <param name="width">目标宽度</param>
    /// <param name="height">目标高度</param>
    /// <returns>JPEG 编码字节数组；解析失败返回原数据</returns>
    public static byte[] Resize(byte[] imageBytes, int width, int height)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        using var resized = original.Resize(new SKImageInfo(width, height), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 85);
        return data.ToArray();
    }

    /// <summary>
    /// 按指定角度旋转图片（画布尺寸随旋转调整，以中心为旋转锚点）
    /// </summary>
    /// <param name="imageBytes">源图片字节数据</param>
    /// <param name="degrees">旋转角度（度）</param>
    /// <returns>旋转后的 JPEG 字节数组；解析失败返回原数据</returns>
    public static byte[] Rotate(byte[] imageBytes, int degrees)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        var rotated = new SKBitmap(original.Height, original.Width);
        using var canvas = new SKCanvas(rotated);
        canvas.Translate(rotated.Width / 2f, rotated.Height / 2f);
        canvas.RotateDegrees(degrees);
        canvas.DrawBitmap(original, -original.Width / 2f, -original.Height / 2f);
        canvas.Flush();

        using var image = SKImage.FromBitmap(rotated);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 85);
        return data.ToArray();
    }

    /// <summary>
    /// 按给定质量值重新压缩图片为 JPEG（质量 1-100，数字越大画质越高、体积越大）
    /// </summary>
    /// <param name="imageBytes">源图片字节数据</param>
    /// <param name="quality">压缩质量（1-100）</param>
    /// <returns>重编码后的 JPEG 字节数组；解析失败返回原数据</returns>
    public static byte[] Compress(byte[] imageBytes, int quality)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        using var image = SKImage.FromBitmap(original);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, Math.Clamp(quality, 1, 100));
        return data.ToArray();
    }

    /// <summary>
    /// 读取图片基础信息（宽、高、格式），不执行完整解码
    /// </summary>
    /// <param name="imageBytes">图片字节数据</param>
    /// <returns>(宽度, 高度, 格式) 元组；解析失败返回 null</returns>
    public static (int Width, int Height, string Format)? GetImageInfo(byte[] imageBytes)
    {
        using var input = new MemoryStream(imageBytes);
        using var codec = SKCodec.Create(input);
        if (codec == null) return null;

        return (codec.Info.Width, codec.Info.Height, codec.EncodedFormat.ToString());
    }
}
