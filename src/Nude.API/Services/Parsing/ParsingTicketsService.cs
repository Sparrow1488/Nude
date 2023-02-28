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
        // Now I can parse only from this source
        if (!request.SourceUrl.Contains("nude-moon.org"))
        {
            throw new BadRequestException("Data from this source cannot be retrieved");
        }
        
        var externalSourceId = _parser.Helper.GetIdFromUrl(request.SourceUrl);
        var ticket = new ParsingTicket
        {
            Meta = new ParsingMeta
            {
                SourceUrl = request.SourceUrl,
                SourceItemId = externalSourceId,
                EntityType = ParsingEntityType.Manga
            },
            Result = new ParsingResult
            {
                StatusCode = "parsing.waiting",
                Message = "Wait to process"
            },
            Status = ParsingStatus.Processing,
            Subscribers = new List<Subscriber>()
        };
        
        if(!string.IsNullOrWhiteSpace(request.CallbackUrl))
            AddSubscriber(ticket, request.CallbackUrl);

        var similarTicket = await _context.ParsingTickets
            .Include(x => x.Meta)
            .Include(x => x.Result)
            .Include(x => x.Subscribers)
            .ThenInclude(x => x.FeedBackInfo)
            .FirstOrDefaultAsync(x => 
                x.Meta.EntityType == ticket.Meta.EntityType &&
                x.Meta.SourceItemId == ticket.Meta.SourceItemId);

        if (similarTicket is null)
        {
            await _context.ParsingTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ParsingResponse>(ticket);
        }
        
        return _mapper.Map<ParsingResponse>(similarTicket);
    }

    private static void AddSubscriber(ParsingTicket ticket, string callbackUrl)
    {
        ticket.Subscribers.Add(new Subscriber
        {
            NotifyStatus = NotifyStatus.OnSuccess,
            FeedBackInfo = new FeedBackInfo
            {
                CallbackUrl = callbackUrl
            }
        });
    }

    public async Task<ParsingResponse> GetTicketAsync(string id)
    {
        var request = await _context.ParsingTickets
            .FirstOrDefaultAsync(x => x.Id.ToString() == id)
            ?? throw new NotFoundException("Request not found", id, "ParsingRequest");

        return _mapper.Map<ParsingResponse>(request);
    }
}