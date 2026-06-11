using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class Sm2Util
{
    public static string GeneratePrivateKey()
    {
        using var ecdsa = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var param = ecdsa.ExportParameters(true);
        var bytes = param.D ?? throw new InvalidOperationException("私钥生成失败");
        return Convert.ToHexString(bytes).ToLower();
    }

    public static string GeneratePublicKey(string privateKeyHex)
    {
        using var ecdsa = ECDsa.Create();
        var param = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            D = Convert.FromHexString(privateKeyHex)
        };
        ecdsa.ImportParameters(param);
        var pub = ecdsa.ExportParameters(false);
        var qx = pub.Q.X ?? throw new InvalidOperationException("公钥生成失败");
        var qy = pub.Q.Y ?? throw new InvalidOperationException("公钥生成失败");
        return "04" + Convert.ToHexString(qx).ToLower() + Convert.ToHexString(qy).ToLower();
    }

    public static (string PrivateKey, string PublicKey) GenerateKeyPair()
    {
        var privateKey = GeneratePrivateKey();
        var publicKey = GeneratePublicKey(privateKey);
        return (privateKey, publicKey);
    }

    public static string Sign(string privateKeyHex, string data)
    {
        using var ecdsa = ECDsa.Create();
        var param = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            D = Convert.FromHexString(privateKeyHex)
        };
        ecdsa.ImportParameters(param);

        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signature = ecdsa.SignData(dataBytes, HashAlgorithmName.SHA256);
        return Convert.ToHexString(signature).ToLower();
    }

    public static bool Verify(string publicKeyHex, string data, string signatureHex)
    {
        using var ecdsa = ECDsa.Create();
        var pubKey = publicKeyHex.StartsWith("04") ? publicKeyHex[2..] : publicKeyHex;
        var qx = Convert.FromHexString(pubKey[..(pubKey.Length / 2)]);
        var qy = Convert.FromHexString(pubKey[(pubKey.Length / 2)..]);

        var param = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            Q = { X = qx, Y = qy }
        };
        ecdsa.ImportParameters(param);

        var dataBytes = Encoding.UTF8.GetBytes(data);
        var sigBytes = Convert.FromHexString(signatureHex);

        if (sigBytes.Length == 64)
        {
            var formatted = new byte[68];
            formatted[0] = 0x30;
            formatted[1] = 0x44;
            formatted[2] = 0x02;
            formatted[3] = 0x20;
            Array.Copy(sigBytes, 0, formatted, 4, 32);
            formatted[36] = 0x02;
            formatted[37] = 0x20;
            Array.Copy(sigBytes, 32, formatted, 38, 32);
            return ecdsa.VerifyData(dataBytes, formatted, HashAlgorithmName.SHA256);
        }

        return ecdsa.VerifyData(dataBytes, sigBytes, HashAlgorithmName.SHA256);
    }

    public static string Encrypt(string publicKeyHex, string plainText)
    {
        return EncryptByPassword(publicKeyHex, plainText);
    }

    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        return DecryptByPassword(privateKeyHex, cipherText);
    }

    private static string EncryptByPassword(string publicKeyHex, string plainText)
    {
        var password = Guid.NewGuid().ToString("N")[..16];
        using var aes = Aes.Create();
        aes.KeySize = 256;
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
        var salt = RandomNumberGenerator.GetBytes(16);
        var key = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, salt, 10000, HashAlgorithmName.SHA256, 32);
        var iv = RandomNumberGenerator.GetBytes(16);
        aes.Key = key;
        aes.IV = iv;

        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        using var encryptor = aes.CreateEncryptor();
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        var encryptedPassword = Convert.ToHexString(
            Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt, 1, HashAlgorithmName.SHA256, 32));

        var result = new byte[salt.Length + iv.Length + cipherBytes.Length];
        Array.Copy(salt, 0, result, 0, salt.Length);
        Array.Copy(iv, 0, result, salt.Length, iv.Length);
        Array.Copy(cipherBytes, 0, result, salt.Length + iv.Length, cipherBytes.Length);

        return Convert.ToHexString(result).ToLower() + encryptedPassword;
    }

    private static string DecryptByPassword(string privateKeyHex, string cipherText)
    {
        var fullBytes = Convert.FromHexString(cipherText);
        int saltLen = 16, ivLen = 16;
        var salt = fullBytes[..saltLen];
        var iv = fullBytes[saltLen..(saltLen + ivLen)];
        var cipherBytes = fullBytes[(saltLen + ivLen)..^64];
        var passwordHash = Encoding.UTF8.GetString(fullBytes[^64..]);

        var password = new string(cipherText[^64..].Where(char.IsLetterOrDigit).Take(32).ToArray());

        using var aes = Aes.Create();
        var key = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt, 10000, HashAlgorithmName.SHA256, 32);
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
