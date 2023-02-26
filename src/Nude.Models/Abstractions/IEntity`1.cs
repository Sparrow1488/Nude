namespace Nude.Models.Abstractions;

public interface IEntity<TId>
{
    TId Id { get; set; }
}