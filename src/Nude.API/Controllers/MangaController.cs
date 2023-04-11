using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nude.API.Contracts.Manga.Responses;
using Nude.API.Models.Formats;
using Nude.API.Models.Mangas;
using Nude.API.Services.Mangas;

namespace Nude.API.Controllers;

[ApiController, Route("manga")]
public class MangaController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMangaService _service;

    public MangaController(
        IMapper mapper,
        IMangaService service)
    {
        _mapper = mapper;
        _service = service;
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
    
    [HttpGet("random")]
    public async Task<IActionResult> GetRandom(FormatType? format)
    {
        var manga = await _service.GetRandomAsync(format);

        if (manga != null)
        {
            return Ok(_mapper.Map<MangaResponse>(manga));
        }

        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> FindBy(string? sourceUrl, string? contentKey, FormatType? format)
    {
        MangaEntry? mangaEntry = null;
        
        if(!string.IsNullOrWhiteSpace(sourceUrl))
            mangaEntry = await _service.FindBySourceUrlAsync(sourceUrl, format);
        if(!string.IsNullOrWhiteSpace(contentKey))
            mangaEntry = await _service.FindByContentKeyAsync(contentKey, format);

        if (mangaEntry != null)
        {
            return Ok(_mapper.Map<MangaResponse>(mangaEntry));
        }

        return NotFound();
    }
}