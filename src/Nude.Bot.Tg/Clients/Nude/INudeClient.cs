using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Contracts.Tickets.Requests;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Formats;

namespace Nude.Bot.Tg.Clients.Nude;

public interface INudeClient
{
    Task<NewMangaResponse?> GetMangaByIdAsync(int id);
    Task<NewMangaResponse?> GetMangaByUrlAsync(string sourceUrl,FormatType format);
    Task<NewMangaResponse?> GetRandomMangaAsync();
    Task<ContentTicketResponse?> CreateContentTicket(ContentTicketRequest request);
    Task<ContentTicketResponse?> GetContentTicketById(int id);
    Task<FormatTicketResponse?> CreateFormatTicket(FormatTicketRequest request);
    Task<FormatTicketResponse?> GetFormatTicketById(int id);
}