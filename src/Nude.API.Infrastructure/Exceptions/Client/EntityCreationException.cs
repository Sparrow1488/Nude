namespace Nude.API.Infrastructure.Exceptions.Client;

public sealed class EntityCreationException : BadRequestException
{
    public EntityCreationException(string message, string? entityType) : base(message)
    {
        Data.Add("entity_type", entityType ?? "unknown_type");
    }
}