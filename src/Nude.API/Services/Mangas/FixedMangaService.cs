using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Extensions;
using Nude.API.Infrastructure.Managers;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Models.Urls;
using Nude.API.Services.Mangas.Results;
using Nude.Data.Infrastructure.Contexts;

// ReSharper disable file ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
// ReSharper disable file PossibleMultipleEnumeration
// ReSharper disable file InvertIf


namespace Nude.API.Services.Mangas;

public class FixedMangaService : IFixedMangaService
{
    private readonly FixedAppDbContext _context;
    private readonly ITagManager _tagManager;

    public FixedMangaService(
        FixedAppDbContext context,
        ITagManager tagManager)
    {
        _context = context;
        _tagManager = tagManager;
    }

    public async Task<MangaCreationResult> CreateAsync(
        string title, 
        string description, 
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
            Images = mangaImages
        };

        if (!string.IsNullOrWhiteSpace(externalSourceUrl))
        {
            entry.ExternalMeta = new MangaExternalMeta
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

    public async Task<MangaTagsAdditionResult> AddTagsAsync(MangaEntry manga, IEnumerable<Tag> tags)
    {
        if (manga.Tags is null)
        {
            await _context.Entry(manga).Collection(nameof(manga.Tags)).LoadAsync();
            manga.Tags ??= new List<Tag>();
        }
        manga.Tags.AddRange(tags);
        await _context.SaveChangesAsync();
        
        return new MangaTagsAdditionResult { IsSuccess = true };
    }

    public Task<MangaEntry?> GetByIdAsync(int id)
    {
        return _context.Mangas
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<MangaEntry?> FindBySourceIdAsync(string id)
    {
        return _context.Mangas
            .AsQueryable()
            .IncludeDependencies()
            .FirstOrDefaultAsync(x =>
                x.ExternalMeta != null && x.ExternalMeta.SourceId == id);
    }
}

public static class MangaEntryExtensions
{
    public static IQueryable<MangaEntry> IncludeDependencies(this IQueryable<MangaEntry> queryable)
    {
        return queryable
            .Include(x => x.Formats)
            .Include(x => x.Tags)
            .Include(x => x.Images)
            .ThenInclude(x => x.Url)
            .Include(x => x.ExternalMeta);
    }
}