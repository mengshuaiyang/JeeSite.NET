using JeeSiteNET.Core.Utils;
using FluentAssertions;
using SkiaSharp;

namespace JeeSiteNET.Core.Tests;

public class ImageUtilTests
{
    private static byte[] CreateTestImage(int width = 10, int height = 10)
    {
        var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.Red);
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    [Fact]
    public void Resize_ShouldResizeCorrectly()
    {
        var testImage = CreateTestImage(20, 20);
        var result = ImageUtil.Resize(testImage, 40, 40);
        result.Should().NotBeNullOrEmpty();

        var info = ImageUtil.GetImageInfo(result);
        info.Should().NotBeNull();
        info.Value.Width.Should().Be(40);
        info.Value.Height.Should().Be(40);
    }

    [Fact]
    public void Rotate_ShouldReturnNonEmpty()
    {
        var testImage = CreateTestImage();
        var result = ImageUtil.Rotate(testImage, 90);
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void Compress_ShouldReturnNonEmpty()
    {
        var testImage = CreateTestImage();
        var result = ImageUtil.Compress(testImage, 50);
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GetImageInfo_ReturnCorrectInfo()
    {
        var testImage = CreateTestImage(15, 25);
        var info = ImageUtil.GetImageInfo(testImage);
        info.Should().NotBeNull();
        info.Value.Width.Should().Be(15);
        info.Value.Height.Should().Be(25);
        info.Value.Format.Should().Be("Png");
    }
}
