using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Exceptions;
using Nude.Models.Tickets;
using Nude.Parsers;

namespace Nude.API.Services.Parsing;

public class ParsingTicketsService : IParsingTicketsService
{
    private readonly AppDbContext _context;
    private readonly INudeParser _parser;
    private readonly IMapper _mapper;

    public ParsingTicketsService(
        AppDbContext context, 
        INudeParser parser,
        IMapper mapper)
    {
        _context = context;
        _parser = parser;
        _mapper = mapper;
    }
    
    public async Task<ParsingResponse> CreateTicketAsync(ParsingCreateRequest request)
    {
        var externalMangaId = _parser.Helper.GetIdFromUrl(request.MangaUrl);
        var ticket = new ParsingTicket
        {
            Url = request.MangaUrl,
            Status = Status.Failed
        };

        #region Check manga exists

        var isMangaExists = await _context.Mangas.AnyAsync(x => x.ExternalId == externalMangaId);
        if (isMangaExists)
        {
            ticket.Status = Status.Success;
            var response = _mapper.Map<ParsingResponse>(ticket);
            return response;
        }

        #endregion

        #region Check similar requests

        var isSimilarRequestExists = await _context.ParsingTickets
            .AnyAsync(x => x.ExternalId == externalMangaId && x.Status == Status.Processing);
        if (isSimilarRequestExists)
        {
            ticket.Status = Status.Processing;
            ticket.Message = "Similar request waiting for processing";
            var response = _mapper.Map<ParsingResponse>(ticket);
            return response;
        }

        #endregion

        #region Save request

        ticket.UniqueId = Guid.NewGuid().ToString();
        ticket.Status = Status.Processing;
        ticket.ExternalId = externalMangaId;
        await _context.AddAsync(ticket);
        await _context.SaveChangesAsync();

        #endregion
        
        return _mapper.Map<ParsingResponse>(ticket);
    }

    public async Task<ParsingResponse> GetRequestAsync(string uniqueId)
    {
        var request = await _context.ParsingTickets
            .FirstOrDefaultAsync(x => x.UniqueId == uniqueId)
            ?? throw new NotFoundException("Request not found", uniqueId, "ParsingRequest");

        return _mapper.Map<ParsingResponse>(request);
    }
}