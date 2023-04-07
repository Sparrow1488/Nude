using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Mangas.Meta;

public class MangaMeta : IEntity
{
    public int Id { get; set; }
    public string? SourceUrl { get; set; }
}