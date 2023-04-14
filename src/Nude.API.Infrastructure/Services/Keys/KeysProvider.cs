namespace Nude.API.Infrastructure.Services.Keys;

public class KeysProvider
{
    public static byte[] GetPrivateKey()
    {
        return File.ReadAllBytes("./key");
    }
}