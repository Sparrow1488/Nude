using System.Security.Claims;
using Newtonsoft.Json;
using Nude.Authorization.Stores.Models;

namespace Nude.Authorization.Stores;

public class CredentialsSecureStore : ICredentialsSecureStore
{
    private const string CredentialsStorePath = "/store/credentials";
    private const string CredentialsEntryFileName = "file.json";

    public Task<bool> ExistsAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        
        var storeEntry = GetEntryStorePath(key);
        return Task.FromResult(Directory.Exists(storeEntry));
    }

    public async Task SaveAsync(string key, UserCredentials credentials)
    {
        ArgumentNullException.ThrowIfNull(key);
        
        ConfigureStore();

        var storeEntry = CreateStoreCredentials(credentials);
        var storeEntryJson = JsonConvert.SerializeObject(storeEntry, Formatting.Indented);

        await using var writer = CreateEntryStore(key);
        await writer.WriteAsync(storeEntryJson);
    }

    public async Task<UserCredentials?> GetAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        
        ConfigureStore();

        if (await ExistsAsync(key))
        {
            var restoredCredentialsJson = await GetEntryStore(key);
            var storeCredentials = JsonConvert.DeserializeObject<StoreUserCredentials>(restoredCredentialsJson)!;

            return RestoreUserCredentials(storeCredentials);
        }

        return null;
    }

    private static void ConfigureStore() =>
        Directory.CreateDirectory("." + CredentialsStorePath);

    private static StreamWriter CreateEntryStore(string key)
    {
        var storePath = GetEntryStorePath(key);
        Directory.CreateDirectory(storePath);

        return File.CreateText(storePath + CredentialsEntryFileName);
    }
    
    private static async Task<string> GetEntryStore(string key)
    {
        return await File.ReadAllTextAsync(GetEntryStorePath(key) + CredentialsEntryFileName);
    }

    private static string GetEntryStorePath(string key)
    {
        return "." + CredentialsStorePath + "/" + key + "/";
    }
    
    private static StoreUserCredentials CreateStoreCredentials(UserCredentials credentials)
    {
        return new StoreUserCredentials
        {
            Schema = credentials.Schema,
            Claims = credentials.Claims.Select(x => new StoreClaim(x.Type, x.Value)).ToList()
        };
    }

    private static UserCredentials RestoreUserCredentials(StoreUserCredentials store)
    {
        var claims = store.Claims?.Select(x => new Claim(x.Type, x.Value)).ToList()
            ?? new List<Claim>();
        return new UserCredentials(claims, store.Schema);
    }
}