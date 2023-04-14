using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Images;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Collections;

#region Rider annotations

// ReSharper disable file ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

public class ImageCollectionsService : IImageCollectionsService
{
    private readonly AppDbContext _context;

    public ImageCollectionsService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ImageCollection> CreateAsync(
        string title, 
        string? description, 
        string contentKey, 
        IEnumerable<ImageEntry> images)
    {
        var collection = new ImageCollection
        {
            Title = title,
            Description = description,
            ContentKey = contentKey,
            Images = images.Select(x => new CollectionImage { Entry = x }).ToList()
        };

        await _context.AddAsync(collection);
        await _context.SaveChangesAsync();

        return collection;
    }

    public Task<ImageCollection?> FindByContentKeyAsync(string contentKey)
    {
        return _context.ImageCollections
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.ContentKey == contentKey);
    }

    public async Task<ImageCollection?> AddFormatAsync(ImageCollection collection, Format format)
    {
        if (collection.Formats == null)
        {
            await _context.Entry(collection).Collection(nameof(collection.Formats)).LoadAsync();
            collection.Formats ??= new List<Format>();
        }
        
        collection.Formats.Add(format);
        await _context.SaveChangesAsync();

        return collection;
    }
}