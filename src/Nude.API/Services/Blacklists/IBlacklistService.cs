using Nude.API.Models.Blacklists;
using Nude.API.Models.Tags;
using Nude.API.Models.Users;

namespace Nude.API.Services.Blacklists;

public interface IBlacklistService
{
    Task<Blacklist> CreateAsync(User user, IEnumerable<Tag> tags);
    Task<Blacklist?> GetAsync(User user);
    Task<Blacklist> GetDefaultAsync();
    Task<Blacklist> AddTagsAsync(Blacklist blacklist, IEnumerable<Tag> tags);
    Task<Blacklist> RemoveTagsAsync(Blacklist blacklist, IEnumerable<Tag> tags);
    Task RemoveAsync(Blacklist blacklist);
}