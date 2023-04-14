using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Collections;
using Nude.API.Models.Images;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Collections;

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
}