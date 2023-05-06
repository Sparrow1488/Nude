using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Managers;
using Nude.API.Infrastructure.Services.Randomizers;
using Nude.API.Models.Blacklists;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Urls;
using Nude.API.Services.Mangas.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

#region Rider annotations

// ReSharper disable file ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable file PossibleMultipleEnumeration
// ReSharper disable file InvertIf

#endregion

namespace Nude.API.Services.Mangas;

public class MangaService : IMangaService
{
    private readonly AppDbContext _context;
    private readonly IRandomizer _randomizer;
    private readonly ITagManager _tagManager;

    public MangaService(
        AppDbContext context,
        IRandomizer randomizer,
        ITagManager tagManager)
    {
        _context = context;
        _randomizer = randomizer;
        _tagManager = tagManager;
    }

    public async Task<MangaCreationResult> CreateAsync(
        string title, 
        string description, 
        string contentKey,
        IEnumerable<string> images, 
        IEnumerable<string>? tags = null, 
        string? author = null,
        string? externalSourceId = null, 
        string? externalSourceUrl = null)
    {
        var mangaImages = images.Select(img => new MangaImage
        {
            Url = new Url { Value = img }
        }).ToList();
        
        var entry = new MangaEntry
        {
            Title = title,
            Description = description,
            ContentKey = contentKey,
            Images = mangaImages
        };

        if (!string.IsNullOrWhiteSpace(externalSourceUrl))
        {
            entry.ExternalMeta = new ExternalMeta
            {
                SourceId = externalSourceId,
                SourceUrl = externalSourceUrl
            };
        }

        await _context.AddAsync(entry);
        await _context.SaveChangesAsync();

        var combinedTags = await CombineTagsAsync(author, tags);
        if (combinedTags.Any())
        {
            await AddTagsAsync(entry, combinedTags);
        }
        
        return new MangaCreationResult(entry);
    }

    private async Task<IEnumerable<Tag>> CombineTagsAsync(string? author, IEnumerable<string>? tags)
    {
        var tagsList = new List<Tag>();
        
        if (tags != null && tags.Any())
        {
            var tagsCollection = await _tagManager.AddRangeAsync(tags, TagType.Trivia);
            tagsList.AddRange(tagsCollection);
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            var authorTag = await _tagManager.AddAsync(author, TagType.Author);
            tagsList.Add(authorTag);
        }

        return tagsList;
    }

    private async Task AddTagsAsync(MangaEntry manga, IEnumerable<Tag> tags)
    {
        if (manga.Tags is null)
        {
            await _context.Entry(manga).Collection(nameof(manga.Tags)).LoadAsync();
            manga.Tags ??= new List<Tag>();
        }
        manga.Tags.AddRange(tags);
        await _context.SaveChangesAsync();
    }

    public Task<MangaEntry?> GetByIdAsync(int id)
    {
        return _context.Mangas
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<int[]> GetAllAsync(Blacklist? blacklist = null)
    {
        var blacklisted = GetBlacklistedTags(blacklist);
        
        return _context.Mangas
            .Where(manga => 
                !manga.Tags.Any(x => 
                    blacklisted.Contains(x.NormalizeValue)))
            .Select(x => x.Id)
            .ToArrayAsync();
    }

    public async Task<MangaEntry?> GetRandomAsync(
        SearchMangaFilter? filter = null, 
        Blacklist? blacklist = null)
    {
        IQueryable<MangaEntry> queryable = _context.Mangas
            .OrderByDescending(x => x.CreatedAt);

        if (blacklist is not null)
        {
            var blacklisted = GetBlacklistedTags(blacklist);
            queryable = queryable
                .Where(manga => !manga.Tags.Any(x => blacklisted.Contains(x.NormalizeValue)));
        }
        
        if (filter?.Format is not null)
        {
            var format = filter.Format;
            queryable = queryable.Where(x => x.Formats.Any(f => f.Type == format));
        }
        
        if (filter?.ExceptIds.Any() ?? false)
        {
            var except = filter.ExceptIds;
            queryable = queryable.Where(x => !except.Contains(x.Id));
        }

        var ids = await queryable.Select(x => x.Id).ToListAsync();

        if (!ids.Any())
        {
            return null;
        }
        
        _randomizer.Shuffle(ids);
        return await GetByIdAsync(ids.First());
    }

    private static string[] GetBlacklistedTags(Blacklist? blacklist) =>
        blacklist?.Tags.Select(x => x.NormalizeValue!).ToArray()
        ?? Array.Empty<string>();

    public Task<MangaEntry?> FindBySourceIdAsync(string id)
    {
        return _context.Mangas
            .IncludeDependencies()
            .FirstOrDefaultAsync(x =>
                x.ExternalMeta != null && x.ExternalMeta.SourceId == id);
    }

    public Task<MangaEntry?> FindBySourceUrlAsync(string url, FormatType? format = null)
    {
        var queryable = _context.Mangas
            .IncludeDependencies()
            .Where(x => x.ExternalMeta.SourceUrl == url);

        if (format != null)
        {
            queryable = queryable.Where(x => x.Formats.Any(f => f.Type == format));
        }

        return queryable.FirstOrDefaultAsync();
    }

    public Task<MangaEntry?> FindByContentKeyAsync(string contentKey, FormatType? format = null)
    {
        var queryable = _context.Mangas
            .IncludeDependencies()
            .Where(x => x.ContentKey == contentKey);

        if (format != null)
        {
            queryable = queryable.Where(x => x.Formats.Any(f => f.Type == format));
        }
        
        return queryable.FirstOrDefaultAsync();
    }

    public async Task<MangaEntry> AddFormatAsync(MangaEntry manga, Format format)
    {
        if (manga.Formats == null)
        {
            await _context.Entry(manga).Collection(nameof(manga.Formats)).LoadAsync();
            manga.Formats ??= new List<Format>();
        }
        
        manga.Formats.Add(format);
        await _context.SaveChangesAsync();

        return manga;
    }
}