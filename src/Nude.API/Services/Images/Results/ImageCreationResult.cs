using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Services.Images.Results;

public class ImageCreationResult : IServiceResult<ImageEntry>
{
    public bool IsSuccess { get; set; }
    public Exception? Exception { get; set; }
    public ImageEntry? Result { get; set; }
}