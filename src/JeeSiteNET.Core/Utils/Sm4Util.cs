using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// SM4 对称加密工具类（基于 BouncyCastle 标准实现）。
/// 分组长度：128 位（16 字节）。密钥长度：128 位。
/// 工作模式：CBC。填充：PKCS#7。密文布局：IV(16) + CipherText。
/// </summary>
public static class Sm4Util
{
    /// <summary>
    /// SM4 密钥字节数（128 位 = 16 字节）。
    /// </summary>
    private const int SM4_KEY_SIZE = 16;

    /// <summary>
    /// SM4 分组字节数（128 位 = 16 字节）。
    /// </summary>
    private const int SM4_BLOCK_SIZE = 16;

    /// <summary>
    /// 加密安全的随机数生成器，用于生成密钥和 IV。
    /// </summary>
    private static readonly SecureRandom _random = new();

    /// <summary>
    /// 生成 16 字节的随机 SM4 密钥，返回小写 hex（32 字符）。
    /// </summary>
    /// <returns>随机密钥 hex（32 字符，小写）。</returns>
    public static string GenerateKey()
    {
        var key = new byte[SM4_KEY_SIZE];
        _random.NextBytes(key);
        return Convert.ToHexString(key).ToLower();
    }

    /// <summary>
    /// 使用 SM4-CBC-PKCS7 加密明文。输出格式 = 随机 IV(16) + 密文（PKCS7 填充）的 hex。
    /// </summary>
    /// <param name="keyHex">32 字符十六进制密钥。</param>
    /// <param name="plainText">明文字符串（UTF-8 编码）。</param>
    /// <returns>小写 hex 密文，前 32 hex 字符为 IV。</returns>
    public static string EncryptCbc(string keyHex, string plainText)
    {
        var key = Convert.FromHexString(keyHex);
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        // 每次加密生成独立随机 IV，避免相同明文产生相同密文
        var iv = new byte[SM4_BLOCK_SIZE];
        _random.NextBytes(iv);

        var engine = new CbcBlockCipher(new SM4Engine());
        var cipher = new PaddedBufferedBlockCipher(engine, new Pkcs7Padding());
        cipher.Init(true, new ParametersWithIV(new KeyParameter(key), iv));

        var output = new byte[cipher.GetOutputSize(plainBytes.Length)];
        var len = cipher.ProcessBytes(plainBytes, 0, plainBytes.Length, output, 0);
        len += cipher.DoFinal(output, len);

        // 将 IV 放在密文前，便于解密时恢复
        var result = new byte[iv.Length + len];
        Array.Copy(iv, 0, result, 0, iv.Length);
        Array.Copy(output, 0, result, iv.Length, len);
        return Convert.ToHexString(result).ToLower();
    }

    /// <summary>
    /// 使用 SM4-CBC-PKCS7 解密密文。输入格式 = IV(16) + 密文的 hex。
    /// </summary>
    /// <param name="keyHex">32 字符十六进制密钥。</param>
    /// <param name="cipherText">小写 hex 密文（前 32 hex 为 IV）。</param>
    /// <returns>解密后的 UTF-8 明文字符串。</returns>
    public static string DecryptCbc(string keyHex, string cipherText)
    {
        var key = Convert.FromHexString(keyHex);
        var fullBytes = Convert.FromHexString(cipherText);
        // 提取前 16 字节作为 IV，剩余字节为真正的密文
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
