using Nude.Authorization;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Constants;
using Nude.Extensions;
using Nude.Navigation.Browser;
using Nude.Parsers.NudeMoon;

namespace Nude.Parsers.Factories;

public class NudeMoonParserFactory : AuthorizedParserFactory<INudeParser>
{
    private readonly IAuthorizationHandler<INudeParser> _authorization;

    public NudeMoonParserFactory(
        ICredentialsSecureStore secureStore,
        IAuthorizationHandler<INudeParser> authorization
    ) : base(secureStore)
    {
        _authorization = authorization;
    }

    protected override string ParserName => NudeMoonDefaults.Name;
    
    protected override Task<UserCredentials> AuthorizeUserAsync(string login, string password)
    {
        return _authorization.AuthorizeAsync(login, password);
    }

    protected override async Task<INudeParser> CreateParserAsync(UserCredentials credentials)
    {
        var browser = await BrowserWrapper.CreateAsync(new BrowserOptions());
        browser.AddCookies(credentials.ToCookies());
        
        return new NudeParser(browser);
    }
}