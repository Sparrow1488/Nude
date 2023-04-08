using Nude.API.Infrastructure.Services.Resolvers;
using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas;
using Nude.API.Services.Steal.Results;

namespace Nude.API.Services.Steal;

public class StealContentService : IStealContentService
{
    private readonly IMangaService _mangaService;
    private readonly IMangaParserResolver _mangaParserResolver;

    public StealContentService(
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
            EntryId = creationResult.Entry!.Id,
            EntryType = nameof(MangaEntry),
            Exception = exception
        };
    }
}