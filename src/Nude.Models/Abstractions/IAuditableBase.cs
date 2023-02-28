namespace Nude.Models.Abstractions;

public interface IAuditableBase
{
    DateTimeOffset CreatedAt { get; set; }
    DateTimeOffset UpdatedAt { get; set; }
}