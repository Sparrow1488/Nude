using AutoMapper;
using Nude.API.Contracts.Parsing.Responses;
using Nude.Models.Requests;

namespace Nude.Mapping.Profiles;

public class RequestsProfile : Profile
{
    public RequestsProfile()
    {
        CreateMap<ParsingRequest, ParsingResponse>()
            .ForMember(x => x.IsAlreadyExists, opt => opt.MapFrom(x => x.Status == Status.Success))
            .ForMember(x => x.MangaUrl, opt => opt.MapFrom(x => x.Url));
    }
}