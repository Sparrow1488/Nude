using Nude.Authorization;
using Nude.Authorization.Stores;
using Nude.Parsers.Abstractions;

namespace Nude.Parsers.Factories;

public abstract class AuthorizedParserFactory<TParser> : IAuthorisedMangaParserFactory<TParser>
where TParser : IMangaParser
{
    public AuthorizedParserFactory(ICredentialsSecureStore secureStore)
    {
        SecureStore = secureStore;
    }
    
    private ICredentialsSecureStore SecureStore { get; }
    protected abstract string ParserName { get; }
    
    public async Task<TParser> CreateAuthorizedAsync(string login, string password)
    {
        var storeKey = ParserName + ":" + login;

        var credentials = await SecureStore.GetAsync(storeKey) 
            ?? await AuthorizeUserAsync(login, password);

        await SecureStore.SaveAsync(storeKey, credentials);

        return await CreateParserAsync(credentials);
    }

    protected abstract Task<UserCredentials> AuthorizeUserAsync(string login, string password);
    protected abstract Task<TParser> CreateParserAsync(UserCredentials credentials);
}