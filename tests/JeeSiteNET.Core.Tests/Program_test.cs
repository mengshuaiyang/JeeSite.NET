using JeeSiteNET.Core.Utils;

Console.WriteLine("=== SM2 测试 ===");
var (priv, pub) = Sm2Util.GenerateKeyPair();
Console.WriteLine($"私钥: {priv}");
Console.WriteLine($"公钥: {pub}");

var text = "Hello 中国 国密算法! 测试 SM2 with SM3";
var sig = Sm2Util.Sign(priv, text);
Console.WriteLine($"签名: {sig}");
var ok = Sm2Util.Verify(pub, text, sig);
Console.WriteLine($"验签: {ok}");

var encrypted = Sm2Util.Encrypt(pub, text);
Console.WriteLine($"加密: {encrypted}");
var decrypted = Sm2Util.Decrypt(priv, encrypted);
Console.WriteLine($"解密: {decrypted}");
Console.WriteLine($"明文匹配: {decrypted == text}");

Console.WriteLine("\n=== SM3 测试 ===");
var hash = EncryptUtil.Sm3(text);
Console.WriteLine($"SM3: {hash}");
Console.WriteLine($"长度: {hash.Length} chars");

Console.WriteLine("\n=== SM4 测试 ===");
var key = Sm4Util.GenerateKey();
Console.WriteLine($"密钥: {key}");
var cbcEnc = Sm4Util.EncryptCbc(key, text);
var cbcDec = Sm4Util.DecryptCbc(key, cbcEnc);
Console.WriteLine($"SM4-CBC 解密匹配: {cbcDec == text}");
