using Nude.API.Models.Abstractions;
using Nude.API.Models.Formats;
using Nude.API.Models.Requests.States;

namespace Nude.API.Models.Requests;

public class FormattingContentRequest : IEntity
{
    public int Id { get; set; }
    public FormatType FormatType { get; set; }
    public FormattingStatus Status { get; set; }
    public FormattedContent? Result { get; set; } = null!;
}