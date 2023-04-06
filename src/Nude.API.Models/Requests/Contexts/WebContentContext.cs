namespace Nude.API.Models.Requests.Contexts;

public class WebContentContext : ReceiveContext
{
    public string ContentUrl { get; set; }
    public string? ContentId { get; set; }
}