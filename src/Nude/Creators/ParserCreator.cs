using Nude.Authorization;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Constants;
using Nude.Navigation.Browser;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;

namespace Nude.Creators;

public class ParserCreator : IParserCreator
{
    private readonly ICredentialsSecureStore _store;

    public ParserCreator()
    {
        _store = new CredentialsSecureStore();
    }
    
    public async Task<INudeParser> CreateNudeMoonAsync(
        string login, 
        string password, 
        bool storeCredentials = false)
    {
        var authHandler = new NudeMoonAuthHandler();
        var credentials = await GetCredentialsAsync(
            () => authHandler.AuthorizeAsync(login, password),
            key: $"[{NudeMoonDefaults.Name}]{login}",
            storeCredentials
        );
        var browser = await BrowserWrapper.CreateAsync(BrowserOptions.Default);
        
        return new NudeParser(credentials, browser);
    }
    
    public async Task<IHentaiChanParser> CreateHentaiChanAsync(
        string login, 
        string password, 
        bool storeCredentials = false)
    {
        var authHandler = new HentaiChanAuthHandler();
        var credentials = await GetCredentialsAsync(
            () => authHandler.AuthorizeAsync(login, password),
            key: $"[{HentaiChanDefaults.Name}]{login}",
            storeCredentials
        );
        
        return new HentaiChanParser(credentials);
    }

    private async Task<UserCredentials?> GetCredentialsAsync(
        Func<Task<UserCredentials>> auth,
        string? key,
        bool useSecureStore)
    {
        UserCredentials? credentials = null;
        if (!string.IsNullOrWhiteSpace(key) && useSecureStore)
        {
            credentials = await _store.GetAsync(key);
        }

        if (credentials is not null)
        {
            return credentials;
        }

        credentials ??= await auth.Invoke();

        if (!string.IsNullOrWhiteSpace(key) && useSecureStore)
        {
            await _store.SaveAsync(key, credentials);
        }

        return credentials;
    }
}