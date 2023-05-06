using AutoMapper;
using Nude.API.Contracts.Blacklists.Responses;
using Nude.API.Models.Blacklists;

namespace Nude.Mapping.Profiles;

public class BlacklistProfile : Profile
{
    public BlacklistProfile()
    {
        CreateMap<Blacklist, BlacklistResponse>();
    }
}