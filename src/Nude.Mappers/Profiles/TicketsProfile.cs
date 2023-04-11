using AutoMapper;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Tickets;

namespace Nude.Mapping.Profiles;

public class TicketsProfile : Profile
{
    public TicketsProfile()
    {
        CreateMap<ContentTicket, ContentTicketResponse>();
    }
}