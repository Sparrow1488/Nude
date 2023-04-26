namespace Nude.API.Contracts.Collections.Requests;

public struct ImageCollectionCreateRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public string[] Images { get; set; }
}