using AutoMapper;
using Nude.API.Contracts.Manga.Responses;
using Nude.Models.Mangas;

namespace Nude.Mapping.Profiles;

public class MangaProfile : Profile
{
    public MangaProfile()
    {
        CreateMap<Manga, MangaResponse>()
            .ForMember(x => x.Tags, opt => opt.MapFrom(x => x.Tags.Select(x => x.Value)))
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.Select(x => x.Url.Value)))
            .ForMember(x => x.Author, opt => opt.MapFrom(x => x.Author.Name));
    }
}