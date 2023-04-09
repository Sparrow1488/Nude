using Nude.API.Models.Formats;

namespace Nude.API.Contracts.Tickets.Requests;

public struct FormatTicketRequest
{
    public string EntryId { get; set; }
    public string EntryType { get; set; }
    public FormatType FormatType { get; set; }
    public string? CallbackUrl { get; set; }
}