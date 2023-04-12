using Nude.API.Models.Images;
using Nude.API.Services.Images.Results;

namespace Nude.API.Services.Images;

public interface IImagesService
{
    Task<ImageCreationResult> CreateAsync(
        string url,
        string contentKey,
        IEnumerable<string>? tags = null,
        string? author = null,
        string? externalSourceId = null,
        string? externalSourceUrl = null);

    Task<bool> ExistsAsync(string contentKey);
    Task<ImageEntry?> GetByIdAsync(int id);
    Task<ImageEntry?> GetByContentKeyAsync(string contentKey);
    Task<ICollection<ImageEntry>> FindByTagsAsync(IEnumerable<string> tags);
}