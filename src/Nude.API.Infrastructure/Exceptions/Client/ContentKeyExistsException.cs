namespace Nude.API.Infrastructure.Exceptions.Client;

public sealed class ContentKeyExistsException : BadRequestException
{
    public ContentKeyExistsException(
        string? message,
        string contentKey, 
        string? entityId = null,
        string? entityType = null) : base(message)
    {
        Data.Add("content_key", contentKey);
        Data.Add("entity_id", entityId ?? "unknown_entity_id");
        Data.Add("entity_type", entityType ?? "unknown_entity_type");
    }
}