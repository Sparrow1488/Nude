using Nude.API.Infrastructure.Services.Resolvers;
using Nude.API.Infrastructure.Utility;
using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas;
using Nude.API.Services.Stealers.Results;

namespace Nude.API.Services.Stealers;

public class ContentStealerService : IContentStealerService
{
    private readonly IMangaService _mangaService;
    private readonly IMangaParserResolver _mangaParserResolver;

    public ContentStealerService(
        IMangaService mangaService,
        IMangaParserResolver mangaParserResolver)
    {
        _mangaService = mangaService;
        _mangaParserResolver = mangaParserResolver;
    }
    
    public async Task<ContentStealingResult> StealContentAsync(string sourceUrl)
    {
        if (_mangaParserResolver.CanBeResolved(sourceUrl))
        {
            return await StealMangaAsync(sourceUrl);
        }

        throw new NotImplementedException();
    }

    private async Task<ContentStealingResult> StealMangaAsync(string mangaUrl)
    {
        var parser = await _mangaParserResolver.ResolveByUrlAsync(mangaUrl);
        var result = await parser.GetByUrlAsync(mangaUrl);

        var creationResult = await _mangaService.CreateAsync(
            result.Title,
            result.Description,
            ContentKeyGenerator.Generate(nameof(MangaEntry), mangaUrl),
            result.Images,
            tags: result.Tags,
            author: result.Author,
            externalSourceId: result.ExternalId,
            externalSourceUrl: mangaUrl
        );

        Exception? exception = null;
        if (!creationResult.IsSuccess)
        {
            exception = new Exception($"Something went wrong on stealing manga from {mangaUrl}");
        }

        return new ContentStealingResult
        {
            IsSuccess = exception == null,
            ContentKey = creationResult?.Entry?.ContentKey,
            Exception = exception
        };
    }
}