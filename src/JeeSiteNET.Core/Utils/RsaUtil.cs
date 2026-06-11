using System.Security.Cryptography;
using System.Text;

namespace JeeSiteNET.Core.Utils;

public static class RsaUtil
{
    public static (string PublicKey, string PrivateKey) GenerateKeyPair(int keySize = 2048)
    {
        using var rsa = RSA.Create(keySize);
        var privateKey = Convert.ToHexString(rsa.ExportRSAPrivateKey());
        var publicKey = Convert.ToHexString(rsa.ExportRSAPublicKey());
        return (publicKey, privateKey);
    }

    public static string Encrypt(string publicKeyHex, string plainText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromHexString(publicKeyHex), out _);
        var data = Encoding.UTF8.GetBytes(plainText);
        var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Convert.ToHexString(encrypted);
    }

    public static string Decrypt(string privateKeyHex, string cipherText)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var data = Convert.FromHexString(cipherText);
        var decrypted = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
        return Encoding.UTF8.GetString(decrypted);
    }

    public static string Sign(string privateKeyHex, string data)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(Convert.FromHexString(privateKeyHex), out _);
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToHexString(signature);
    }

    public static bool Verify(string publicKeyHex, string data, string signatureHex)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(Convert.FromHexString(publicKeyHex), out _);
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var sigBytes = Convert.FromHexString(signatureHex);
        return rsa.VerifyData(dataBytes, sigBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}
