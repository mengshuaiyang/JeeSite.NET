using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>SM4 对称加密工具类（基于 BouncyCastle 标准实现）。</summary>
public static class Sm4Util
{
    private const int SM4_KEY_SIZE = 16;
    private const int SM4_BLOCK_SIZE = 16;
    private static readonly SecureRandom _random = new();

    /// <summary>生成 16 字节随机 SM4 密钥（hex 小写 32 字符）</summary>
    public static string GenerateKey()
    {
        var key = new byte[SM4_KEY_SIZE];
        _random.NextBytes(key);
        return Convert.ToHexString(key).ToLower();
    }

    /// <summary>SM4-CBC 加密：输出格式 = IV(16) + CipherText(PKCS7 padded)，hex 编码</summary>
    public static string EncryptCbc(string keyHex, string plainText)
    {
        var key = Convert.FromHexString(keyHex);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var iv = new byte[SM4_BLOCK_SIZE];
        _random.NextBytes(iv);

        var engine = new CbcBlockCipher(new SM4Engine());
        var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());
        cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

        var output = new byte[cipher.GetOutputSize(plainBytes.Length)];
        var len = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, output, 0);
        len += cipher.DoFinal(output, len);

        var result = new byte[iv.Length + len];
        Array.Copy(iv, 0, result, 0, iv.Length);
        Array.Copy(output, 0, result, iv.Length, len);
        return Convert.ToHexString(result).ToLower();
    }

    /// <summary>SM4-CBC 解密：输入格式 = IV(16) + CipherText</summary>
    public static string DecryptCbc(string keyHex, string cipherText)
    {
        var key = Convert.FromHexString(keyHex);
        var fullBytes = Convert.FromHexString(cipherText);
        var iv = fullBytes[..SM4_BLOCK_SIZE];
        var cipherBytes = fullBytes[SM4_BLOCK_SIZE..];

        var engine = new CbcBlockCipher(new SM4Engine());
        var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());
        cipher.Init(false, new ParametersWithIV(new KeyParameter(key), iv));

        var output = new byte[cipher.GetOutputSize(cipherBytes.Length)];
        var len = cipher.ProcessBytes(cipherBytes, 0, cipherBytes.Length, output, 0);
        len += cipher.DoFinal(output, len);

        var plain = new byte[len];
        Array.Copy(output, 0, plain, 0, len);
        return Encoding.UTF8.GetString(plain);
    }
}
