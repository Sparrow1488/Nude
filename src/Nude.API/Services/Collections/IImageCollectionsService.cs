using Nude.API.Models.Collections;
using Nude.API.Models.Formats;
using Nude.API.Models.Images;

namespace Nude.API.Services.Collections;

public interface IImageCollectionsService
{
    Task<ImageCollection> CreateAsync(
        string title,
        string? description,
        string contentKey,
        IEnumerable<ImageEntry> images
    );

    Task<ImageCollection?> FindByContentKeyAsync(string contentKey);
    Task<ImageCollection?> AddFormatAsync(ImageCollection collection, Format format);
}