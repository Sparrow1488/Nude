namespace Nude.Models;

public class Manga
{
    public string Title { get; set; } = "Untitled";
    public string Description { get; set; } = "";
    public List<string> Tags { get; set; } = new();
    public List<string> Images { get; set; } = new();
    public int Likes { get; set; }
}