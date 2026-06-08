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
}
