using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Repositories;
using Nude.Models.Authors;
using Nude.Models.Mangas;
using Nude.Models.Requests;
using Nude.Models.Sources;
using Nude.Models.Tags;
using Nude.Providers;
using Quartz;

namespace Nude.API.Jobs;

public sealed class ParsingJob : IJob
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;

    public ParsingJob(AppDbContext context, INudeParser parser)
    {
        _context = context;
        _parser = parser;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var request = await _context.ParsingRequests
            .FirstOrDefaultAsync(x => x.Status == Status.Processing);

        if (request is null) return;

        var externalManga = await _parser.GetByUrlAsync(request.Url);
        var source = await _context.Sources.FirstOrDefaultAsync(x => x.Type == SourceType.NudeMoon) 
                     ?? new Source { Type = SourceType.NudeMoon };

        var manga = new Manga
        {
            Title = externalManga.Title,
            Description = externalManga.Description,
            Tags = externalManga.Tags.Select(x => new Tag { Value = x, NormalizeValue = "" }).ToList(), // TagsManager
            Author = new Author { Name = "none" }, // Find in db,
            Images = externalManga.Images.Select(x => new MangaImage { Url = new(){ Value = x}}).ToList(),
            Likes = externalManga.Likes,
            Source = source,
            OriginUrl = new() {Value = request.Url}
        };

        request.Status = Status.Success;

        await _context.AddAsync(manga);
        await _context.SaveChangesAsync();
    }
}