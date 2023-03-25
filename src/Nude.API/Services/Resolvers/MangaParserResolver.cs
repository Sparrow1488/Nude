using Nude.API.Infrastructure.Exceptions;
using Nude.Constants;
using Nude.Models.Sources;
using Nude.Parsers.Abstractions;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;

namespace Nude.API.Services.Resolvers;

public class MangaParserResolver : IMangaParserResolver
{
    private readonly IConfiguration _configuration;
    private readonly IAuthorisedMangaParserFactory<IHentaiChanParser> _hentaiChanFactory;
    private readonly IAuthorisedMangaParserFactory<INudeParser> _nudeMoonFactory;

    public MangaParserResolver(
        IConfiguration configuration,
        IAuthorisedMangaParserFactory<IHentaiChanParser> hentaiChanFactory,
        IAuthorisedMangaParserFactory<INudeParser> nudeMoonFactory
    )
    {
        _configuration = configuration;
        _hentaiChanFactory = hentaiChanFactory;
        _nudeMoonFactory = nudeMoonFactory;
    }

    public async Task<IMangaParser> ResolveByUrlAsync(string mangaUrl)
    {
        if (mangaUrl.Contains("nude-moon.org"))
        {
            return await ResolveParserAsync(NudeMoonDefaults.Name, async (login, password) =>
                await _nudeMoonFactory.CreateAuthorizedAsync(login, password));
        }
        if (mangaUrl.Contains(".hentaichan."))
        {
            return await ResolveParserAsync(HentaiChanDefaults.Name, async (login, password) =>
                await _hentaiChanFactory.CreateAuthorizedAsync(login, password));
        }

        throw new BadRequestException("Request manga source not supported (parser not resolved)");
    }
    
    public Task<IMangaParser> ResolveByTypeAsync(SourceType sourceType)
    {
        throw new NotImplementedException();
    }

    private Task<IMangaParser> ResolveParserAsync(string name, Func<string, string, Task<IMangaParser>> resolver)
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