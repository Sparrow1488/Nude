using AutoMapper;
using Nude.API.Contracts.Formats.Responses;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Contracts.Metas.Responses;
using Nude.API.Contracts.Tags.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Tags;

namespace Nude.Mapping.Profiles;

public class MangaProfile : Profile
{
    public MangaProfile()
    {
        CreateMap<MangaEntry, MangaResponse>()
            .ForMember(x => x.External, opt => opt.MapFrom(x => x.ExternalMeta))
            .ForMember(x => x.Formats, opt => opt.MapFrom(x => x.Formats))
            .ForMember(x => x.Images, opt => opt.MapFrom(x => x.Images.Select(x => x.Url.Value)));

        CreateMap<ExternalMeta, ExternalMetaResponse>();
        
        CreateMap<Tag, TagResponse>();
        
        CreateMap<Format, FormatResponse>()
            .IncludeAllDerived();
        
        CreateMap<TelegraphFormat, TelegraphFormatResponse>();
    }
}