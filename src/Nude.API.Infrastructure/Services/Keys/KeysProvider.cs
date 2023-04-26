namespace Nude.API.Infrastructure.Services.Keys;

public static class KeysProvider
{
    public static byte[] GetPrivateKey()
    {
        return File.ReadAllBytes("./key");
    }
}