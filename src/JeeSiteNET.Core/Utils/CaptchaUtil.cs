using SkiaSharp;
using System.Security.Cryptography;

namespace JeeSiteNET.Core.Utils;

public static class CaptchaUtil
{
    private static readonly char[] Characters = "ABDEFGHKMNRSWX2345689".ToCharArray();

    public static string GenerateCode(int length = 4)
    {
        var chars = new char[length];
        var bytes = RandomNumberGenerator.GetBytes(length);
        for (int i = 0; i < length; i++)
            chars[i] = Characters[bytes[i] % Characters.Length];
        return new string(chars);
    }

    public static byte[] GenerateImage(string code, int width = 100, int height = 36)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;
        var random = new Random(code.GetHashCode());

        canvas.Clear(SKColors.White);

        for (int i = 0; i < 50; i++)
        {
            using var paint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = new SKColor(
                    (byte)random.Next(100, 200),
                    (byte)random.Next(100, 200),
                    (byte)random.Next(100, 200))
            };
            canvas.DrawArc(new SKRect(
                random.Next(width - 3), random.Next(height - 2),
                random.Next(width - 3) + random.Next(6),
                random.Next(height - 2) + random.Next(6)),
                random.Next(360), random.Next(360), false, paint);
        }

        using var linePaint = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            Color = new SKColor(180, 180, 180)
        };
        for (int i = 0; i < 5; i++)
            canvas.DrawLine(random.Next(width), random.Next(height),
                random.Next(width), random.Next(height), linePaint);

        using var font = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        using var skFont = new SKFont(font, 26);
        for (int i = 0; i < code.Length; i++)
        {
            float x = 8 + i * (width - 16) / code.Length;
            float y = height - 8;
            canvas.Save();
            float angle = (float)(random.NextDouble() * 40 - 20);
            canvas.RotateDegrees(angle, x + 8, y - 4);
            using var glyphPaint = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = new SKColor(
                    (byte)random.Next(30, 120),
                    (byte)random.Next(30, 120),
                    (byte)random.Next(30, 120))
            };
            canvas.DrawText(code[i].ToString(), x, y, SKTextAlign.Left, skFont, glyphPaint);
            canvas.Restore();
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 80);
        return data.ToArray();
    }
}
