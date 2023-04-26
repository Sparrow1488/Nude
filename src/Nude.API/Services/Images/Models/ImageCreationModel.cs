using Nude.API.Models.Users;

namespace Nude.API.Services.Images.Models;

public class ImageCreationModel
{
    public string Url { get; set; }
    public string ContentKey { get; set; }
    public IEnumerable<string>? Tags { get; set; }
    public User? Owner { get; set; }
    public string? Author { get; set; }
    public string? ExternalSourceId { get; set; }
    public string? ExternalSourceUrl { get; set; }
}