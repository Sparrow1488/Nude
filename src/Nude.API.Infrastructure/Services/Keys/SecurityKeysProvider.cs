namespace Nude.API.Infrastructure.Services.Keys;

public static class SecurityKeysProvider
{
    public static byte[] GetPrivateKey()
    {
        return File.ReadAllBytes("./key");
    }
}