using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// 通用哈希与加密工具：提供 MD5、SHA-1、SHA-256 与国产 SM3 的字符串摘要方法，
/// 全部返回小写十六进制字符串。注意：MD5/SHA-1 仅用于与旧系统兼容，不可作为密码存储方案。
/// </summary>
public static class EncryptUtil
{
    /// <summary>
    /// 计算输入字符串的 MD5 哈希值（128 位），返回 32 位小写十六进制字符串。
    /// </summary>
    /// <param name="input">待计算的字符串（UTF-8 编码）。</param>
    /// <returns>32 位小写十六进制字符串。</returns>
    public static string Md5(string input)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    /// <summary>
    /// 计算输入字符串的 SHA-1 哈希值（160 位），返回 40 位小写十六进制字符串。
    /// </summary>
    /// <param name="input">待计算的字符串（UTF-8 编码）。</param>
    /// <returns>40 位小写十六进制字符串。</returns>
    public static string Sha1(string input)
    {
        var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    /// <summary>
    /// 计算输入字符串的 SHA-256 哈希值（256 位），返回 64 位小写十六进制字符串。
    /// </summary>
    /// <param name="input">待计算的字符串（UTF-8 编码）。</param>
    /// <returns>64 位小写十六进制字符串。</returns>
    public static string Sha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    /// <summary>
    /// 计算 SM3 国产密码杂凑算法（256 位），返回 64 位小写十六进制字符串。
    /// 遵循 GM/T 0004-2012《SM3 密码杂凑算法》规范，适用于国密合规场景。
    /// </summary>
    /// <param name="input">待计算的字符串（按 UTF-8 编码转换为字节）。</param>
    /// <returns>64 位小写十六进制字符串。</returns>
    public static string Sm3(string input)
    {
        // 将字符串转换为 UTF-8 字节序列
        var data = Encoding.UTF8.GetBytes(input);

        // 按 SM3 规范填充消息，使其长度满足 (len + padding) mod 512 == 448，最后 64 位为原始长度
        var padded = PadSm3(data);

        // 8 个 32 位初始寄存器值（SM3 定义的固定 IV）
        var v = new uint[] { 0x7380166F, 0x4914B2B9, 0x172442D7, 0xDA8A0600, 0xA96F30BC, 0xE1638AA5, 0xE38DEE4D, 0xB0FB0E4E };
        var w = new uint[68];   // 消息扩展字 w[0..67]
        var w2 = new uint[64];  // 辅助扩展字 w'[0..63]

        // 以 512 位（64 字节）为一块循环迭代
        for (int i = 0; i < padded.Length; i += 64)
        {
            // 步骤 1：将 16 个 32 位字填入 w[0..15]（大端）
            for (int t = 0; t < 16; t++)
                w[t] = BytesToUint(padded, i + t * 4);

            // 步骤 2：递推 w[16..67]
            for (int t = 16; t < 68; t++)
                w[t] = P1(w[t - 16] ^ w[t - 9] ^ Rotl(w[t - 3], 15)) ^ Rotl(w[t - 13], 7) ^ w[t - 6];

            // 步骤 3：w'[t] = w[t] ^ w[t+4]
            for (int t = 0; t < 64; t++)
                w2[t] = w[t] ^ w[t + 4];

            // 初始化 8 个状态字（A..H）
            var a = v[0]; var b = v[1]; var c = v[2]; var d = v[3];
            var e = v[4]; var f = v[5]; var g = v[6]; var h = v[7];

            // 步骤 4：前 16 轮，使用 FF0/GG0 + 常量 0x79CC4519
            for (int t = 0; t < 16; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x79CC4519, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF0(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG0(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

            // 步骤 5：后 48 轮，使用 FF1/GG1 + 常量 0x7A879D8A
            for (int t = 16; t < 64; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x7A879D8A, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF1(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG1(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

            // 步骤 6：Davies-Meyer 反馈，将本轮结果与旧状态异或
            v[0] ^= a; v[1] ^= b; v[2] ^= c; v[3] ^= d;
            v[4] ^= e; v[5] ^= f; v[6] ^= g; v[7] ^= h;
        }

        // 将 8 个 32 位状态字按大端序写回 32 字节结果数组
        var result = new byte[32];
        for (int i = 0; i < 8; i++)
        {
            result[i * 4] = (byte)(v[i] >> 24);
            result[i * 4 + 1] = (byte)(v[i] >> 16);
            result[i * 4 + 2] = (byte)(v[i] >> 8);
            result[i * 4 + 3] = (byte)v[i];
        }
        return Convert.ToHexStringLower(result);
    }

    /// <summary>
    /// SM3 消息填充：追加 0x80 + 若干 0，使消息长度 mod 512 == 448，
    /// 最后 64 位写入原始消息的位长度（大端序）。
    /// </summary>
    private static byte[] PadSm3(byte[] data)
    {
        long bitLen = data.LongLength * 8;

        // 计算填充长度：data.Length % 64 < 56 时补齐到 56，否则补齐到 56+64
        int padLen = (data.Length % 64 < 56) ? (56 - data.Length % 64) : (120 - data.Length % 64);

        var padded = new byte[data.Length + padLen + 8];
        Array.Copy(data, padded, data.Length);

        // 第 1 个填充字节为 0x80（二进制 1000 0000）
        padded[data.Length] = 0x80;

        // 最后 8 字节写入原始消息长度（大端序）
        for (int i = 0; i < 8; i++)
            padded[padded.Length - 1 - i] = (byte)(bitLen >> (8 * i));

        return padded;
    }

    /// <summary>32 位字循环左移 n 位。</summary>
    private static uint Rotl(uint x, int n) => (x << n) | (x >> (32 - n));

    /// <summary>SM3 置换函数 P0：x ^ (x&lt;&lt;&lt;9) ^ (x&lt;&lt;&lt;17)。</summary>
    private static uint P0(uint x) => x ^ Rotl(x, 9) ^ Rotl(x, 17);

    /// <summary>SM3 置换函数 P1：x ^ (x&lt;&lt;&lt;15) ^ (x&lt;&lt;&lt;23)。</summary>
    private static uint P1(uint x) => x ^ Rotl(x, 15) ^ Rotl(x, 23);

    /// <summary>SM3 布尔函数 FF0（前 16 轮）：x ^ y ^ z。</summary>
    private static uint FF0(uint x, uint y, uint z) => x ^ y ^ z;

    /// <summary>SM3 布尔函数 FF1（后 48 轮）：(x&amp;y) | (x&amp;z) | (y&amp;z)。</summary>
    private static uint FF1(uint x, uint y, uint z) => (x & y) | (x & z) | (y & z);

    /// <summary>SM3 布尔函数 GG0（前 16 轮）：x ^ y ^ z。</summary>
    private static uint GG0(uint x, uint y, uint z) => x ^ y ^ z;

    /// <summary>SM3 布尔函数 GG1（后 48 轮）：(x&amp;y) | ((~x)&amp;z)。</summary>
    private static uint GG1(uint x, uint y, uint z) => (x & y) | ((~x) & z);

    /// <summary>将 4 个字节按大端序组合为 32 位无符号整数。</summary>
    private static uint BytesToUint(byte[] bytes, int i) =>
        (uint)(bytes[i] << 24) | (uint)(bytes[i + 1] << 16) | (uint)(bytes[i + 2] << 8) | bytes[i + 3];
}
