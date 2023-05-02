using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Images;
using Nude.API.Services.Collections.Results;

namespace Nude.API.Services.Collections;

public interface IImageCollectionService
{
    Task<CollectionCreationResult> CreateAsync(
        string title,
        string? description,
        string contentKey,
        IEnumerable<ImageEntry> images
    );

    Task<ImageCollection?> FindByContentKeyAsync(string contentKey);
    Task<ImageCollection?> AddFormatAsync(ImageCollection collection, Format format);
}