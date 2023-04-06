namespace Nude.API.Models.Abstractions;

public interface IEntity<TId>
{
    TId Id { get; set; }
}