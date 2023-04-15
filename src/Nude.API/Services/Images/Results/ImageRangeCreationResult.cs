using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Services.Images.Results;

public class ImageRangeCreationResult : IServiceResult<IEnumerable<ImageEntry>>
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
    public IEnumerable<ImageEntry>? Result { get; set; }
}