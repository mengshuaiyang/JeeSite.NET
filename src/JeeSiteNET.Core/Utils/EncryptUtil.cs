using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

// ================================================================
// 加密工具类 —— 提供项目中常用的哈希算法
//
// 包含：MD5（兼容旧系统）、SHA-1、SHA-256、SM3（国密标准）
//
// 使用场景：
//   - 密码存储（AuthService.cs 中 LoginAsync 和 RegisterAsync 使用 MD5）
//   - 数据完整性校验
//   - 参数签名
//
// 对应的其他加密类：
//   RsaUtil.cs    — RSA 非对称加密（密钥对/加密/解密/签名/验签）
//   Sm2Util.cs    — SM2 国密椭圆曲线（签名/验签/混合加密）
//   Sm4Util.cs    — SM4 国密对称加密（CBC 模式）
//
// 重要：MD5 仅用于兼容 JeeSite5 现有数据。
//       新系统建议用 SM3 或 SHA-256。
// ================================================================

/// <summary>通用哈希与加密工具（MD5/SHA-1/SHA-256/SM3），全部返回小写十六进制字符串。</summary>
public static class EncryptUtil
{
    /// <summary>MD5 哈希（128位 → 32位 hex）。注意：仅用于兼容 JeeSite5 已有数据。</summary>
    public static string Md5(string input)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    /// <summary>SHA-1 哈希（160位 → 40位 hex）。</summary>
    public static string Sha1(string input)
    {
        var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    /// <summary>SHA-256 哈希（256位 → 64位 hex）。推荐用于密码存储。</summary>
    public static string Sha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    // ========== SM3 国产密码杂凑算法（GM/T 0004-2012）==========
    // 与 SHA-256 等价的国密标准，输出 256 位（32 字节）。
    // 
    // 实现步骤：
    //   ① 消息填充：补 0x80 + 0x00 至长度 mod 512 = 448，最后 64 位记录原长
    //   ② 迭代压缩：每 512 位（64 字节）一块，共 64 轮
    //   ③ 输出 8 个 32 位状态字拼接为 32 字节摘要
    //
    // 使用场景：需要满足中国国家密码管理局合规要求的项目

    /// <summary>SM3 哈希（256位 → 64位 hex），遵循 GM/T 0004-2012 标准。</summary>
    public static string Sm3(string input)
    {
        var data = Encoding.UTF8.GetBytes(input);
        var padded = PadSm3(data);

        // 8 个 32 位初始值（SM3 协议规定的固定 IV）
        var v = new uint[] { 0x7380166F, 0x4914B2B9, 0x172442D7, 0xDA8A0600, 0xA96F30BC, 0xE1638AA5, 0xE38DEE4D, 0xB0FB0E4E };
        var w = new uint[68];
        var w2 = new uint[64];

        for (int i = 0; i < padded.Length; i += 64)
        {
            // 将 16 个字装入 w[0..15]
            for (int t = 0; t < 16; t++)
                w[t] = BytesToUint(padded, i + t * 4);

            // 递推生成 w[16..67]
            for (int t = 16; t < 68; t++)
                w[t] = P1(w[t - 16] ^ w[t - 9] ^ Rotl(w[t - 3], 15)) ^ Rotl(w[t - 13], 7) ^ w[t - 6];

            // w'[t] = w[t] ^ w[t+4]
            for (int t = 0; t < 64; t++)
                w2[t] = w[t] ^ w[t + 4];

            var a = v[0]; var b = v[1]; var c = v[2]; var d = v[3];
            var e = v[4]; var f = v[5]; var g = v[6]; var h = v[7];

            // 前 16 轮（FF0/GG0 + 0x79CC4519）
            for (int t = 0; t < 16; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x79CC4519, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF0(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG0(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

            // 后 48 轮（FF1/GG1 + 0x7A879D8A）
            for (int t = 16; t < 64; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x7A879D8A, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF1(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG1(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

            // Davies-Meyer 压缩：本轮结果 ⊕ 旧状态
            v[0] ^= a; v[1] ^= b; v[2] ^= c; v[3] ^= d;
            v[4] ^= e; v[5] ^= f; v[6] ^= g; v[7] ^= h;
        }

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

    /// <summary>SM3 填充：补 0x80 + 0x00 + 64 位原始长度（大端）。</summary>
    private static byte[] PadSm3(byte[] data)
    {
        long bitLen = data.LongLength * 8;
        int padLen = (data.Length % 64 < 56) ? (56 - data.Length % 64) : (120 - data.Length % 64);
        var padded = new byte[data.Length + padLen + 8];
        Array.Copy(data, padded, data.Length);
        padded[data.Length] = 0x80;
        for (int i = 0; i < 8; i++)
            padded[padded.Length - 1 - i] = (byte)(bitLen >> (8 * i));
        return padded;
    }

    /// <summary>32 位字循环左移 n 位。</summary>
    private static uint Rotl(uint x, int n) => (x << n) | (x >> (32 - n));

    /// <summary>SM3 置换 P0：x ^ (x&lt;&lt;&lt;9) ^ (x&lt;&lt;&lt;17)。</summary>
    private static uint P0(uint x) => x ^ Rotl(x, 9) ^ Rotl(x, 17);

    /// <summary>SM3 置换 P1：x ^ (x&lt;&lt;&lt;15) ^ (x&lt;&lt;&lt;23)。</summary>
    private static uint P1(uint x) => x ^ Rotl(x, 15) ^ Rotl(x, 23);

    /// <summary>SM3 FF0（前 16 轮）：x ^ y ^ z。</summary>
    private static uint FF0(uint x, uint y, uint z) => x ^ y ^ z;

    /// <summary>SM3 FF1（后 48 轮）：(x&amp;y)|(x&amp;z)|(y&amp;z)。</summary>
    private static uint FF1(uint x, uint y, uint z) => (x & y) | (x & z) | (y & z);

    /// <summary>SM3 GG0（前 16 轮）：x ^ y ^ z。</summary>
    private static uint GG0(uint x, uint y, uint z) => x ^ y ^ z;

    /// <summary>SM3 GG1（后 48 轮）：(x&amp;y)|((~x)&amp;z)。</summary>
    private static uint GG1(uint x, uint y, uint z) => (x & y) | ((~x) & z);

    /// <summary>4 字节大端 → uint32。</summary>
    private static uint BytesToUint(byte[] bytes, int i) =>
        (uint)(bytes[i] << 24) | (uint)(bytes[i + 1] << 16) | (uint)(bytes[i + 2] << 8) | bytes[i + 3];
}
