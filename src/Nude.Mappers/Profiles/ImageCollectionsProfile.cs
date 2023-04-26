using AutoMapper;
using Nude.API.Contracts.Collections.Responses;
using Nude.API.Models.Collections;

namespace Nude.Mapping.Profiles;

public class ImageCollectionsProfile : Profile
{
    public ImageCollectionsProfile()
    {
        CreateMap<ImageCollection, ImageCollectionResponse>()
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.Count));
    }
}