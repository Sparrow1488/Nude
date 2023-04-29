using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Exceptions.Client;
using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Images;
using Nude.API.Services.Collections.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Collections;

#region Rider annotations

// ReSharper disable file ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

public class ImageCollectionService : IImageCollectionService
{
    private readonly AppDbContext _context;

    public ImageCollectionService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<CollectionCreationResult> CreateAsync(
        string title, 
        string? description, 
        string contentKey, 
        IEnumerable<ImageEntry> images)
    {
        var exists = await _context.ImageCollections
            .FirstOrDefaultAsync(x => x.ContentKey == contentKey);
        
        if (exists != null)
        {
            var exception = new ContentKeyExistsException(
                "Similar collection already exists",
                exists.ContentKey,
                entityId: exists.Id.ToString(),
                entityType: nameof(ImageCollection)
            );
            return new CollectionCreationResult(exception);
        }
        
        var collection = new ImageCollection
        {
            Title = title,
            Description = description,
            ContentKey = contentKey,
            Images = images.Select(x => new CollectionImage { Entry = x }).ToList()
        };

        await _context.AddAsync(collection);
        await _context.SaveChangesAsync();

        return new CollectionCreationResult(collection);
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