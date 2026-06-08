using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class EncryptUtil
{
    public static string Md5(string input)
    {
        var bytes = MD5.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    public static string Sha1(string input)
    {
        var bytes = SHA1.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    public static string Sha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexStringLower(bytes);
    }

    public static string Sm3(string input)
    {
        var data = Encoding.UTF8.GetBytes(input);
        var padded = PadSm3(data);
        var v = new uint[] { 0x7380166F, 0x4914B2B9, 0x172442D7, 0xDA8A0600, 0xA96F30BC, 0xE1638AA5, 0xE38DEE4D, 0xB0FB0E4E };
        var w = new uint[68];
        var w2 = new uint[64];

        for (int i = 0; i < padded.Length; i += 64)
        {
            for (int t = 0; t < 16; t++)
                w[t] = BytesToUint(padded, i + t * 4);

            for (int t = 16; t < 68; t++)
                w[t] = P1(w[t - 16] ^ w[t - 9] ^ Rotl(w[t - 3], 15)) ^ Rotl(w[t - 13], 7) ^ w[t - 6];

            for (int t = 0; t < 64; t++)
                w2[t] = w[t] ^ w[t + 4];

            var a = v[0]; var b = v[1]; var c = v[2]; var d = v[3];
            var e = v[4]; var f = v[5]; var g = v[6]; var h = v[7];

            for (int t = 0; t < 16; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x79CC4519, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF0(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG0(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

            for (int t = 16; t < 64; t++)
            {
                var ss1 = Rotl(Rotl(a, 12) + e + Rotl(0x7A879D8A, t), 7);
                var ss2 = ss1 ^ Rotl(a, 12);
                var tt1 = FF1(a, b, c) + d + ss2 + w2[t];
                var tt2 = GG1(e, f, g) + h + ss1 + w[t];
                d = c; c = Rotl(b, 9); b = a; a = tt1;
                h = g; g = Rotl(f, 19); f = e; e = P0(tt2);
            }

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

    private static uint Rotl(uint x, int n) => (x << n) | (x >> (32 - n));
    private static uint P0(uint x) => x ^ Rotl(x, 9) ^ Rotl(x, 17);
    private static uint P1(uint x) => x ^ Rotl(x, 15) ^ Rotl(x, 23);
    private static uint FF0(uint x, uint y, uint z) => x ^ y ^ z;
    private static uint FF1(uint x, uint y, uint z) => (x & y) | (x & z) | (y & z);
    private static uint GG0(uint x, uint y, uint z) => x ^ y ^ z;
    private static uint GG1(uint x, uint y, uint z) => (x & y) | ((~x) & z);
    private static uint BytesToUint(byte[] bytes, int i) =>
        (uint)(bytes[i] << 24) | (uint)(bytes[i + 1] << 16) | (uint)(bytes[i + 2] << 8) | bytes[i + 3];
}
