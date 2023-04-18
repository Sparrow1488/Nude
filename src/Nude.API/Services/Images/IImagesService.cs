using Nude.API.Models.Images;
using Nude.API.Services.Images.Models;
using Nude.API.Services.Images.Results;

namespace Nude.API.Services.Images;

public interface IImagesService
{
    Task<ImageCreationResult> CreateAsync(ImageCreationModel model);
    Task<ImageRangeCreationResult> CreateRangeAsync(IEnumerable<ImageCreationModel> models);
    Task<bool> ExistsAsync(string contentKey);
    Task<ImageEntry?> GetByIdAsync(int id);
    Task<ImageEntry?> GetByContentKeyAsync(string contentKey);
    Task<ICollection<ImageEntry>> GetRandomAsync(int count = 1);
    Task<ICollection<ImageEntry>> FindAsync(IEnumerable<string> contentKeys);
    Task<ICollection<ImageEntry>> FindByTagsAsync(IEnumerable<string> tags);
}