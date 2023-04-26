using Microsoft.EntityFrameworkCore;
using Nude.API.Models.Tags;
using Nude.Data.Infrastructure.Contexts;
using Tag = Nude.API.Models.Tags.Tag;

namespace Nude.API.Infrastructure.Managers;

public class TagManager : ITagManager
{
    private readonly AppDbContext _context;

    public TagManager(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Tag> AddAsync(string tag, TagType type)
    {
        var collection = await AddRangeAsync(new[] { tag }, type);
        return collection.First();
    }

    public async Task<ICollection<Tag>> AddRangeAsync(IEnumerable<string> tags, TagType type)
    {
        var created = new List<Tag>();
        var normalizedTags = tags.Select(NormalizeTag).ToList();

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
                Type = type,
                NormalizeValue = NormalizeTag(x)
            })
            .ToList();

        await _context.AddRangeAsync(notExists);
        await _context.SaveChangesAsync();
        
        created.AddRange(notExists);
        created.AddRange(existsTags);
        return created;
    }

    public string NormalizeTag(string tag)
    {
        var normalize = tag.Where(s => char.IsDigit(s) || char.IsLetter(s));
        return string.Join("", normalize).ToUpper();
    }
}