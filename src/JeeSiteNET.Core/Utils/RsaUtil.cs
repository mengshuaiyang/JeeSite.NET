using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

// ================================================================
// RSA 非对称加密工具类
//
// 用途：密钥交换、数字签名、数据加密
// 密钥：2048 位 RSA（可配置）
// 填充：加密用 OAEP-SHA256（抗 CCA），签名用 PKCS#1 v1.5
//
// 加密链路对比：
//   对称加密：Sm4Util  ← 适合大批量数据加密
//   非对称加密：RsaUtil ← 适合小数据加密 + 签名（如密钥交换、身份验证）
//   国密签名：Sm2Util  ← 国密合规的椭圆曲线签名
//
// 使用示例：
//   var (pub, pri) = RsaUtil.GenerateKeyPair();
//   var encrypted = RsaUtil.Encrypt(pub, "你好世界");
//   var decrypted = RsaUtil.Decrypt(pri, encrypted);
//   var sig = RsaUtil.Sign(pri, "数据");
//   var ok = RsaUtil.Verify(pub, "数据", sig);
// ================================================================

/// <summary>RSA 非对称加密工具。默认 2048 位密钥，OAEP-SHA256 加密，PKCS#1 v1.5 签名。</summary>
public static class RsaUtil
{
    /// <summary>
    /// 生成 RSA 密钥对。
    /// 公钥格式：PKCS#1 DER（"MIIBCg..." 式 hex）
    /// 私钥格式：PKCS#8 DER
    /// </summary>
    /// <param name="keySize">密钥长度，默认 2048 位（推荐）</param>
    /// <returns>(PublicKey, PrivateKey) 大写 hex 字符串</returns>
    public static (string PublicKey, string PrivateKey) GenerateKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        var privateKey = Convert.ToHexString(rsa.ExportRSAPrivateKey());
        var publicKey = Convert.ToHexString(rsa.ExportRSAPublicKey());
        return (publicKey, privateKey);
    }

    /// <summary>
    /// 公钥加密。OAEP-SHA256 填充，比 PKCS#1 v1.5 更安全。
    /// 2048 位密钥下，明文最大约 190 字节。
    /// </summary>
    public static string Encrypt(string publicKeyHex, string plainText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromHexString(publicKeyHex), out _);
        var data = Encoding.UTF8.GetBytes(plainText);
        var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToHexString(encrypted);
    }

    /// <summary>
    /// 私钥解密。与 Encrypt 配对使用。
    /// </summary>
    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var data = Convert.FromHexString(cipherText);
        var decrypted = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decrypted);
    }

    /// <summary>
    /// 私钥签名（SHA-256 + PKCS#1 v1.5）。
    /// 签名可用于验证数据完整性和来源。
    /// </summary>
    public static string Sign(string privateKeyHex, string data)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var signature = rsa.SignData(Encoding.UTF8.GetBytes(data), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToHexString(signature).ToLower();
    }

    /// <summary>
    /// 公钥验签。与 Sign 配对使用。
    /// </summary>
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
