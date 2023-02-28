using Microsoft.EntityFrameworkCore;
using Nude.API.Data.Contexts;
using Nude.API.Data.Repositories;
using Nude.API.Infrastructure.Constants;
using Nude.Models.Mangas;
using Nude.Models.Sources;
using Nude.Models.Tickets;
using Nude.Parsers;
using Quartz;

namespace Nude.API.Background;

public sealed class ParsingBgService : IJob
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private AppDbContext _context = null!;
    private readonly INudeParser _parser;
    private readonly IMangaRepository _repository;
    private readonly ILogger<ParsingBgService> _logger;

    private DateTimeOffset _processStartedAt = DateTimeOffset.Now;
    
    public ParsingBgService(
        IDbContextFactory<AppDbContext> dbContextFactory,
        INudeParser parser, 
        IMangaRepository repository,
        ILogger<ParsingBgService> logger)
    {
        _dbContextFactory = dbContextFactory;
        _parser = parser;
        _repository = repository;
        _logger = logger;
    }

    public ParsingTicket? Ticket { get; set; }

    public async Task Execute(IJobExecutionContext context)
    {
        while (true)
        {
            try
            {
                _processStartedAt = DateTimeOffset.UtcNow;
                _context = await _dbContextFactory.CreateDbContextAsync();

                await Execute();
                
                await Task.Delay(TimeSpan.FromSeconds(3));
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
                    
                        // TODO: Send callback
                    }
                }

                await _context.DisposeAsync();
            }
        }
    }
    
    private async Task Execute()
    {
        _logger.LogInformation("Check parsing tickets");

        Ticket = await _context.ParsingTickets
            .Include(x => x.Meta)
            .Include(x => x.Result)
            .Include(x => x.Subscribers)
            .ThenInclude(x => x.FeedBackInfo)
            .FirstOrDefaultAsync(x => x.Status == ParsingStatus.WaitToProcess);
            
        if (Ticket is null)
        {
            _logger.LogInformation("No Requests");
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

    private async Task OnSuccessProcessTicketAsync(ParsingTicket ticket, Manga manga)
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