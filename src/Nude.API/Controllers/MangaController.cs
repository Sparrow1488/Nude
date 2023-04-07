using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Models.Mangas.Meta;
using Nude.API.Models.Urls;
using Nude.API.Services.Manga;
using Nude.API.Services.Mangas;
using Nude.Data.Infrastructure.Contexts;

namespace Nude.API.Controllers;

[ApiController, Route("manga")]
public class MangaController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly FixedAppDbContext _context;
    private readonly IFixedMangaService _service;

    public MangaController(
        IMapper mapper,
        FixedAppDbContext context,
        IFixedMangaService service)
    {
        _mapper = mapper;
        _context = context;
        _service = service;
    }

    [HttpPost]
    public async Task Create()
    {
        var manga = new MangaEntry
        {
            Title = "Test",
            Description = "Lorem ipsum",
            ExternalMeta = new MangaExternalMeta
            {
                SourceId = "1488",
                SourceUrl = "https://huina.com/mangas/1"
            },
            Images = new List<MangaImage>
            {
                new() { Url = new Url { Value = "img_1.jpg" }},
                new() { Url = new Url { Value = "img_2.jpg" }},
                new() { Url = new Url { Value = "img_3.jpg" }}
            },
            Formats = new List<FormattedContent>
            {
                new TelegraphContent { Url = "https://telegra.ph/sex-1" }
            }
        };

        await _context.AddAsync(manga);
        await _context.SaveChangesAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var manga = await _service.GetByIdAsync(id);

        if (manga != null)
        {
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> FindBySourceId(string sourceId)
    {
        var manga = await _service.FindBySourceIdAsync(sourceId);

        if (manga != null)
        {
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return NotFound();
    }
}