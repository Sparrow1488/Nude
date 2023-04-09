using AutoMapper;
using Nude.API.Contracts.Tickets.Responses;
using Nude.API.Models.Tickets;
using Nude.API.Models.Tickets.Contexts;
using Nude.API.Models.Tickets.Results;

namespace Nude.Mapping.Profiles;

public class TicketsProfile : Profile
{
    public TicketsProfile()
    {
        CreateMap<ContentTicketContext, TicketContextResponse>();
        
        #region Content Tickets

        CreateMap<ContentTicket, ContentTicketResponse>();

        CreateMap<ContentResult, ContentResponse>();

        #endregion

        #region Content Format Tickets

        CreateMap<ContentFormatTicket, FormatTicketResponse>();

        CreateMap<ContentFormatTicketContext, FormatTicketContextResponse>();

        #endregion
    }
}