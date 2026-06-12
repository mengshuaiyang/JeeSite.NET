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
    private static readonly ECDomainParameters _sm2Domain = InitSm2Domain();
    private static readonly SecureRandom _random = new();

    private static ECDomainParameters InitSm2Domain()
    {
        var ecParams = Org.BouncyCastle.Crypto.EC.CustomNamedCurves.GetByName("sm2p256v1");
        if (ecParams == null)
            throw new InvalidOperationException("无法获取 sm2p256v1 曲线参数");
        return new ECDomainParameters(ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());
    }

    private static BigInteger ByteArrayToBigInt(byte[] bytes) => new(1, bytes);

    /// <summary>生成 256 位 SM2 私钥（hex 小写，64 字符）</summary>
    public static string GeneratePrivateKey()
    {
        var gen = new ECKeyGenerationParameters(_sm2Domain, _random);
        var keyGen = new ECKeyPairGenerator();
        keyGen.Init(gen);
        var pair = keyGen.GenerateKeyPair();
        var priv = (ECPrivateKeyParameters)pair.Private;
        return priv.D.ToByteArrayUnsigned().Length == 32
            ? Convert.ToHexString(priv.D.ToByteArrayUnsigned()).ToLower()
            : Convert.ToHexString(FixLength32(priv.D.ToByteArrayUnsigned())).ToLower();
    }

    /// <summary>从私钥生成 SM2 公钥（未压缩格式：04 + X + Y，hex 130 字符）</summary>
    public static string GeneratePublicKey(string privateKeyHex)
    {
        var d = ByteArrayToBigInt(Convert.FromHexString(privateKeyHex));
        var q = _sm2Domain.G.Multiply(d).Normalize();
        var x = FixLength32(q.AffineXCoord.ToBigInteger().ToByteArrayUnsigned());
        var y = FixLength32(q.AffineYCoord.ToBigInteger().ToByteArrayUnsigned());
        return "04" + Convert.ToHexString(x).ToLower() + Convert.ToHexString(y).ToLower();
    }

    public static (string PrivateKey, string PublicKey) GenerateKeyPair()
    {
        var priv = GeneratePrivateKey();
        var pub = GeneratePublicKey(priv);
        return (priv, pub);
    }

    /// <summary>SM2withSM3 签名（默认用户 ID = 1234567812345678）。返回 DER hex。</summary>
    public static string Sign(string privateKeyHex, string data)
    {
        var privParams = CreatePrivateKeyParameters(privateKeyHex);
        var signer = SignerUtilities.GetSigner("SM3withSM2");
        signer.Init(true, new ParametersWithID(privParams, Encoding.ASCII.GetBytes("1234567812345678")));
        signer.BlockUpdate(Encoding.UTF8.GetBytes(data));
        var sig = signer.GenerateSignature();
        return Convert.ToHexString(sig).ToLower();
    }

    public static bool Verify(string publicKeyHex, string data, string signatureHex)
    {
        var pubParams = CreatePublicKeyParameters(publicKeyHex);
        var signer = SignerUtilities.GetSigner("SM3withSM2");
        signer.Init(false, new ParametersWithID(pubParams, Encoding.ASCII.GetBytes("1234567812345678")));
        signer.BlockUpdate(Encoding.UTF8.GetBytes(data));
        return signer.VerifySignature(Convert.FromHexString(signatureHex));
    }

    /// <summary>SM2 椭圆曲线加密（C1C2C3 ASN.1 DER）</summary>
    public static string Encrypt(string publicKeyHex, string plainText)
    {
        var pubParams = CreatePublicKeyParameters(publicKeyHex);
        var engine = new SM2Engine();
        engine.Init(true, new ParametersWithRandom(pubParams, _random));
        var cipherBytes = engine.ProcessBlock(Encoding.UTF8.GetBytes(plainText));
        return Convert.ToHexString(cipherBytes).ToLower();
    }

    /// <summary>SM2 椭圆曲线解密</summary>
    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        var privParams = CreatePrivateKeyParameters(privateKeyHex);
        var engine = new SM2Engine();
        engine.Init(false, privParams);
        var plainBytes = engine.ProcessBlock(Convert.FromHexString(cipherText));
        return Encoding.UTF8.GetString(plainBytes);
    }

    private static byte[] FixLength32(byte[] bytes)
    {
        if (bytes.Length == 32) return bytes;
        if (bytes.Length > 32) return bytes.TakeLast(32).ToArray();
        var result = new byte[32];
        Array.Copy(bytes, 0, result, 32 - bytes.Length, bytes.Length);
        return result;
    }

    private static ECPrivateKeyParameters CreatePrivateKeyParameters(string privateKeyHex)
    {
        var d = ByteArrayToBigInt(Convert.FromHexString(privateKeyHex));
        return new ECPrivateKeyParameters(d, _sm2Domain);
    }

    private static ECPublicKeyParameters CreatePublicKeyParameters(string publicKeyHex)
    {
        var hex = publicKeyHex.StartsWith("04") ? publicKeyHex[2..] : publicKeyHex;
        var x = ByteArrayToBigInt(Convert.FromHexString(hex[..64]));
        var y = ByteArrayToBigInt(Convert.FromHexString(hex[64..]));
        var q = _sm2Domain.Curve.CreatePoint(x, y).Normalize();
        return new ECPublicKeyParameters(q, _sm2Domain);
    }
}
