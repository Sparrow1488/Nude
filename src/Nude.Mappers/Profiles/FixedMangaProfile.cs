using AutoMapper;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;

namespace Nude.Mapping.Profiles;

public class FixedMangaProfile : Profile
{
    public FixedMangaProfile()
    {
        CreateMap<MangaEntry, NewMangaResponse>()
            .ForMember(x => x.External, opt => opt.MapFrom(x => x.ExternalMeta))
            .ForMember(x => x.Formats, opt => opt.MapFrom(x => x.Formats))
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.Select(x => x.Url.Value)));

        CreateMap<MangaExternalMeta, MangaExternalMetaResponse>();
        
        CreateMap<Tag, TagResponse>();
        
        CreateMap<FormattedContent, FormattedContentResponse>()
            .IncludeAllDerived();
        
        CreateMap<TelegraphContent, TelegraphContentResponse>();
    }
}