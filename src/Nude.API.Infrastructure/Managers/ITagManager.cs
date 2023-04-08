using Nude.API.Models.Tags;
using Tag = Nude.API.Models.Tags.Tag;

namespace Nude.API.Infrastructure.Managers;

public interface ITagManager
{
    Task<Tag> AddAsync(string tag, TagType type);
    Task<ICollection<Tag>> AddRangeAsync(IEnumerable<string> tags, TagType type);
    string NormalizeTag(string tag);
}