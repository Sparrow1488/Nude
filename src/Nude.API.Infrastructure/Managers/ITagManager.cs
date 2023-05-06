using Nude.API.Models.Tags;
using Tag = Nude.API.Models.Tags.Tag;

namespace Nude.API.Infrastructure.Managers;

public interface ITagManager
{
    Task<Tag> AddAsync(string tag, TagType type);
    Task<ICollection<Tag>> AddRangeAsync(IEnumerable<string> tags, TagType type);
    Task<Tag[]> FindAsync(params string[] search);
    string NormalizeTag(string tag);
}