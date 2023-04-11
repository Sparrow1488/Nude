using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Managers;
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
    private readonly ITagManager _tagManager;

    public MangaService(
        AppDbContext context,
        ITagManager tagManager)
    {
        _context = context;
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
        
        return new MangaCreationResult { IsSuccess = true, Entry = entry };
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

    public async Task<MangaEntry?> GetRandomAsync(FormatType? format = null)
    {
        var queryable = _context.Mangas.AsQueryable();
        if (format != null)
        {
            queryable = queryable.Where(x => x.Formats.Any(x => x.Type == format));
        }

        var ids = await queryable.Select(x => x.Id).ToListAsync();
        var mangaId = ids[Random.Shared.Next(0, ids.Count - 1)];
        
        return await GetByIdAsync(mangaId);
    }

    public Task<MangaEntry?> FindBySourceIdAsync(string id)
    {
        return _context.Mangas
            .AsQueryable()
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
            queryable = queryable.Where(x => x.Formats.Any(x => x.Type == format));
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
            queryable = queryable.Where(x => x.Formats.Any(x => x.Type == format));
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