using Nude.API.Contracts.Manga.Responses;
using Nude.Providers;

namespace Nude.API.Services.Manga;

public class NudeMoonService : IMangaService
{
    private readonly INudeParser _nudeParser;

    public NudeMoonService(INudeParser nudeParser)
    {
        _nudeParser = nudeParser;
    }
    
    public async Task<MangaResponse> GetByUrlAsync(string url)
    {
        var manga = await _nudeParser.GetByUrlAsync(url);
        return new MangaResponse
        {
            Id = Random.Shared.Next(100, 1000),
            Title = manga.Title,
            Description = manga.Description,
            Images = manga.Images,
            Tags = manga.Tags,
            Likes = manga.Likes
        };
    }
}