using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using System.Text;

namespace JeeSiteNET.Core.Utils;

/// <summary>
/// SM2 椭圆曲线国密算法工具类（基于 BouncyCastle 标准实现）。
/// 曲线：sm2p256v1（GB/T 32918-2016）。
/// 签名：SM2withSM3（GM/T 0003.2-2012）。
/// 加密：C1C2C3 ASN.1 DER 混合加密。
/// </summary>
public static class Sm2Util
{
    /// <summary>
    /// SM2 曲线域参数（p, a, b, G, n, h），全局初始化一次即可复用。
    /// </summary>
    private static readonly ECDomainParameters _sm2Domain = InitSm2Domain();

    /// <summary>
    /// 加密安全的随机数生成器，用于密钥生成与加密过程中的随机量。
    /// </summary>
    private static readonly SecureRandom _random = new();

    /// <summary>
    /// 初始化 SM2 标准曲线参数（sm2p256v1）。
    /// </summary>
    /// <returns>SM2 椭圆曲线域参数对象。</returns>
    /// <exception cref="InvalidOperationException">当 BouncyCastle 无法定位 sm2p256v1 曲线时抛出。</exception>
    private static ECDomainParameters InitSm2Domain()
    {
        var ecParams = Org.BouncyCastle.Crypto.EC.CustomNamedCurves.GetByName("sm2p256v1");
        if (ecParams == null)
            throw new InvalidOperationException("无法获取 sm2p256v1 曲线参数");
        return new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());
    }

    /// <summary>
    /// 将无符号大端字节数组转换为 BouncyCastle 的 BigInteger（正数表示）。
    /// </summary>
    /// <param name="bytes">无符号大端字节数组。</param>
    /// <returns>正整数 BigInteger。</returns>
    private static BigInteger ByteArrayToBigInt(byte[] bytes) => new(1, bytes);

    /// <summary>
    /// 生成 256 位 SM2 私钥，返回小写 hex 字符串（固定 64 字符）。
    /// </summary>
    /// <returns>私钥 hex（64 字符，小写）。</returns>
    public static string GeneratePrivateKey()
    {
        var gen = new ECKeyGenerationParameters(_sm2Domain, _random);
        var keyGen = new ECKeyPairGenerator();
        keyGen.Init(gen);
        var pair = keyGen.GenerateKeyPair();
        var priv = (ECPrivateKeyParameters)pair.Private;
        var bytes = priv.D.ToByteArrayUnsigned();
        // 补齐/截断为精确 32 字节，保证 hex 输出固定 64 字符
        return Convert.ToHexString(bytes.Length == 32 ? bytes : FixLength32(bytes)).ToLower();
    }

    /// <summary>
    /// 从私钥计算 SM2 公钥（未压缩格式：04 + X + Y），输出 130 字符小写 hex。
    /// </summary>
    /// <param name="privateKeyHex">私钥 hex（64 字符）。</param>
    /// <returns>公钥 hex（130 字符，前缀 04）。</returns>
    public static string GeneratePublicKey(string privateKeyHex)
    {
        var d = ByteArrayToBigInt(Convert.FromHexString(privateKeyHex));
        // 公钥 Q = d * G（G 为基点，曲线标量乘法）
        var q = _sm2Domain.G.Multiply(d).Normalize();
        var x = FixLength32(q.AffineXCoord.ToBigInteger().ToByteArrayUnsigned());
        var y = FixLength32(q.AffineYCoord.ToBigInteger().ToByteArrayUnsigned());
        // 未压缩公钥：0x04 || X(32) || Y(32)
        return "04" + Convert.ToHexString(x).ToLower() + Convert.ToHexString(y).ToLower();
    }

    /// <summary>
    /// 一次性生成 SM2 私钥/公钥对。
    /// </summary>
    /// <returns>(PrivateKey = 64 hex, PublicKey = 130 hex)。</returns>
    public static (string PrivateKey, string PublicKey) GenerateKeyPair()
    {
        var priv = GeneratePrivateKey();
        var pub = GeneratePublicKey(priv);
        return (priv, pub);
    }

    /// <summary>
    /// 使用 SM2withSM3 签名算法对给定数据签名，默认用户 ID 为 "1234567812345678"。
    /// 返回 DER 编码的签名 hex（小写）。
    /// </summary>
    /// <param name="privateKeyHex">签名者的私钥 hex。</param>
    /// <param name="data">待签名的字符串（UTF-8 编码）。</param>
    /// <returns>DER 签名 hex（小写）。</returns>
    public static string Sign(string privateKeyHex, string data)
    {
        var privParams = CreatePrivateKeyParameters(privateKeyHex);
        var signer = SignerUtilities.GetSigner("SM3withSM2");
        // SM2 规范要求携带用户 ID（默认 1234567812345678），参与 Z 值计算
        signer.Init(true, new ParametersWithID(privParams, Encoding.ASCII.GetBytes("1234567812345678")));
        signer.BlockUpdate(Encoding.UTF8.GetBytes(data));
        var sig = signer.GenerateSignature();
        return Convert.ToHexString(sig).ToLower();
    }

    /// <summary>
    /// 使用 SM2withSM3 验证数据签名。
    /// </summary>
    /// <param name="publicKeyHex">签名者的公钥 hex（允许带或不带 04 前缀）。</param>
    /// <param name="data">原文数据（UTF-8 编码）。</param>
    /// <param name="signatureHex">待验证的 DER 签名 hex。</param>
    /// <returns>true = 验证通过；false = 签名无效。</returns>
    public static bool Verify(string publicKeyHex, string data, string signatureHex)
    {
        var pubParams = CreatePublicKeyParameters(publicKeyHex);
        var signer = SignerUtilities.GetSigner("SM3withSM2");
        signer.Init(false, new ParametersWithID(pubParams, Encoding.ASCII.GetBytes("1234567812345678")));
        signer.BlockUpdate(Encoding.UTF8.GetBytes(data));
        return signer.VerifySignature(Convert.FromHexString(signatureHex));
    }

    /// <summary>
    /// 使用 SM2 椭圆曲线加密（C1C2C3 ASN.1 DER 格式）加密明文。
    /// </summary>
    /// <param name="publicKeyHex">接收方公钥 hex。</param>
    /// <param name="plainText">明文字符串（UTF-8 编码）。</param>
    /// <returns>加密后的密文 hex（小写）。</returns>
    public static string Encrypt(string publicKeyHex, string plainText)
    {
        var pubParams = CreatePublicKeyParameters(publicKeyHex);
        var engine = new SM2Engine();
        engine.Init(true, new ParametersWithRandom(pubParams, _random));
        var cipherBytes = engine.ProcessBlock(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(cipherBytes).ToLower();
    }

    /// <summary>
    /// 使用 SM2 椭圆曲线解密 C1C2C3 ASN.1 DER 密文。
    /// </summary>
    /// <param name="privateKeyHex">接收方私钥 hex。</param>
    /// <param name="cipherText">密文 hex。</param>
    /// <returns>解密后的原文（UTF-8 字符串）。</returns>
    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        var privParams = CreatePrivateKeyParameters(privateKeyHex);
        var engine = new SM2Engine();
        engine.Init(false, privParams);
        var plainBytes = engine.ProcessBlock(Convert.FromHexString(cipherText));
        return Encoding.UTF8.GetString(plainBytes);
    }

    /// <summary>
    /// 将任意长度字节数组规整为 32 字节：超过则取尾部 32 字节，不足则左补零。
    /// 用于保证 SM2 X/Y 坐标与私钥的固定输出长度。
    /// </summary>
    /// <param name="bytes">原始字节数组。</param>
    /// <returns>32 字节数组（大端，左补零）。</returns>
    private static byte[] FixLength32(byte[] bytes)
    {
        if (bytes.Length == 32) return bytes;
        if (bytes.Length > 32) return bytes.TakeLast(32).ToArray();
        var result = new byte[32];
        // 左补零：将 bytes 放在 result 的尾部（大端表示）
        Array.Copy(bytes, 0, result, 32 - bytes.Length, bytes.Length);
        return result;
    }

    /// <summary>
    /// 将 hex 私钥转换为 BouncyCastle EC 私钥参数对象。
    /// </summary>
    /// <param name="privateKeyHex">hex 私钥。</param>
    /// <returns>BouncyCastle EC 私钥参数。</returns>
    private static ECPrivateKeyParameters CreatePrivateKeyParameters(string privateKeyHex)
    {
        var d = ByteArrayToBigInt(Convert.FromHexString(privateKeyHex));
        return new ECPrivateKeyParameters(d, _sm2Domain);
    }

    /// <summary>
    /// 将 hex 公钥（允许带或不带 04 前缀）转换为 BouncyCastle EC 公钥参数对象。
    /// </summary>
    /// <param name="publicKeyHex">hex 公钥。</param>
    /// <returns>BouncyCastle EC 公钥参数。</returns>
    private static ECPublicKeyParameters CreatePublicKeyParameters(string publicKeyHex)
    {
        // 允许两种输入：完整的未压缩公钥（含 04 前缀）或裸 X||Y
        var hex = publicKeyHex.StartsWith("04") ? publicKeyHex[2..] : publicKeyHex;
        // X 取前 32 字节（64 hex），Y 取后 32 字节
        var x = ByteArrayToBigInt(Convert.FromHexString(hex[..64]));
        var y = ByteArrayToBigInt(Convert.FromHexString(hex[64..]));
        var q = _sm2Domain.Curve.CreatePoint(x, y).Normalize();
        return new ECPublicKeyParameters(q, _sm2Domain);
    }
}
