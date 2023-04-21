using Microsoft.Extensions.Configuration;
using Nude.Authorization.Handlers;
using Nude.Navigation.Browser;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;

namespace Nude.API.Infrastructure.Services.Creators;

public class ParserCreator
{
    public async Task<INudeParser> CreateNudeMoonAsync(string login, string password)
    {
        var authHandler = new NudeMoonAuthHandler();
        var credentials = await authHandler.AuthorizeAsync(login, password);
        var browser = await BrowserWrapper.CreateAsync(BrowserOptions.Default);
        return new NudeParser(credentials, browser);
    }
    
    public async Task<IHentaiChanParser> CreateHentaiChanAsync(string login, string password)
    {
        var authHandler = new HentaiChanAuthHandler();
        var credentials = await authHandler.AuthorizeAsync(login, password);
        return new HentaiChanParser(credentials);
    }
}