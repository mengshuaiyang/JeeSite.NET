using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 图形验证码工具：使用 SkiaSharp 生成随机字符集的 PNG 图片，
/// 包含干扰弧线与字符旋转，增加 OCR 识别难度。适用于登录/注册/找回密码等场景。
/// </summary>
public static class CaptchaUtil
{
    /// <summary>
    /// 验证码字符集：排除易混淆字符（0/O/1/I/L 等），提高用户识别正确率。
    /// </summary>
    private static readonly char[] Characters = "ABDEFGHKMNRSWX2345689".ToCharArray();

    /// <summary>
    /// 生成指定长度的验证码字符串（字符均取自安全字符集）。
    /// </summary>
    /// <param name="length">验证码长度，默认 4。</param>
    /// <returns>随机字符组成的验证码。</returns>
    public static string GenerateCode(int length = 4)
    {
        // 使用加密安全的随机数生成器，避免可预测
        var bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(length);
        var chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = Characters[bytes[i] % Characters.Length];
        return new string(chars);
    }

    /// <summary>
    /// 根据给定验证码字符串绘制一张 PNG 图片，并返回其字节数组。
    /// </summary>
    /// <param name="code">验证码文字。</param>
    /// <param name="width">图片宽度（像素）。默认 100。</param>
    /// <param name="height">图片高度（像素）。默认 36。</param>
    /// <returns>PNG 格式的图片字节数组。</returns>
    public static byte[] GenerateImage(string code, int width = 100, int height = 36)
    {
        // 创建 Skia 画布：SkiaSharp 支持跨平台绘制，无需 System.Drawing.Common
        using var surface = SKSurface.Create(new SKImageInfo(width, height));
        var canvas = surface.Canvas;

        // 使用 code 的哈希作为随机种子，保证可重现（便于测试）
        var random = new Random(code.GetHashCode());

        // 白色背景
        canvas.Clear(SKColors.White);

        // 绘制 50 个半透明的彩色椭圆弧，作为背景干扰
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
            canvas.DrawArc(
                new SKRect(
                    random.Next(width - 3), random.Next(height - 2),
                    random.Next(width - 3) + random.Next(6),
                    random.Next(height - 2) + random.Next(6)),
                random.Next(360), random.Next(360), false, paint);
        }

        // 绘制 5 条随机细直线，提高识别难度
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

        // 字体与字形（Arial Bold 作为默认字体，跨平台通常可用）
        using var font = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        using var skFont = new SKFont(font, 26);

        // 逐字符绘制，并在绘制前随机旋转字符
        for (int i = 0; i < code.Length; i++)
        {
            // 字符横向分布：首尾留 8px 边距
            float x = 8 + i * (width - 16) / code.Length;
            float y = height - 8;

            // 保存当前画布状态，再按角度旋转，旋转后恢复
            canvas.Save();
            float angle = (float)(random.NextDouble() * 40 - 20); // -20 ~ +20 度
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

        // 将画布内容编码为 PNG，返回字节数组
        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 80);
        return data.ToArray();
    }
}
