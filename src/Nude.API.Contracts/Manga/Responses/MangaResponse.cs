namespace Nude.API.Contracts.Manga.Responses;

public struct MangaResponse
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<string> Tags { get; set; }
    public List<string> Images { get; set; }
    public int Likes { get; set; }
    public string Author { get; set; }
}