using Nude.Models.Sources;
using Nude.Parsers.Abstractions;
using Nude.Parsers.Factories;
using Nude.Parsers.HentaiChan;
using Nude.Parsers.NudeMoon;

namespace Nude.API.Services.Resolvers;

public class MangaParserResolver : IMangaParserResolver
{
    private readonly AuthorizedParserFactory<IHentaiChanParser> _hentaiChanFactory;

    public MangaParserResolver(AuthorizedParserFactory<IHentaiChanParser> hentaiChanFactory)
    {
        _hentaiChanFactory = hentaiChanFactory;
    }

    public Task<IMangaParser> ResolveByUrlAsync(string mangaUrl)
    {
        if (mangaUrl.Contains("nude-moon.org"))
        {
            // return NudeParser.CreateAsync()
        }
        throw new NotImplementedException();
    }

    public Task<IMangaParser> ResolveByUrlAsync(SourceType sourceType)
    {
        throw new NotImplementedException();
    }
}