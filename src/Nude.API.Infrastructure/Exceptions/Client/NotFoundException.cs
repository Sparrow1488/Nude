using Microsoft.AspNetCore.Http;
using Nude.API.Infrastructure.Exceptions.Base;

// ReSharper disable VirtualMemberCallInConstructor

namespace Nude.API.Infrastructure.Exceptions.Client;

public class NotFoundException : ApiException, IStatusCodeException
{
    public NotFoundException(string message, string? entityId = null, string? entityType = null) : base(message)
    {
        Data.Add("entity_id", entityId ?? "unknown_id");
        Data.Add("entity_type", entityType ?? "unknown_type");
    }
    
    public int StatusCode => StatusCodes.Status404NotFound;
}