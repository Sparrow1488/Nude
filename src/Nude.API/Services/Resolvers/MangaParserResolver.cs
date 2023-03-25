using Nude.API.Infrastructure.Exceptions;
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
        string login;
        string password;
        
        if (mangaUrl.Contains("nude-moon.org"))
        {
            (login, password) = GetCredentials("NudeMoon");
            return await _nudeMoonFactory.CreateAuthorizedAsync(login, password);
        }
        else if (mangaUrl.Contains(".hentaichan."))
        {
            (login, password) = GetCredentials("HentaiChan");
            return await _hentaiChanFactory.CreateAuthorizedAsync(login, password);
        }

        throw new BadRequestException("Request manga source not supported");
    }
    
    public Task<IMangaParser> ResolveByTypeAsync(SourceType sourceType)
    {
        throw new NotImplementedException();
    }

    private (string login, string password) GetCredentials(string parserName) =>
        (GetLogin(parserName), GetPassword(parserName));

    private string GetLogin(string parserName) =>
        _configuration.GetRequiredSection($"Credentials:{parserName}:Login").Value;
    
    private string GetPassword(string parserName) =>
        _configuration.GetRequiredSection($"Credentials:{parserName}:Password").Value;
}