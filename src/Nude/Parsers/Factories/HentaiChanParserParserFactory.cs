using Nude.Authorization;
using Nude.Authorization.Handlers;
using Nude.Authorization.Stores;
using Nude.Parsers.HentaiChan;

namespace Nude.Parsers.Factories;

public class HentaiChanParserParserFactory : AuthorizedParserFactory<IHentaiChanParser>
{
    private readonly IAuthorizationHandler<IHentaiChanParser> _authorization;

    public HentaiChanParserParserFactory(
        ICredentialsSecureStore secureStore,
        IAuthorizationHandler<IHentaiChanParser> authorization
    ) : base(secureStore)
    {
        _authorization = authorization;
    }

    protected override string ParserName => "HentaiChan";

    protected override Task<UserCredentials> AuthorizeUserAsync(string login, string password)
    {
        return _authorization.AuthorizeAsync(login, password);
    }

    protected override Task<IHentaiChanParser> CreateParserAsync(UserCredentials credentials)
    {
        return HentaiChanParser.CreateAsync(credentials);
    }
}