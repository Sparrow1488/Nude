using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Exceptions;
using Nude.Providers;

namespace Nude.API.Services.Manga;

public class NudeMoonService : IMangaService
{
    private readonly AppDbContext _context;
    private readonly INudeParser _nudeParser;

    public NudeMoonService(AppDbContext context, INudeParser nudeParser)
    {
        _context = context;
        _nudeParser = nudeParser;
    }
    
    public async Task<MangaResponse> GetByUrlAsync(string url)
    {
        var mangaId = _nudeParser.Helper.GetIdFromUrl(url);
        var manga = await _context.Mangas
                        .Include(x => x.Images)
                        .ThenInclude(x => x.Url)
                        .Include(x => x.Tags)
                        .FirstOrDefaultAsync(x => x.ExternalId == mangaId)
            ?? throw new NotFoundException("Manga not found", mangaId, "Manga");
        
        return new MangaResponse
        {
            Id = manga.Id,
            ExternalId = manga?.ExternalId ?? "",
            Title = manga.Title,
            Description = manga.Description,
            Images = manga.Images.Select(x => x.Url.Value).ToList(),
            Tags = manga.Tags.Select(x => x.Value).ToList(),
            Likes = manga.Likes
        };
    }
}