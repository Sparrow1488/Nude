using Nude.API.Models.Abstractions;

namespace Nude.API.Models.Mangas.Meta;

public abstract class MangaMeta : IEntity
{
    public int Id { get; set; }
    public virtual bool IsExternal { get; set; }
}