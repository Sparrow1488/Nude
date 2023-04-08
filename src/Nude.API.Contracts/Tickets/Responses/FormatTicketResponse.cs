using Nude.API.Contracts.Formats.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Tickets.States;

namespace Nude.API.Contracts.Tickets.Responses;

public struct FormatTicketResponse
{
    public int Id { get; set; }
    public FormatType FormatType { get; set; }
    public FormattingStatus Status { get; set; }
    public FormattedContentResponse? Result { get; set; }
    public FormatTicketContextResponse Context { get; set; }
}