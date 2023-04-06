namespace Nude.API.Models.Abstractions;

public interface IAuditableBase
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
}