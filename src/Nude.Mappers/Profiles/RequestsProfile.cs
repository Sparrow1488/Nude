using AutoMapper;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Models.Tickets;

namespace Nude.Mapping.Profiles;

public class RequestsProfile : Profile
{
    public RequestsProfile()
    {
        CreateMap<ParsingTicket, ParsingResponse>()
            .ForMember(x => x.IsAlreadyExists, opt => opt.MapFrom(x => x.Status == Status.Success))
            .ForMember(x => x.MangaUrl, opt => opt.MapFrom(x => x.Url));
    }
}