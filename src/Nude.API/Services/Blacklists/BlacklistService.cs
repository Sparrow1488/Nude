using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Managers;
using Nude.API.Models.Blacklists;
using Nude.API.Models.Tags;
using Nude.API.Models.Users;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Blacklists;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

public class BlacklistService : IBlacklistService
{
    private readonly AppDbContext _context;
    private readonly ITagManager _tagManager;

    public BlacklistService(
        AppDbContext context,
        ITagManager tagManager)
    {
        _context = context;
        _tagManager = tagManager;
    }

    public async Task<Blacklist> CreateAsync(User user, IEnumerable<Tag> tags)
    {
        var blacklist = new Blacklist
        {
            User = user,
            Tags = tags.ToArray()
        };

        await _context.AddAsync(blacklist);
        await _context.SaveChangesAsync();

        return blacklist;
    }

    public Task<Blacklist?> GetAsync(User user)
    {
        return _context.Blacklists
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.User.Id == user.Id);
    }

    public async Task<Blacklist> GetDefaultAsync()
    {
        var tags = await _tagManager.FindAsync("яой", "трап", "фурри", "футанари");
        return new Blacklist
        {
            Tags = tags
        };
    }

    public async Task<Blacklist> AddTagsAsync(Blacklist blacklist, IEnumerable<Tag> tags)
    {
        if (blacklist.Tags is null)
        {
            await _context.Entry(blacklist).Collection(x => x.Tags).LoadAsync();
        }
        
        blacklist.Tags!.AddRange(tags);
        await _context.SaveChangesAsync();
        
        return blacklist;
    }

    public async Task<Blacklist> RemoveTagsAsync(Blacklist blacklist, IEnumerable<Tag> tags)
    {
        var remains = blacklist.Tags
            .Where(tag => !tags.Select(x => x.NormalizeValue).Contains(tag.NormalizeValue));
        
        blacklist.Tags = remains.ToList();
        _context.Update(blacklist);
        await _context.SaveChangesAsync();

        return blacklist;
    }

    public async Task RemoveAsync(Blacklist blacklist)
    {
        _context.Remove(blacklist);
        await _context.SaveChangesAsync();
    }
}