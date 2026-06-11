using SkiaSharp;

namespace JeeSiteNET.Core.Utils;

public static class BarcodeUtil
{
    public static byte[] GenerateCode128(string text, int width = 400, int height = 100)
    {
        using var bitmap = new SKBitmap(width, height);
        using var canvas = new SKCanvas(bitmap);
        canvas.Clear(SKColors.White);

        var codeData = EncodeCode128(text);
        int barCount = codeData.Count;
        float barWidth = (float)width / barCount;
        float y = 10;
        float barHeight = height - 20;

        for (int i = 0; i < barCount; i++)
        {
            if (codeData[i] == 1)
            {
                canvas.DrawRect(i * barWidth, y, barWidth, barHeight, new SKPaint { Color = SKColors.Black, IsAntialias = true });
            }
        }

        using var paint = new SKPaint
        {
            Color = SKColors.Black,
            TextSize = 14,
            IsAntialias = true,
            TextAlign = SKTextAlign.Center
        };
        canvas.DrawText(text, width / 2f, height - 4, paint);

        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    public static byte[] GenerateQrCode(string text, int size = 300)
    {
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

    private static List<byte> EncodeCode128(string text)
    {
        var result = new List<byte>();
        if (string.IsNullOrEmpty(text)) return result;

        var code128B = new Dictionary<char, int>();
        for (int i = 0; i < 95; i++)
            code128B[(char)(32 + i)] = i;

        var patterns = new Dictionary<int, int[]>
        {
            [0] = [2, 1, 2, 2, 2, 2],
            [1] = [2, 2, 2, 1, 2, 2],
            [2] = [2, 2, 2, 2, 2, 1],
        };

        var startPattern = new[] { 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1 };
        var stopPattern = new[] { 1, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1 };

        // Start Code B
        foreach (var b in startPattern) result.Add((byte)b);

        int checksum = 104;
        for (int i = 0; i < text.Length; i++)
        {
            int val = code128B.GetValueOrDefault(text[i], 0);
            checksum += val * (i + 1);
        }
        checksum %= 103;

        foreach (var ch in text)
        {
            int val = code128B.GetValueOrDefault(ch, 0);
            foreach (var b in EncodeByte(val))
                result.Add((byte)b);
        }

        foreach (var b in EncodeByte(checksum))
            result.Add((byte)b);

        foreach (var b in stopPattern) result.Add((byte)b);
        return result;
    }

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

    private static int[,] GenerateQrMatrix(string text)
    {
        int version = 2;
        int size = 17 + 4 * version;
        var matrix = new int[size, size];

        // Finder patterns (top-left, top-right, bottom-left)
        AddFinderPattern(matrix, 0, 0);
        AddFinderPattern(matrix, size - 7, 0);
        AddFinderPattern(matrix, 0, size - 7);

        // Timing patterns
        for (int i = 8; i < size - 8; i++)
        {
            matrix[i, 6] = (i % 2 == 0) ? 1 : 0;
            matrix[6, i] = (i % 2 == 0) ? 1 : 0;
        }

        // Data bits (simplified - encode text as raw bits in data area)
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
