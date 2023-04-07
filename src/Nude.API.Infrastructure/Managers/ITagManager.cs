using Nude.Models.Tags;

namespace Nude.API.Infrastructure.Managers;

public interface ITagManager
{
    Task<Tag> AddAsync(string tag);
    Task<ICollection<Tag>> AddRangeAsync(IEnumerable<string> tags);
    string? NormalizeTag(string tag);
}