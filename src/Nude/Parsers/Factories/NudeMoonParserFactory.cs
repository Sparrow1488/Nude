using Nude.Authorization;
using Nude.Authorization.Cookies;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
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

    protected override string ParserName => "NudeMoon";
    
    protected override Task<UserCredentials> AuthorizeUserAsync(string login, string password)
    {
        return _authorization.AuthorizeAsync(login, password);
    }

    protected override async Task<INudeParser> CreateParserAsync(UserCredentials credentials)
    {
        var browser = await BrowserWrapper.CreateAsync(new BrowserOptions());
        var cookies = new DefaultAuthorizationCookies()
            .CreateFrom(credentials)
            .ToList();
        
        browser.AddCookies(cookies);
        
        return new NudeParser(browser);
    }
}