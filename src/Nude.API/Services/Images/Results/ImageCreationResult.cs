using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Services.Images.Results;

public class ImageCreationResult : ServiceResult<ImageEntry>
{
    public ImageCreationResult(Exception exception) : base(exception)
    {
    }

    public ImageCreationResult(ImageEntry result) : base(result)
    {
    }
}