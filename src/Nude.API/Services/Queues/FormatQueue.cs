using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Constants;
using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Services.Collections;
using Nude.API.Services.Mangas;
using Nude.API.Services.Queues.Models;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Services.Queues;

#region Rider annotations

// ReSharper disable file InconsistentNaming

#endregion

public class FormatQueue : IFormatQueue
{
    private readonly AppDbContext _context;
    private readonly IMangaService _mangaService;
    private readonly IImageCollectionService _collectionService;

    public FormatQueue(
        AppDbContext context,
        IMangaService mangaService,
        IImageCollectionService collectionService)
    {
        _context = context;
        _mangaService = mangaService;
        _collectionService = collectionService;
    }
    
    public async Task<FormatModelAgent?> DequeueAsync()
    {
        var manga = await GetMangaAsync();
        if (manga != null)
        {
            return new FormatModelAgent
            {
                Title = manga.Title,
                Description = "With Love by Nude",
                ContentKey = manga.ContentKey,
                Images = manga.Images.Select(x => x.Url.Value).ToList(),
                Type = GetMissingFormatType(manga.Formats),
                ReleaseFunc = format => _mangaService.AddFormatAsync(manga, format)
            };
        }

        var collection = await GetImageCollectionAsync();
        if (collection != null)
        {
            return new FormatModelAgent
            {
                Title = collection.Title,
                Description = collection.Description,
                ContentKey = collection.ContentKey,
                Images = collection.Images.Select(x => x.Entry.Url).ToList(),
                Type = GetMissingFormatType(collection.Formats),
                ReleaseFunc = format => _collectionService.AddFormatAsync(collection, format)
            };
        }

        return null;
    }

    private async Task<MangaEntry?> GetMangaAsync()
    {
        var mangaKey = await _context.Mangas
            .Where(x => !x.Formats.Any(x => x.Type == FormatType.Telegraph) && x.Images.Count <= ContentLimits.MaxFormatImagesCount)
            .Select(x => x.ContentKey)
            .FirstOrDefaultAsync();

        return mangaKey != null
            ? await _mangaService.FindByContentKeyAsync(mangaKey)
            : null;
    }
    
    private async Task<ImageCollection?> GetImageCollectionAsync()
    {
        var collectionKey = await _context.ImageCollections
            .Where(x => !x.Formats.Any(x => x.Type == FormatType.Telegraph) && x.Images.Count <= ContentLimits.MaxFormatImagesCount)
            .Select(x => x.ContentKey)
            .FirstOrDefaultAsync();
        
        return collectionKey != null
            ? await _collectionService.FindByContentKeyAsync(collectionKey)
            : null;
    }

    private static FormatType GetMissingFormatType(IEnumerable<Format> formats)
    {
        return FormatType.Telegraph;
    }
}