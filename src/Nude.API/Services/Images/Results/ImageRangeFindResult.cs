using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Images;

namespace Nude.API.Services.Images.Results;

public class ImageRangeFindResult : ServiceResult<IEnumerable<ImageEntry>>
{
    public ImageRangeFindResult(Exception exception) : base(exception)
    {
    }

    public ImageRangeFindResult(IEnumerable<ImageEntry> result) : base(result)
    {
    }
}