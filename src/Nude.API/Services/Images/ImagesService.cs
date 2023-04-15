using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Nude.API.Infrastructure.Managers;
using Nude.API.Models.Images;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;
using Nude.API.Services.Images.Models;
using Nude.API.Services.Images.Results;
using Nude.Data.Infrastructure.Contexts;
using Nude.Data.Infrastructure.Extensions;

// ReSharper disable file PossibleMultipleEnumeration

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
    
    public async Task<ImageCreationResult> CreateAsync(ImageCreationModel model)
    {
        var result = await CreateRangeAsync(new[] { model });
        return new ImageCreationResult
        {
            IsSuccess = true, 
            Exception = result.Exception,
            Result = result.Result?.FirstOrDefault() 
        };
    }

    public async Task<ImageRangeCreationResult> CreateRangeAsync(IEnumerable<ImageCreationModel> models)
    {
        var images = new List<ImageEntry>();
        foreach (var model in models)
        {
            var image = new ImageEntry
            {
                Url = model.Url,
                ContentKey = model.ContentKey,
                ExternalMeta = new ExternalMeta
                {
                    SourceId = model.ExternalSourceId,
                    SourceUrl = model.ExternalSourceUrl
                },
                Tags = await CombineTagsAsync(model.Author, model.Tags)
            };

            images.Add(image);
        }

        if (models.Any())
        {
            await _context.AddRangeAsync(images);
            await _context.SaveChangesAsync();
        }

        return new ImageRangeCreationResult
        {
            IsSuccess = true,
            Result = images
        };
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

    public async Task<ICollection<ImageEntry>> FindAsync(IEnumerable<string> contentKeys)
    {
        var keysArray = contentKeys.ToArray();
        return await _context.Images
            .IncludeDependencies()
            .Where(x => keysArray.Contains(x.ContentKey))
            .ToListAsync();
    }

    public Task<ICollection<ImageEntry>> FindByTagsAsync(IEnumerable<string> tags)
    {
        throw new NotImplementedException();
    }
}