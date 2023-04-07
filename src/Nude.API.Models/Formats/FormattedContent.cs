using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Formats;

public abstract class FormattedContent : IEntity
{
    public int Id { get; set; }
    public virtual FormatType Type { get; set; }
}