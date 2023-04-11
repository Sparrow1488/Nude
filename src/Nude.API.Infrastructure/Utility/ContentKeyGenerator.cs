using System.Security.Cryptography;
using System.Text;

namespace Nude.API.Infrastructure.Utility;

public static class ContentKeyGenerator
{
    public static string Generate(string entityType, string row)
    {
        return entityType + "-" + HashRow(row).ToUpper();
    }

    private static string HashRow(string row)
    {
        var builder = new StringBuilder();
        foreach (var b in GetHash(row))
            builder.Append(b.ToString("X2"));

        return builder.ToString();
    }
    
    private static byte[] GetHash(string inputString)
    {
        using HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
    }
}