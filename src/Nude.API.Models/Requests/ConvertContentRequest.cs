using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Requests.States;

namespace Nude.API.Models.Requests;

public class ConvertContentRequest : IEntity
{
    public int Id { get; set; }
    public FormatType FormatType { get; set; }
    public ConvertStatus Status { get; set; }
    public FormattedContent? Result { get; set; } = null!;
}