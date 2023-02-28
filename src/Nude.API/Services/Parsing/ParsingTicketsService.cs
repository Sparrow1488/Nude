using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.API.Data.Contexts;
using Nude.API.Infrastructure.Constants;
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
                StatusCode = ParsingResultCodes.Waiting,
                Message = "Wait to process"
            },
            Status = ParsingStatus.WaitToProcess,
            Subscribers = new List<Subscriber>()
        };
        
        if(WantSubscribe(request))
            AddSubscriber(ticket, request.CallbackUrl!);

        var similarTicket = await _context.ParsingTickets
            .Include(x => x.Meta)
            .Include(x => x.Result)
            .Include(x => x.Subscribers)
            .ThenInclude(x => x.FeedBackInfo)
            .FirstOrDefaultAsync(x => 
                x.Meta.EntityType == ticket.Meta.EntityType &&
                x.Meta.SourceItemId == ticket.Meta.SourceItemId &&
                x.Status != ParsingStatus.Failed);

        if (similarTicket is null)
        {
            await _context.ParsingTickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ParsingResponse>(ticket);
        }
        
        var alreadySubscribed = similarTicket.Subscribers
            .Any(x => x.FeedBackInfo.CallbackUrl == request.CallbackUrl);
        if (WantSubscribe(request) && !alreadySubscribed)
        {
            AddSubscriber(similarTicket, request.CallbackUrl!);
            _context.Update(similarTicket);
            await _context.SaveChangesAsync();
        }
        
        return _mapper.Map<ParsingResponse>(similarTicket);
    }

    private static bool WantSubscribe(ParsingCreateRequest request)
    {
        return !string.IsNullOrWhiteSpace(request.CallbackUrl);
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

    public async Task<ParsingResponse> GetTicketAsync(int id)
    {
        var request = await _context.ParsingTickets
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("Ticket not found", id.ToString(), "ParsingTicket");

        return _mapper.Map<ParsingResponse>(request);
    }
}