using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nude.API.Contracts.Parsing.Requests;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Data.Infrastructure.Contexts;
using Nude.API.Infrastructure.Constants;
using Nude.API.Infrastructure.Exceptions;
using Nude.API.Services.Resolvers;
using Nude.Models.Tickets.Parsing;

namespace Nude.API.Services.Parsing;

public class ParsingTicketsService : IParsingTicketsService
{
    private readonly AppDbContext _context;
    private readonly IMangaParserResolver _parserResolver;
    private readonly IMapper _mapper;

    public ParsingTicketsService(
        AppDbContext context,
        IMangaParserResolver parserResolver,
        IMapper mapper)
    {
        _context = context;
        _parserResolver = parserResolver;
        _mapper = mapper;
    }
    
    public async Task<ParsingResponse> CreateTicketAsync(ParsingCreateRequest request)
    {
        if (!AvailableSources.IsAvailable(request.SourceUrl))
        {
            throw new BadRequestException("Data from this source cannot be retrieved");
        }

        var parser = await _parserResolver.ResolveByUrlAsync(request.SourceUrl);
        var externalSourceId = parser.Helper.GetIdFromUrl(request.SourceUrl);
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
        
        if(IsSubscribedToNotification(request))
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
        if (IsSubscribedToNotification(request) && !alreadySubscribed)
        {
            AddSubscriber(similarTicket, request.CallbackUrl!);
            _context.Update(similarTicket);
            await _context.SaveChangesAsync();
        }
        
        return _mapper.Map<ParsingResponse>(similarTicket);
    }

    private static bool IsSubscribedToNotification(ParsingCreateRequest request)
    {
        return !string.IsNullOrWhiteSpace(request.CallbackUrl);
    }

    private static void AddSubscriber(ParsingTicket ticket, string callbackUrl)
    {
        ticket.Subscribers.Add(new Subscriber
        {
            NotifyStatus = NotifyStatus.All,
            FeedBackInfo = new FeedBackInfo
            {
                CallbackUrl = callbackUrl
            }
        });
    }

    public async Task<ParsingResponse> GetTicketAsync(int id)
    {
        var request = await _context.ParsingTickets
            .Include(x => x.Meta)
            .Include(x => x.Result)
            .Include(x => x.Subscribers)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new NotFoundException("Ticket not found", id.ToString(), "ParsingTicket");

        return _mapper.Map<ParsingResponse>(request);
    }
}