using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Repositories;
using Nude.Models.Requests;
using Nude.Models.Sources;
using Nude.Providers;
using Quartz;

namespace Nude.API.Jobs;

public sealed class ParsingJob : IJob
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;
    private readonly IMangaRepository _repository;

    public ParsingJob(AppDbContext context, INudeParser parser, IMangaRepository repository)
    {
        _context = context;
        _parser = parser;
        _repository = repository;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var request = await _context.ParsingRequests
            .FirstOrDefaultAsync(x => x.Status == Status.Processing);

        if (request is null) return;

        var exManga = await _parser.GetByUrlAsync(request.Url);

        await _repository.AddAsync(
            exManga.ExternalId,
            exManga.Title, 
            exManga.Description, 
            exManga.Tags, 
            exManga.Images, 
            exManga.Likes, 
            "Unknown",
            SourceType.NudeMoon, 
            request.Url);
        await _repository.SaveAsync();

        request.Status = Status.Success;
        await _context.SaveChangesAsync();
    }
}