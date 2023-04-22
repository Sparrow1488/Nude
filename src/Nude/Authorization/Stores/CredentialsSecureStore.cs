using Newtonsoft.Json;
using Nude.Authorization.Stores.Models;
using Nude.Extensions;
using Nude.Helpers;

namespace Nude.Authorization.Stores;

public class CredentialsSecureStore : ICredentialsSecureStore
{
    private const string CredentialsEntryFileName = "file.json";
    
    private readonly string _credentialsStorePath = NudeEnvironment.GetNudeStoreCredentialsPath();

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
            var restoredCredentialsJson = await GetStoreEntryValue(key);
            var storeCredentials = JsonConvert.DeserializeObject<StoreUserCredentials>(restoredCredentialsJson)!;

            return storeCredentials.RevertCredentials();
        }

        return null;
    }

    private void ConfigureStore() =>
        Directory.CreateDirectory(_credentialsStorePath);
    
    private Task<bool> ExistsAsync(string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        
        var storeEntry = GetStoreEntryPath(key);
        return Task.FromResult(Directory.Exists(storeEntry));
    }

    private StreamWriter CreateEntryStore(string key)
    {
        var storePath = GetStoreEntryPath(key);
        Directory.CreateDirectory(storePath);

        return File.CreateText(storePath + CredentialsEntryFileName);
    }
    
    private async Task<string> GetStoreEntryValue(string key)
    {
        return await File.ReadAllTextAsync(GetStoreEntryPath(key) + CredentialsEntryFileName);
    }

    private string GetStoreEntryPath(string key)
    {
        return _credentialsStorePath + "/" + key + "/";
    }
    
    private static StoreUserCredentials CreateStoreCredentials(UserCredentials credentials)
    {
        return new StoreUserCredentials
        {
            Schema = credentials.Schema,
            Claims = credentials.Claims.ToStoreEntries()
        };
    }
}