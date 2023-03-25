using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Repositories;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Services.Background;
using Nude.API.Infrastructure.Services.FeedBack;
using Nude.Models.Sources;
using Nude.Models.Tickets.Parsing;
using Nude.Parsers.NudeMoon;

namespace Nude.API.Services.Workers;

public sealed class ParsingBackgroundWorker : IBackgroundWorker
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;
    private readonly IMangaRepository _repository;
    private readonly IFeedBackService _feedBack;
    private readonly ILogger<ParsingBackgroundWorker> _logger;

    private DateTimeOffset _processStartedAt = DateTimeOffset.Now;
    
    public ParsingBackgroundWorker(
        IMangaRepository repository,
        IFeedBackService feedBack,
        INudeParser parser,
        AppDbContext context,
        ILogger<ParsingBackgroundWorker> logger)
    {
        _repository = repository;
        _feedBack = feedBack;
        _parser = parser;
        _context = context;
        _logger = logger;
    }

    private ParsingTicket? Ticket { get; set; }

    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        try
        {
            _processStartedAt = DateTimeOffset.UtcNow;

            await Execute();
        }
        catch (Exception ex)
        {
            await OnErrorProcessTicketAsync(Ticket!, ex);
        }
        finally
        {
            if (Ticket is not null)
            {
                var notifyStatus = Ticket.Status == ParsingStatus.Success
                    ? NotifyStatus.OnSuccess
                    : NotifyStatus.OnError;

                var subs = Ticket.Subscribers
                    .Where(x => x.NotifyStatus == notifyStatus)
                    .ToList();
            
                if (subs.Any())
                {
                    _logger.LogInformation(
                        "On notify status '{status}' subscribed {subs} users",
                        notifyStatus.ToString(),
                        subs.Count);

                    foreach (var sub in subs)
                    {
                        await _feedBack.SendAsync(Ticket, sub.FeedBackInfo);
                    }
                }
            }
        }
    }
    
    private async Task Execute()
    {
        _logger.LogDebug("Check parsing tickets");

        Ticket = await _context.ParsingTickets
            .Include(x => x.Meta)
            .Include(x => x.Result)
            .Include(x => x.Subscribers)
            .ThenInclude(x => x.FeedBackInfo)
            .FirstOrDefaultAsync(x => x.Status == ParsingStatus.WaitToProcess);
            
        if (Ticket is null)
        {
            _logger.LogDebug("No Requests");
            return;
        }

        await OnProcessParsingTicketAsync(Ticket);
    }

    private async Task OnProcessParsingTicketAsync(ParsingTicket ticket)
    {
        if (ticket.Meta.EntityType != ParsingEntityType.Manga)
        {
            await OnCannotProcessTicketAsync(ticket);
            return;
        }
        
        _logger.LogInformation("Start parsing item...");

        var externalManga = await _parser.GetByUrlAsync(ticket.Meta.SourceUrl);

        var manga = await _repository.AddAsync(externalManga, SourceType.NudeMoon);
        await _repository.SaveAsync();

        await OnSuccessProcessTicketAsync(ticket, manga);
    }

    private async Task OnCannotProcessTicketAsync(ParsingTicket ticket)
    {
        _logger.LogError(
            "ParsingBgServices cannot process this type ({type}) of ticket",
            ticket.Meta.EntityType.ToString());

        ticket.Status = ParsingStatus.Failed;
        ticket.Result.StatusCode = ParsingResultCodes.CannotProcess;
        ticket.Result.Message = "Cannot process this type of ticket";

        await UpdateTicketAsync(ticket);
    }

    private async Task OnSuccessProcessTicketAsync(ParsingTicket ticket, Models.Mangas.Manga manga)
    {
        var processTime = DateTimeOffset.UtcNow - _processStartedAt;
        _logger.LogInformation(
            "Ticket {id} processed success in {time}",
            ticket.Id,
            $"{processTime.Minutes}:{processTime.Seconds} mins");

        ticket.Status = ParsingStatus.Success;
        ticket.Result.StatusCode = ParsingResultCodes.Ok;
        ticket.Result.Message = "Source parsed success";
        ticket.Result.EntityId = manga.Id.ToString();

        await UpdateTicketAsync(ticket);
    }

    private async Task OnErrorProcessTicketAsync(ParsingTicket ticket, Exception ex)
    {
        _logger.LogError(ex, "Failure process request");

        ticket.Status = ParsingStatus.Failed;
        ticket.Result.StatusCode = ParsingResultCodes.Error;
        ticket.Result.Message = ex.Message;

        await UpdateTicketAsync(ticket);
    }

    private async Task UpdateTicketAsync(ParsingTicket ticket)
    {
        _context.Update(ticket);
        await _context.SaveChangesAsync();
    }
}