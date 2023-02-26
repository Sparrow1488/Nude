using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Models.Urls;

namespace Nude.API.Data.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly AppDbContext _context;

    public MangaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Manga> AddAsync(
        string externalId,
        string title, 
        string desc, 
        IEnumerable<string> tags, 
        IEnumerable<string> images, 
        int likes, 
        string authorName,
        SourceType sourceType, 
        string originUrl)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(x => x.Name == authorName) 
                     ?? new Author { Name = authorName };
        
        var source = await _context.Sources.FirstOrDefaultAsync(x => x.Type == sourceType)
                     ?? new Source { Type = sourceType };
        
        var manga = new Manga
        {
            ExternalId = externalId,
            Title = title,
            Description = desc,
            Tags = tags.Select(x => new Tag { Value = x, NormalizeValue = "" }).ToList(), // TagsManager
            Author = author,
            Images = images.Select(x => new MangaImage { Url = new Url { Value = x }}).ToList(),
            Likes = likes,
            Source = source,
            OriginUrl = new Url { Value = originUrl }
        };
        await AddAsync(manga);
        return manga;
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