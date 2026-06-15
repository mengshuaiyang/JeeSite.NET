using JeeSiteNET.Core.Utils;
using FluentAssertions;

namespace JeeSiteNET.Core.Tests;

public class BarcodeUtilTests
{
    [Fact]
    public void GenerateCode128_ShouldReturnNonEmptyBytes()
    {
        var result = BarcodeUtil.GenerateCode128("ABC123");
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateQrCode_ShouldReturnNonEmptyBytes()
    {
        var result = BarcodeUtil.GenerateQrCode("https://jeesite.net");
        result.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateCode128_OutputStartsWithPngHeader()
    {
        var result = BarcodeUtil.GenerateCode128("Test");
        result.Take(4).Should().Equal(0x89, 0x50, 0x4E, 0x47);
    }

    [Fact]
    public void GenerateQrCode_OutputStartsWithPngHeader()
    {
        var result = BarcodeUtil.GenerateQrCode("Test QR");
        result.Take(4).Should().Equal(0x89, 0x50, 0x4E, 0x47);
    }
}
