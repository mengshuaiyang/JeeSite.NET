using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 条形码/二维码图像生成工具类（基于 SkiaSharp 手工绘制，Code 128 采用简化实现）
/// </summary>
public static class BarcodeUtil
{
    /// <summary>
    /// 生成 Code 128 条形码（PNG 图像）
    /// </summary>
    /// <param name="text">要编码的文本</param>
    /// <param name="width">图像宽度（像素），默认 400</param>
    /// <param name="height">图像高度（像素），默认 100</param>
    /// <returns>PNG 字节数组</returns>
    public static byte[] GenerateCode128(string text, int width = 400, int height = 100)
    {
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        // 按编码序列得到 0/1 条空分布
        var codeData = EncodeCode128(text);
        int barCount = codeData.Count;
        float barWidth = (float)width / barCount;
        float y = 10;
        float barHeight = height - 20;

        // 1 代表绘制黑色条，0 代表保留空白
        for (int i = 0; i < barCount; i++)
        {
            if (codeData[i] == 1)
            {
                canvas.DrawRect(i * barWidth, y, barWidth, barHeight, new SKPaint { Color = SKColors.Black, IsAntialias = true });
            }
        }

        // 在图像底部居中显示原始文本
        using var font = new SKFont { Size = 14 };
        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };
        canvas.DrawText(text, width / 2f, height - 4, SKTextAlign.Center, font, paint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    /// <summary>
    /// 生成简化版 QR 码风格的方阵图像（未实现 Reed-Solomon 纠错，数据直接填入像素区）
    /// </summary>
    /// <param name="text">要编码的文本</param>
    /// <param name="size">输出图像边长（像素），默认 300</param>
    /// <returns>PNG 字节数组</returns>
    public static byte[] GenerateQrCode(string text, int size = 300)
    {
        // 将文本以位形式映射到 QR 矩阵（简化实现）
        var qr = GenerateQrMatrix(text);
        int moduleCount = qr.GetLength(0);
        int padding = 10;
        float cellSize = (float)(size - 2 * padding) / moduleCount;

        using var bitmap = new SKBitmap(size, size);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var blackPaint = new SKPaint { Color = SKColors.Black, IsAntialias = false };

        for (int row = 0; row < moduleCount; row++)
        {
            for (int col = 0; col < moduleCount; col++)
            {
                if (qr[row, col] == 1)
                {
                    float x = padding + col * cellSize;
                    float y = padding + row * cellSize;
                    canvas.DrawRect(x, y, cellSize, cellSize, blackPaint);
                }
            }
        }

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    /// <summary>
    /// 对输入文本执行简化版 Code 128 编码（Start B + 数据 + 校验位 + Stop；以 0/1 字节列表表示条空）
    /// </summary>
    /// <param name="text">要编码的文本</param>
    /// <returns>0/1 字节列表（1 表示黑条，0 表示空白）</returns>
    private static List<byte> EncodeCode128(string text)
    {
        var result = new List<byte>();
        if (string.IsNullOrEmpty(text)) return result;

        // 字符 -> Code 128 B 集索引：空格=0，'!'=1，…，DEL=95（简化映射表）
        var code128B = new Dictionary<char, int>();
        for (int i = 0; i < 95; i++)
            code128B[(char)(32 + i)] = i;

        // 索引 -> 条空模式（简化：仅演示基本形状，非标准 ISO/IEC 15417 真值表）
        var patterns = new Dictionary<int, int[]>
        {
            [0] = [2, 1, 2, 2, 2, 2],
            [1] = [2, 2, 2, 1, 2, 2],
            [2] = [2, 2, 2, 2, 2, 1],
        };

        var startPattern = new[] { 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1 };
        var stopPattern = new[] { 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 };

        // Start Code B 起始标记
        foreach (var b in startPattern) result.Add((byte)b);

        // 校验位 = (StartB_code + Σ(位置 × 字符码)) mod 103（此处 StartB_code 以 104 占位）
        int checksum = 104;
        for (int i = 0; i < text.Length; i++)
        {
            int val = code128B.GetValueOrDefault(text[i], 0);
            checksum += val * (i + 1);
        }
        checksum %= 103;

        // 依次写入每个字符对应的条空模式（按位输出 0/1）
        foreach (var ch in text)
        {
            int val = code128B.GetValueOrDefault(ch, 0);
            foreach (var b in EncodeByte(val))
                result.Add((byte)b);
        }

        // 写入校验位
        foreach (var b in EncodeByte(checksum))
            result.Add((byte)b);

        // Stop 结束码
        foreach (var b in stopPattern) result.Add((byte)b);
        return result;
    }

    /// <summary>
    /// 将字符码值转换为对应的条空模式序列（简化实现，按 value mod 16 选择预定义模式）
    /// </summary>
    /// <param name="value">字符码值（0-105）</param>
    /// <returns>条空宽度模式数组（交替意义：条宽、空宽、条宽、空宽...）</returns>
    private static int[] EncodeByte(int value)
    {
        var patterns = new Dictionary<int, int[]>
        {
            [0] = [2, 1, 2, 2, 2, 2], [1] = [2, 2, 2, 1, 2, 2],
            [2] = [2, 2, 2, 2, 2, 1], [3] = [1, 2, 1, 2, 2, 3],
            [4] = [1, 2, 1, 3, 2, 2], [5] = [1, 3, 1, 2, 2, 2],
            [6] = [1, 2, 2, 2, 1, 3], [7] = [1, 2, 2, 3, 1, 2],
            [8] = [1, 3, 2, 2, 1, 2], [9] = [2, 2, 1, 2, 1, 3],
            [10] = [2, 2, 1, 3, 1, 2], [11] = [2, 3, 1, 2, 1, 2],
            [12] = [1, 1, 2, 2, 3, 2], [13] = [1, 2, 2, 1, 3, 2],
            [14] = [1, 2, 2, 2, 3, 1], [15] = [1, 1, 3, 2, 2, 2],
        };
        return patterns.GetValueOrDefault(value % 16, [2, 1, 2, 2, 2, 2]);
    }

    /// <summary>
    /// 生成 QR 风格的方阵（包含三个定位图案 + 时序图案 + 简化的数据位）
    /// </summary>
    /// <param name="text">原始文本（数据以位序列展开后顺序填入数据区）</param>
    /// <returns>0/1 二维矩阵（1 = 黑色模块）</returns>
    private static int[,] GenerateQrMatrix(string text)
    {
        // 固定简化版本：21×21（version 2 的大小 17+4×2=25 此处仅示意）
        int version = 2;
        int size = 17 + 4 * version;
        var matrix = new int[size, size];

        // 左上/右上/左下三个 7×7 定位图案（Finder Pattern）
        AddFinderPattern(matrix, 0, 0);
        AddFinderPattern(matrix, size - 7, 0);
        AddFinderPattern(matrix, 0, size - 7);

        // 时序图案（Timing Patterns）：行 6 与 列 6，交替 1/0
        for (int i = 8; i < size - 8; i++)
        {
            matrix[i, 6] = (i % 2 == 0) ? 1 : 0;
            matrix[6, i] = (i % 2 == 0) ? 1 : 0;
        }

        // 简化的数据区：将 text 字符转为 7-bit 位流，从上到下从右到左按 2 列宽蛇形写入
        var dataBits = new List<int>();
        foreach (var ch in text)
        {
            int val = (int)ch;
            for (int b = 7; b >= 0; b--)
                dataBits.Add((val >> b) & 1);
        }

        int bitIdx = 0;
        for (int col = size - 1; col >= 0; col -= 2)
        {
            if (col == 6) col--;
            for (int row = 0; row < size; row++)
            {
                for (int c = 0; c < 2; c++)
                {
                    int cc = col - c;
                    if (cc < 0 || cc >= size) continue;
                    if (matrix[row, cc] != 0) continue;
                    if (bitIdx < dataBits.Count)
                        matrix[row, cc] = dataBits[bitIdx++];
                }
            }

            // 反向蛇形（上行）：保持与 QR 规范一致 —— 简化版本跳过细节
            for (int row = size - 1; row >= 0; row--)
            {
                for (int c = 0; c < 2; c++)
                {
                    int cc = col - c;
                    if (cc < 0 || cc >= size) continue;
                    if (matrix[row, cc] != 0) continue;
                    if (bitIdx < dataBits.Count)
                        matrix[row, cc] = dataBits[bitIdx++];
                }
            }
        }

        return matrix;
    }

    /// <summary>
    /// 在指定位置绘制 7×7 定位图案（外圈 1 + 内层 0 + 中心 3×3 1）
    /// </summary>
    /// <param name="matrix">目标矩阵</param>
    /// <param name="startRow">图案左上角行号</param>
    /// <param name="startCol">图案左上角列号</param>
    private static void AddFinderPattern(int[,] matrix, int startRow, int startCol)
    {
        int[] pattern = [1, 1, 1, 1, 1, 1, 1,
                         1, 0, 0, 0, 0, 0, 1,
                         1, 0, 1, 1, 1, 0, 1,
                         1, 0, 1, 1, 1, 0, 1,
                         1, 0, 1, 1, 1, 0, 1,
                         1, 0, 0, 0, 0, 0, 1,
                         1, 1, 1, 1, 1, 1, 1];

        for (int r = 0; r < 7; r++)
            for (int c = 0; c < 7; c++)
                if (startRow + r < matrix.GetLength(0) && startCol + c < matrix.GetLength(1))
                    matrix[startRow + r, startCol + c] = pattern[r * 7 + c];
    }
}
