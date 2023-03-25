using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Managers;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Urls;

namespace Nude.API.Data.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly AppDbContext _context;
    private readonly ITagManager _tagManager;

    public MangaRepository(AppDbContext context, ITagManager tagManager)
    {
        _context = context;
        _tagManager = tagManager;
    }
    
    public async Task<Manga> AddAsync(Models.Manga manga, SourceType sourceType)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Name == manga.Author) 
            ?? new Author { Name = manga.Author };
        
        var source = await _context.Sources.FirstOrDefaultAsync(x => x.Type == sourceType)
            ?? new Source { Type = sourceType };

        var mangaTags = await _tagManager.AddRangeAsync(manga.Tags);
        
        var newManga = new Manga
        {
            ExternalId = manga.ExternalId,
            Title = manga.Title,
            Description = manga.Description,
            Tags = mangaTags,
            Author = author,
            Images = manga.Images.Select(x => new MangaImage { Url = new Url { Value = x }}).ToList(),
            Likes = manga.Likes,
            Source = source,
            OriginUrl = new Url { Value = manga.OriginUrl }
        };

        await AddAsync(newManga);
        await SaveAsync();
        
        return newManga;
    }

    public async Task<Manga> AddAsync(Manga manga)
    {
        await _context.Mangas.AddAsync(manga);
        return manga;
    }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }
}