using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Mangas;

public class TghManga : IEntity
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string TghUrl { get; set; }
}