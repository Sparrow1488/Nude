using AutoMapper;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Models.Tickets;
using Nude.Models.Tickets.Parsing;

namespace Nude.Mapping.Profiles;

public class ParsingTicketsProfile : Profile
{
    public ParsingTicketsProfile()
    {
        CreateMap<ParsingTicket, ParsingResponse>()
            .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status.ToString()))
            .ForMember(x => x.Subscribers, opt => opt.MapFrom(x => x.Subscribers.Count));

        CreateMap<ParsingResult, ParsingResultResponse>();
        CreateMap<ParsingMeta, ParsingMetaResponse>()
            .ForMember(x => x.EntityType, opt => opt.MapFrom(x => x.EntityType.ToString()));
    }
}