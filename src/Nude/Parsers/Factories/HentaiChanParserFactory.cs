using Nude.Authorization;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Constants;
using Nude.Extensions;
using Nude.Navigation.Http;
using Nude.Parsers.HentaiChan;

namespace Nude.Parsers.Factories;

public class HentaiChanParserFactory : AuthorizedParserFactory<IHentaiChanParser>
{
    private readonly IAuthorizationHandler<IHentaiChanParser> _authorization;

    public HentaiChanParserFactory(
        ICredentialsSecureStore secureStore,
        IAuthorizationHandler<IHentaiChanParser> authorization
    ) : base(secureStore)
    {
        _authorization = authorization;
    }

    protected override string ParserName => HentaiChanDefaults.Name;

    protected override Task<UserCredentials> AuthorizeUserAsync(string login, string password)
    {
        return _authorization.AuthorizeAsync(login, password);
    }

    protected override Task<IHentaiChanParser> CreateParserAsync(UserCredentials credentials)
    {
        var navigator = new HttpClientNavigator();
        navigator.AddCookies(credentials.ToCookies());

        return Task.FromResult((IHentaiChanParser) new HentaiChanParser(navigator));
    }
}