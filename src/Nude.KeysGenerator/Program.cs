using System.Security.Cryptography;

using var rsa = RSA.Create();
var privateKey = rsa.ExportRSAPrivateKey();
await File.WriteAllBytesAsync("./private_key", privateKey);