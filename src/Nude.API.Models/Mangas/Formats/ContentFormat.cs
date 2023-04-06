using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Mangas.Formats;

public abstract class ContentFormat : IEntity
{
    public int Id { get; set; }
    public virtual FormatType Type { get; set; }
}