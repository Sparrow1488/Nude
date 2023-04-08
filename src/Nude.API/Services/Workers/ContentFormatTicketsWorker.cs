using Nude.API.Infrastructure.Services.Background;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Tickets.States;
using Nude.API.Services.Formatters;
using Nude.API.Services.Mangas;
using Nude.API.Services.Tickets;

#region Rider annotations

// ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

#endregion

namespace Nude.API.Services.Workers;

public class ContentFormatTicketsWorker : IBackgroundWorker
{
    private readonly IMangaService _mangaService;
    private readonly IContentFormatTicketService _ticketService;
    private readonly IContentFormatterService _formatterService;
    private readonly ILogger<ContentFormatTicketsWorker> _logger;

    public ContentFormatTicketsWorker(
        IMangaService mangaService,
        IContentFormatTicketService ticketService,
        IContentFormatterService formatterService,
        ILogger<ContentFormatTicketsWorker> logger)
    {
        _mangaService = mangaService;
        _ticketService = ticketService;
        _formatterService = formatterService;
        _logger = logger;
    }
    
    public async Task ExecuteAsync(BackgroundServiceContext ctx, CancellationToken ctk)
    {
        var ticket = await _ticketService.GetWaitingAsync();

        if (ticket == null)
        {
            _logger.LogDebug("No waiting format-tickets");
            return;
        }
        
        _logger.LogInformation("Starting process format-ticket {id}", ticket.Id);

        if (ticket.Context.EntityType == nameof(MangaEntry))
        {
            var manga = await _mangaService.GetByIdAsync(int.Parse(ticket.Context.EntityId));
            var images = manga!.Images.Select(x => x.Url.Value);
            
            images = new List<string>() { images.First() }; // TODO: У МЕНЯ ХУЕВЫЙ ИНЕТ!!! УДАЛИТЬ!!!

            var format = await _formatterService.FormatAsync(
                manga.Title,
                "With Love By Nude",
                images,
                FormatType.Telegraph);

            await _mangaService.AddFormatAsync(manga, format);

            await _ticketService.UpdateStatusAsync(ticket, FormattingStatus.Success);
            await _ticketService.UpdateResultAsync(ticket, format);
            
            return;
        }

        throw new NotImplementedException();
    }
}