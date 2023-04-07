using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Infrastructure.Services.Resolvers;
using Nude.API.Models.Tags;
using Nude.API.Services.Mangas;
using Nude.API.Services.Mangas.Results;
using Nude.API.Services.Tickets;
using Nude.Data.Infrastructure.Contexts;
using Nude.Models;

namespace Nude.API.Services.Workers;

public class ContentTicketsWorker : IBackgroundWorker
{
    private readonly FixedAppDbContext _context;
    private readonly IFixedMangaService _mangaService;
    private readonly IContentTicketService _ticketService;
    private readonly IMangaParserResolver _parserResolver;
    private readonly ILogger<ContentTicketsWorker> _logger;

    public ContentTicketsWorker(
        FixedAppDbContext context,
        IFixedMangaService mangaService,
        IMangaParserResolver parserResolver,
        IContentTicketService ticketService,
        ILogger<ContentTicketsWorker> logger)
    {
        _context = context;
        _mangaService = mangaService;
        _ticketService = ticketService;
        _parserResolver = parserResolver;
        _logger = logger;
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        var ticket = await _ticketService.GetWaitingAsync();

        if (ticket == null)
        {
            _logger.LogInformation("No waiting tickets");
            return;
        }
        
        // IF RESOURCE IS MANGA RESOURCE
        var contentUrl = ticket.Context.ContentUrl;
        if (AvailableSources.IsAvailable(contentUrl))
        {
            var parser = await _parserResolver.ResolveByUrlAsync(contentUrl);
            var result = await parser.GetByUrlAsync(contentUrl);

            var creationResult = await _mangaService.CreateAsync(
                result.Title, 
                result.Description, 
                result.Images,
                externalSourceUrl: contentUrl,
                externalSourceId: parser.Helper.GetIdFromUrl(contentUrl)
            );
            
            LogResult(creationResult);

            // TODO: add tags
            // await _mangaService.AddTagsAsync(creationResult.Entry.Id, result.Tags);
        }
    }

    private void LogResult(MangaCreationResult result)
    {
        if (result.IsSuccess)
        {
            // TODO: receive stats (time)
            _logger.LogInformation("Entry created success");
        }
        else
        {
            _logger.LogError(
                result.Exception,
                "Entry creation failed {reason}",
                result.Exception!.Message);
        }
    }
    
}