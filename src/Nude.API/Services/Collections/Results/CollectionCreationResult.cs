using Nude.API.Infrastructure.Abstractions;
using Nude.API.Models.Collections;

namespace Nude.API.Services.Collections.Results;

public class CollectionCreationResult : ServiceResult<ImageCollection>
{
    public CollectionCreationResult(Exception exception) : base(exception) { }
    public CollectionCreationResult(ImageCollection result) : base(result) { }
}