namespace Nude.Models;

public class Manga
{
    public string ExternalId { get; set; }
    public string Title { get; set; } = "Untitled";
    public string Description { get; set; } = "";
    public string OriginUrl { get; set; }
    public List<string> Tags { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public int Likes { get; set; }
    public string Author { get; set; }
}