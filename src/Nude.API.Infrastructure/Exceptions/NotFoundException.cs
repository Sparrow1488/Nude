using Microsoft.AspNetCore.Http;

namespace Nude.API.Infrastructure.Exceptions;

public sealed class NotFoundException : ApiException, IStatusCodeException
{
    public NotFoundException(string message, string? entityId, string? entityType) : base(message)
    {
        Data.Add("entity_id", entityId ?? "unknown_id");
        Data.Add("entity_type", entityId ?? "unknown_type");
    }
    
    public int StatusCode => StatusCodes.Status404NotFound;
}