using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

public static class ImageUtil
{
    public static byte[] GenerateThumbnail(byte[] imageBytes, int maxWidth, int maxHeight)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
        if (scale >= 1) return imageBytes;

        int newWidth = (int)(original.Width * scale);
        int newHeight = (int)(original.Height * scale);

        using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);
        return data.ToArray();
    }

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

    public static byte[] Compress(byte[] imageBytes, int quality)
    {
        using var input = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(input);
        if (original == null) return imageBytes;

        using var image = SKImage.FromBitmap(original);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, Math.Clamp(quality, 1, 100));
        return data.ToArray();
    }

    public static (int Width, int Height, string Format)? GetImageInfo(byte[] imageBytes)
    {
        using var input = new MemoryStream(imageBytes);
        using var codec = SKCodec.Create(input);
        if (codec == null) return null;

        return (codec.Info.Width, codec.Info.Height, codec.EncodedFormat.ToString());
    }
}
