using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Managers;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Services.Images.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

namespace Nude.API.Services.Images;

public class ImagesService : IImagesService
{
    private readonly AppDbContext _context;
    private readonly ITagManager _tagManager;

    public ImagesService(
        AppDbContext context,
        ITagManager tagManager)
    {
        _context = context;
        _tagManager = tagManager;
    }
    
    public async Task<ImageCreationResult> CreateAsync(
        string url, 
        string contentKey, 
        IEnumerable<string>? tags = null, 
        string? author = null,
        string? externalSourceId = null, 
        string? externalSourceUrl = null)
    {
        var image = new ImageEntry
        {
            Url = url,
            ContentKey = contentKey,
            ExternalMeta = new ExternalMeta
            {
                SourceId = externalSourceId,
                SourceUrl = externalSourceUrl
            },
            Tags = await CombineTagsAsync(author, tags)
        };

        await _context.AddAsync(image);
        await _context.SaveChangesAsync();

        return new ImageCreationResult { IsSuccess = true, Result = image };
    }

    public Task<bool> ExistsAsync(string contentKey)
    {
        return _context.Images.AnyAsync(x => x.ContentKey == contentKey);
    }

    private async Task<ICollection<Tag>> CombineTagsAsync(string? author, IEnumerable<string>? trivia)
    {
        var tagsList = new List<Tag>();
        if (trivia != null)
        {
            var triviaTags = await _tagManager.AddRangeAsync(trivia, TagType.Trivia);
            tagsList.AddRange(triviaTags);
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            var authorTag = await _tagManager.AddAsync(author, TagType.Author);
            tagsList.Add(authorTag);
        }

        return tagsList;
    }

    public Task<ImageEntry?> GetByIdAsync(int id)
    {
        return _context.Images
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ImageEntry?> GetByContentKeyAsync(string contentKey)
    {
        return _context.Images
            .IncludeDependencies()
            .FirstOrDefaultAsync(x => x.ContentKey == contentKey);
    }

    public Task<ICollection<ImageEntry>> FindByTagsAsync(IEnumerable<string> tags)
    {
        throw new NotImplementedException();
    }
}