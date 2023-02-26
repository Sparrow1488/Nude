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
    private readonly ILogger<ParsingJob> _logger;

    public ParsingJob(
        AppDbContext context, 
        INudeParser parser, 
        IMangaRepository repository,
        ILogger<ParsingJob> logger)
    {
        _context = context;
        _parser = parser;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Parsing started");
        
        var request = await _context.ParsingRequests
            .FirstOrDefaultAsync(x => x.Status == Status.Processing);

        if (request is null)
        {
            _logger.LogInformation("No Requests");
            return;
        }
        
        var similarRequests = await _context.ParsingRequests
            .Where(x => x.Url == request.Url)
            .ToListAsync();
        
        _logger.LogInformation(
            "Found similar url ({url}) requests: {similarCount}",
            request.Url,
            similarRequests.Count);

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
        similarRequests.ForEach(x => x.Status = Status.Success);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation(
            "Success parsed items: {count}",
            similarRequests.Count + 1);
    }
}