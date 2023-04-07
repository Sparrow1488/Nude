using AutoMapper;
using Nude.API.Contracts.Tickets;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;

namespace Nude.Mapping.Profiles;

public class TicketsProfile : Profile
{
    public TicketsProfile()
    {
        CreateMap<ContentTicket, ContentTicketResponse>();

        CreateMap<ContentResult, ContentResponse>();
        CreateMap<TicketContext, TicketContextResponse>();
    }
}