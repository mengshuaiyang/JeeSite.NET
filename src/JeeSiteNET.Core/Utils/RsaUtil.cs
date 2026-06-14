using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// RSA 非对称加密工具类。默认密钥长度 2048 位，加密填充使用 OAEP SHA-256，
/// 与传统 PKCS#1 v1.5 相比具备更强的可证明安全性。
/// </summary>
public static class RsaUtil
{
    /// <summary>
    /// 生成 RSA 密钥对，返回 (PrivateKey, PublicKey) 的小写 hex 格式。
    /// </summary>
    /// <param name="keySize">密钥长度（位），默认 2048。</param>
    /// <returns>(PrivateKey = PKCS#8 DER hex, PublicKey = PKCS#1 DER hex)。</returns>
    public static (string PublicKey, string PrivateKey) GenerateKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        var privateKey = Convert.ToHexString(rsa.ExportRSAPrivateKey());
        var publicKey = Convert.ToHexString(rsa.ExportRSAPublicKey());
        return (publicKey, privateKey);
    }

    /// <summary>
    /// 使用 RSA 公钥加密明文。对于 2048 位密钥，明文最大长度约 190 字节（受 OAEP 填充开销限制）。
    /// </summary>
    /// <param name="publicKeyHex">PKCS#1 DER hex 公钥。</param>
    /// <param name="plainText">明文字符串（UTF-8 编码）。</param>
    /// <returns>密文 hex（小写）。</returns>
    public static string Encrypt(string publicKeyHex, string plainText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromHexString(publicKeyHex), out _);
        var data = Encoding.UTF8.GetBytes(plainText);
        // OAEP SHA-256 可防御选择密文攻击（CCA），比 PKCS#1 v1.5 更安全
        var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToHexString(encrypted);
    }

    /// <summary>
    /// 使用 RSA 私钥解密密文。
    /// </summary>
    /// <param name="privateKeyHex">PKCS#8 DER hex 私钥。</param>
    /// <param name="cipherText">密文 hex。</param>
    /// <returns>解密后的 UTF-8 明文字符串。</returns>
    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var data = Convert.FromHexString(cipherText);
        var decrypted = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decrypted);
    }

    /// <summary>
    /// 使用 RSA 私钥对数据签名（SHA-256 + PKCS#1 v1.5 签名填充）。
    /// </summary>
    /// <param name="privateKeyHex">PKCS#8 DER hex 私钥。</param>
    /// <param name="data">待签名的字符串（UTF-8 编码）。</param>
    /// <returns>签名 hex（小写）。</returns>
    public static string Sign(string privateKeyHex, string data)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var signature = rsa.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToHexString(signature).ToLower();
    }

    /// <summary>
    /// 使用 RSA 公钥验证签名。
    /// </summary>
    /// <param name="publicKeyHex">PKCS#1 DER hex 公钥。</param>
    /// <param name="data">原始字符串（UTF-8 编码）。</param>
    /// <param name="signatureHex">签名 hex。</param>
    /// <returns>true = 验证通过；false = 签名无效。</returns>
    public static bool Verify(string publicKeyHex, string data, string signatureHex)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromHexString(publicKeyHex), out _);
        return rsa.VerifyData(
            Encoding.UTF8.GetBytes(data),
            Convert.FromHexString(signatureHex),
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);
    }
}
