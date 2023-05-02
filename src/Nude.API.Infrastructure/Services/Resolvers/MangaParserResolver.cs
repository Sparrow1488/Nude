using Microsoft.Extensions.Configuration;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Infrastructure.Utility;
using Nude.Constants;
using Nude.Creators;
using Nude.Parsers;

namespace Nude.API.Infrastructure.Services.Resolvers;

public class MangaParserResolver : IMangaParserResolver
{
    private readonly IConfiguration _configuration;
    private readonly IParserCreator _creator;

    public MangaParserResolver(
        IConfiguration configuration,
        IParserCreator creator)
    {
        _configuration = configuration;
        _creator = creator;
    }

    public bool CanBeResolved(string mangaUrl)
    {
        return ContentAware.IsSealingAvailable(mangaUrl);
    }

    public async Task<IMangaParser> ResolveByUrlAsync(string mangaUrl)
    {
        if (mangaUrl.Contains("nude-moon.org"))
        {
            return await ResolveParserAsync(NudeMoonDefaults.Name, async (login, password) =>
                await _creator.CreateNudeMoonAsync(login, password, true));
        }
        if (mangaUrl.Contains(".hentaichan."))
        {
            return await ResolveParserAsync(HentaiChanDefaults.Name, async (login, password) =>
                await _creator.CreateHentaiChanAsync(login, password, true));
        }

        throw new BadRequestException("Request manga source not supported (parser not resolved)");
    }
    
    private Task<IMangaParser> ResolveParserAsync(
        string name, 
        Func<string, string, Task<IMangaParser>> resolver)
    {
        var (login, password) = GetCredentials(name);
        return resolver.Invoke(login, password);
    }

    private (string login, string password) GetCredentials(string parserName) =>
        (GetLogin(parserName), GetPassword(parserName));

    private string GetLogin(string parserName) =>
        _configuration.GetRequiredSection($"Credentials:{parserName}:Login").Value;
    
    private string GetPassword(string parserName) =>
        _configuration.GetRequiredSection($"Credentials:{parserName}:Password").Value;
}