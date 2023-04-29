using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Services.Images.Results;

public class ImageRangeCreationResult : ServiceResult<IEnumerable<ImageEntry>>
{
    public ImageRangeCreationResult(Exception exception) : base(exception)
    {
    }

    public ImageRangeCreationResult(IEnumerable<ImageEntry> result) : base(result)
    {
    }
}