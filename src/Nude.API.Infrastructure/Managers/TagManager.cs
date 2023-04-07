using Microsoft.EntityFrameworkCore;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models.Tags;

namespace Nude.API.Infrastructure.Managers;

public class TagManager : ITagManager
{
    private readonly AppDbContext _context;

    public TagManager(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Tag> AddAsync(string tag)
    {
        var created = await AddRangeAsync(new[] {tag});
        return created.First();
    }

    public async Task<ICollection<Tag>> AddRangeAsync(IEnumerable<string> tags)
    {
        var created = new List<Tag>();
        const string notNormalizedTag = "not_normalized";
        var normalizedTags = tags.Select(x => NormalizeTag(x) ?? notNormalizedTag).ToList();

        var existsTags = await _context.Tags
            .Where(x => normalizedTags.Contains(x.NormalizeValue))
            .ToListAsync();

        if (existsTags.Count == normalizedTags.Count)
        {
            return existsTags;
        }

        var notExists = tags
            .Where(tag => existsTags.All(x => x.NormalizeValue != NormalizeTag(tag)))
            .Select(x => new Tag
            {
                Value = x,
                NormalizeValue = NormalizeTag(x) ?? notNormalizedTag
            })
            .ToList();

        await _context.AddRangeAsync(notExists);
        await _context.SaveChangesAsync();
        
        created.AddRange(notExists);
        created.AddRange(existsTags);
        return created;
    }

    public string? NormalizeTag(string tag)
    {
        // TODO: read from configuration file
        // external source tag -> normalized tag
        return tag;
    }
}