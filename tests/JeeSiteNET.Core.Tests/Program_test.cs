    // 引入 JeeSiteNET.Core.Utils 命名空间
// 引入命名空间：JeeSiteNET.Core.Utils
using JeeSiteNET.Core.Utils;

// 控制台输出
Console.WriteLine("=== SM2 测试 ===");
var (priv, pub) = Sm2Util.GenerateKeyPair();
// 控制台输出
Console.WriteLine($"私钥: {priv}");
// 控制台输出
Console.WriteLine($"公钥: {pub}");

// 声明并初始化变量：text
var text = "Hello 中国 国密算法! 测试 SM2 with SM3";
// 声明并初始化变量：sig
var sig = Sm2Util.Sign(priv, text);
// 控制台输出
Console.WriteLine($"签名: {sig}");
// 声明并初始化变量：ok
var ok = Sm2Util.Verify(pub, text, sig);
// 控制台输出
Console.WriteLine($"验签: {ok}");

// 声明并初始化变量：encrypted
var encrypted = Sm2Util.Encrypt(pub, text);
// 控制台输出
Console.WriteLine($"加密: {encrypted}");
// 声明并初始化变量：decrypted
var decrypted = Sm2Util.Decrypt(priv, encrypted);
// 控制台输出
Console.WriteLine($"解密: {decrypted}");
// 控制台输出
Console.WriteLine($"明文匹配: {decrypted == text}");

// 控制台输出
Console.WriteLine("\n=== SM3 测试 ===");
// 声明并初始化变量：hash
var hash = EncryptUtil.Sm3(text);
// 控制台输出
Console.WriteLine($"SM3: {hash}");
// 控制台输出
Console.WriteLine($"长度: {hash.Length} chars");

// 控制台输出
Console.WriteLine("\n=== SM4 测试 ===");
// 声明并初始化变量：key
var key = Sm4Util.GenerateKey();
// 控制台输出
Console.WriteLine($"密钥: {key}");
// 声明并初始化变量：cbcEnc
var cbcEnc = Sm4Util.EncryptCbc(key, text);
// 声明并初始化变量：cbcDec
var cbcDec = Sm4Util.DecryptCbc(key, cbcEnc);
// 控制台输出
Console.WriteLine($"SM4-CBC 解密匹配: {cbcDec == text}");
