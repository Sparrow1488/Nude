using AutoMapper;
using Nude.API.Contracts.Images.Responses;
using Nude.API.Models.Images;

namespace Nude.Mapping.Profiles;

public class ImagesProfile : Profile
{
    public ImagesProfile()
    {
        CreateMap<ImageEntry, ImageResponse>()
            .ForMember(x => x.External, opt => opt.MapFrom(x => x.ExternalMeta));
    }
}