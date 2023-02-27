using Nude.Models.Abstractions;

namespace Nude.Models.Mangas;

public class TghManga : IEntity
{
    public int Id { get; set; }
    public string ExternalId { get; set; }
    public string TghUrl { get; set; }
}